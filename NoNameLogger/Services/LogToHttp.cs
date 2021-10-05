using NoNameLogger.Configs;
using NoNameLogger.Events;
using NoNameLogger.Extensions;
using NoNameLogger.Interfaces;
using System;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using System.Text;
using System.IO;

namespace NoNameLogger.Services
{
    class LogToHttp : ILog
    {
        private readonly HttpConfig _config;

        public LogToHttp(HttpConfig config)
        {
            _config = config;
        }

        public void Log(LogEvent logEvent)
        {
            try
            {
                //https://docs.microsoft.com/ru-ru/dotnet/framework/network-programming/how-to-send-data-using-the-webrequest-class
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(_config.Url);
                request.Credentials = CredentialCache.DefaultCredentials;
                foreach (var header in _config.Headers)
                {
                    request.Headers.Add(header.Key, header.Value);
                }
                request.Method = _config.MethodRequest.ToUpper();
                string data = GenerateData(logEvent);
                var requestData = _config.Encoding.GetBytes(data);
                if(requestData.Length > 0)
                {
                    request.ContentType = _config.ContentType;
                    request.ContentLength = requestData.Length;
                    Stream stream = request.GetRequestStream();
                    stream.Write(requestData, 0, requestData.Length);
                    stream.Close();
                }
                 
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                response.Close();
            }
            catch(Exception ex)
            { }
        }

        private string GenerateData(LogEvent logEvent)
        {
            return $"{_config.BeforeLogData}" +
                $"{_config.Formatter.Serialize(logEvent.ToLog())}" +
                $"{_config.AfterLogData}";
        }

        public void Dispose()
        {

        }
    }
}
