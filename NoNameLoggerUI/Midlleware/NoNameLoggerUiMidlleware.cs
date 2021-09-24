using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NoNameLoggerUI.Filters;
using NoNameLoggerUI.Interface;
using System;
using System.Collections.Generic;
using System.IO;
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
        public NoNameLoggerUiMidlleware(RequestDelegate next, IWebHostEnvironment hostingEnv,
            ILoggerFactory loggerFactory)
        {
            _next = next;
            _staticFileMiddleware = CreateStaticFileMiddleware(next, hostingEnv, loggerFactory);
        }


        public async Task Invoke(HttpContext httpContext)
        {
            var httpMethod = httpContext.Request.Method;
            var path = httpContext.Request.Path.Value;

            
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
            htmlBuilder.Replace("%(TableData)", CreateTableData(httpContext));

            await response.WriteAsync(htmlBuilder.ToString(), Encoding.UTF8);
        }

        private Func<Stream> IndexStream { get; } = () => typeof(NoNameLoggerUiMidlleware).GetTypeInfo().Assembly
            .GetManifestResourceStream("NoNameLoggerUI.wwwroot.index.html");

        private string CreateTableData(HttpContext httpContext)
        {
            var provider = httpContext.RequestServices.GetService<IDataProvider>();
            LogFilter logFilter = new LogFilter();
            var logs = provider.FetchLogs(logFilter);
            StringBuilder htmlTableBody = new StringBuilder();
            foreach(var log in logs)
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
    }
}
