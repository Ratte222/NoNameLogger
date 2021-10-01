using NoNameLogger.Interfaces;
using NoNameLoggerMySQL.Config;
using NoNameLoggerMySQL.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace NoNameLoggerMySQL.Providers
{
    class LogToMySQLProvider : ISinksProvider
    {
        private readonly MySQLConfig _config;

        public LogToMySQLProvider(MySQLConfig config)
        {
            _config = config;
        }

        public ILog CreateLog()
        {
            return new LogToMySQL(_config);
        }
    }
}
