using NoNameLogger.Interfaces;
using NoNameLoggerMsSql.Configs;
using NoNameLoggerMsSql.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace NoNameLoggerMsSql.Providers
{
    class LogToMsSQLProvider : ISinksProvider
    {
        private readonly MsSQLConfig _config;

        public LogToMsSQLProvider(MsSQLConfig config)
        {
            _config = config;
        }

        public ILog CreateLog()
        {
            return new LogToMsSQL(_config);
        }
    }
}
