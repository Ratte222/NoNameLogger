using NoNameLogger.Configs;
using NoNameLogger.Providers;
using NoNameLogger.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace NoNameLogger.LoggerConfigExtensions
{
    public static class HttpConfigExtension
    {
        public static LoggerConfiguration Http(this LoggerSinkConfiguration sinkConfiguration,
            Func<HttpConfig, HttpConfig> func)
        {
            var config = func(new HttpConfig());
            return sinkConfiguration.AddSinksProviders(new LogToHttpProvider(config.CheckConfiguration()));
        }
    }
}
