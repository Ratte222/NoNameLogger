using NoNameLogger.Configs;
using NoNameLogger.Interfaces;
using NoNameLogger.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace NoNameLogger.Providers
{
    class LogToFileProvider : ISinksProvider
    {
        private readonly FileConfig _config;

        public LogToFileProvider(FileConfig config)
        {
            _config = config;
        }

        public ILog CreateLog()
        {
            return new LogToFile(_config);
        }
    }
}
