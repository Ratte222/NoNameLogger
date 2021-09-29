using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NoNameLogger.Model;
using NoNameLoggerUI.DTO;
using NoNameLoggerUI.Extensions;
using NoNameLoggerUI.Filters;
using NoNameLoggerUI.Helpers;
using NoNameLoggerUI.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NoNameLoggerUI.Middleware
{
    public class NoNameLoggerUiMidlleware
    {
        private const string EmbeddedFileNamespace = "NoNameLoggerUI.wwwroot.dist";
        private readonly string _routePrefix = "NoNameLoggerUI";
        private readonly RequestDelegate _next;
        private readonly StaticFileMiddleware _staticFileMiddleware;
        private readonly JsonSerializerSettings _jsonSerializerOptions;
        private readonly ILogger<NoNameLoggerUiMidlleware> _logger;
        public NoNameLoggerUiMidlleware(RequestDelegate next, IWebHostEnvironment hostingEnv,
            ILoggerFactory loggerFactory, ILogger<NoNameLoggerUiMidlleware> logger)
        {
            _next = next;
            _logger = logger;
            _staticFileMiddleware = CreateStaticFileMiddleware(next, hostingEnv, loggerFactory);
            _jsonSerializerOptions = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.None
            };
        }


        public async Task Invoke(HttpContext httpContext)
        {
            var httpMethod = httpContext.Request.Method;
            var path = httpContext.Request.Path.Value;

            if (httpMethod == "GET" && Regex.IsMatch(path, $"^/{Regex.Escape(_routePrefix)}/api/logs/?$", RegexOptions.IgnoreCase))
            {
                try
                {
                    httpContext.Response.ContentType = "application/json;charset=utf-8";
                    

                    var result = FetchLogsAsync(httpContext);
                    httpContext.Response.StatusCode = (int)HttpStatusCode.OK;
                    await httpContext.Response.WriteAsync(result);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                    httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                    var errorMessage = httpContext.Request.IsLocal()
                        ? JsonConvert.SerializeObject(new { errorMessage = ex.Message })
                        : JsonConvert.SerializeObject(new { errorMessage = "Internal server error" });

                    await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(new { errorMessage }));
                }

                return;
            }

            if (httpMethod == "GET" && Regex.IsMatch(path, $"^/?{Regex.Escape(_routePrefix)}/?$", RegexOptions.IgnoreCase))
            {
                var indexUrl = httpContext.Request.GetEncodedUrl().TrimEnd('/') + "/index.html";
                RespondWithRedirect(httpContext.Response, indexUrl);
                return;
            }

            if (httpMethod == "GET" && Regex.IsMatch(path, $"^/{Regex.Escape(_routePrefix)}/?index.html$", RegexOptions.IgnoreCase))
            {
                await RespondWithIndexHtml(httpContext.Response, httpContext);
                return;
            }

            await _staticFileMiddleware.Invoke(httpContext);
        }

        private void RespondWithRedirect(HttpResponse response, string location)
        {
            response.StatusCode = 301;
            response.Headers["Location"] = location;
        }

        private StaticFileMiddleware CreateStaticFileMiddleware(
            RequestDelegate next,
            IWebHostEnvironment hostingEnv,
            ILoggerFactory loggerFactory)
        {
            var staticFileOptions = new StaticFileOptions
            {
                RequestPath = $"/{_routePrefix}",
                FileProvider = new EmbeddedFileProvider(typeof(NoNameLoggerUiMidlleware).GetTypeInfo().Assembly, EmbeddedFileNamespace),
            };

            return new StaticFileMiddleware(next, hostingEnv, Options.Create(staticFileOptions), loggerFactory);
        }

        private async Task RespondWithIndexHtml(HttpResponse response, HttpContext httpContext)
        {
            response.StatusCode = 200;
            response.ContentType = "text/html;charset=utf-8";

            await using var stream = IndexStream();
            var htmlBuilder = new StringBuilder(await new StreamReader(stream).ReadToEndAsync());
            string nil = "nil";
            htmlBuilder.Replace("%(Configs)", JsonConvert.SerializeObject(
                new { _routePrefix, nil }, _jsonSerializerOptions));

            await response.WriteAsync(htmlBuilder.ToString(), Encoding.UTF8);
        }

        private Func<Stream> IndexStream { get; } = () => typeof(NoNameLoggerUiMidlleware).GetTypeInfo().Assembly
            .GetManifestResourceStream("NoNameLoggerUI.wwwroot.index.html");

        private string CreateTableData(HttpContext httpContext)
        {
            var provider = httpContext.RequestServices.GetService<IDataProvider>();
            (LogFilter logFilter, PageResponse<Log> pageResponse) = GetParameterFromRequest(httpContext);
            pageResponse.Items = provider.FetchLogs(logFilter, pageResponse);
            StringBuilder htmlTableBody = new StringBuilder();
            foreach(var log in pageResponse.Items)
            {
                htmlTableBody.AppendLine("<tr>");
                htmlTableBody.AppendLine($"<td>{log.Id}</td>");
                htmlTableBody.AppendLine($"<td>{log.Level}</td>");
                htmlTableBody.AppendLine($"<td>{log.TimeStamp}</td>");
                htmlTableBody.AppendLine($"<td>{log.Message}</td>");
                //htmlTableBody.AppendLine($"<td>{log.MessageTemplate}</td>");                
                htmlTableBody.AppendLine($"<td>{log.Exception}</td>");
                htmlTableBody.AppendLine($"<td>{log.Properties}</td>");
                htmlTableBody.AppendLine("</tr>");
            }
            return htmlTableBody.ToString();
        }

        private string FetchLogsAsync(HttpContext httpContext)
        {
            var provider = httpContext.RequestServices.GetService<IDataProvider>();
            (LogFilter logFilter, PageResponse<Log> pageResponse) = GetParameterFromRequest(httpContext);
            pageResponse.Items = provider.FetchLogs(logFilter, pageResponse);
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Log, LogDTO>()
            .ForMember(dest => dest.RowNo, opt => opt.MapFrom(scr => scr.Id))
            .ForMember(dest => dest.PropertyType, opt => opt.MapFrom(scr => "xml"))
            .ForMember(dest => dest.Properties, opt => opt.MapFrom(scr => $"<properties>{scr.Properties}</properties>"))
            .ForMember(dest => dest.Level, opt => opt.MapFrom(scr => (LogLevel)Enum.Parse(typeof(NoNameLogger.LogLevel), scr.Level)))
            .ForMember(dest => dest.Timestamp, opt => opt.MapFrom(scr => scr.TimeStamp)));
            var mapper = new Mapper(config);
            var logs = mapper.Map<IEnumerable<Log>, IEnumerable<LogDTO>>(pageResponse.Items);
              long total = provider.CountLogs(logFilter);
            int count = pageResponse.PageLength;
            int currentPage = pageResponse.PageNumber;
            var result = JsonConvert.SerializeObject(new { logs, total,
                count, currentPage }, _jsonSerializerOptions);
            return result;
        }

        private (LogFilter, PageResponse<Log>) GetParameterFromRequest(HttpContext httpContext)
        {
            httpContext.Request.Query.TryGetValue("page", out var pageStr);
            httpContext.Request.Query.TryGetValue("count", out var countStr);
            httpContext.Request.Query.TryGetValue("level", out var levelStr);
            httpContext.Request.Query.TryGetValue("search", out var searchStr);
            httpContext.Request.Query.TryGetValue("startDate", out var startDateStar);
            httpContext.Request.Query.TryGetValue("endDate", out var endDateStar);

            int.TryParse(pageStr, out var currentPage);
            int.TryParse(countStr, out var count);

            DateTime.TryParse(startDateStar, out var startDate);
            DateTime.TryParse(endDateStar, out var endDate);

            if (endDate != default)
                endDate = new DateTime(endDate.Year, endDate.Month, endDate.Day, 23, 59, 59);

            currentPage = currentPage == default ? 1 : currentPage;
            count = count == default ? 10 : count;

            var filter = new LogFilter()
            {
                StartDate = startDate == default ? (DateTime?)null : startDate,
                EndDate = endDate == default ? (DateTime?)null : endDate,
                LevelString = levelStr,
                SearchString = searchStr
            };
            PageResponse<Log> pageResponse = new Helpers.PageResponse<Log>(count, currentPage);
            return (filter, pageResponse);
        }
    }
}
