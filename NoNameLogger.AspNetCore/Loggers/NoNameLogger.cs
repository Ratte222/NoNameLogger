using System;
using System.Collections.Generic;
using logging = Microsoft.Extensions.Logging;
using System.Text;
using NoNameLogger.AspNetCore.Config;

namespace NoNameLogger.AspNetCore.Loggers
{
    public class NoNameLogger: logging.ILogger
    {
        private readonly string _name;
        private readonly Func<NoNameLoggerConfig> _getCurrentConfig;

        public NoNameLogger(
            string name,
            Func<NoNameLoggerConfig> getCurrentConfig) =>
            (_name, _getCurrentConfig) = (name, getCurrentConfig);

        public IDisposable BeginScope<TState>(TState state) => default;

        public bool IsEnabled(logging.LogLevel logLevel) =>
            _getCurrentConfig().LogLevels.Contains(logLevel);

        public void Log<TState>(
            logging.LogLevel logLevel,
            logging.EventId eventId,
            TState state,
            Exception exception,
            Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            NoNameLoggerConfig config = _getCurrentConfig();
            if (config.EventId == 0 || config.EventId == eventId.Id)
            {
                config.Logger.Log(LogLevelExtension.ToLogLeavel((int)logLevel), formatter(state, exception), _name,
                    eventId.Id.ToString(), eventId.Name);
            }
        }
    }
}
