using NoNameLogger.Configs;
using NoNameLogger.Interfaces;
using NoNameLogger.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace NoNameLogger.Providers
{
    class LogToConsoleProvider:ISinksProvider
    {
        private readonly ConsoleConfig _config;

        public LogToConsoleProvider(ConsoleConfig config)
        {
            _config = config;
        }

        public ILog CreateLog()
        {
            return new LogToConsole(_config);
        }
    }
}
