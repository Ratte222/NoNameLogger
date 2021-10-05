using NoNameLogger.Configs;
using NoNameLogger.Interfaces;
using NoNameLogger.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace NoNameLogger.Providers
{
    class LogToHttpProvider : ISinksProvider
    {
        private readonly HttpConfig _config;

        public LogToHttpProvider(HttpConfig config)
        {
            _config = config;
        }

        public ILog CreateLog()
        {
            return new LogToHttp(_config);
        }
    }
}
