using System;
using System.Collections.Generic;
using logging = Microsoft.Extensions.Logging;
using NoNameLogger.AspNetCore.Config;
using NoNameLogger.Services;
using NoNameLogger.Configs;

namespace NoNameLogger.AspNetCore.Loggers
{
    public class NoNameFileLogger : Logger, logging.ILogger
    {
        private readonly string _name;
        private readonly Func<FileConfiguration> _getCurrentConfig;
        //private readonly LogInFile _logInFile;

        public NoNameFileLogger(
            string name,
            Func<FileConfiguration> getCurrentConfig, List<Interfaces.ILog> log) : base(log)
        {
            (_name, _getCurrentConfig) = (name, getCurrentConfig);
            //_logInFile = new LogInFile(_getCurrentConfig());
        }

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

            FileConfiguration config = _getCurrentConfig();
            if (config.EventId == 0 || config.EventId == eventId.Id)
            {
                //ConsoleColor originalColor = Console.ForegroundColor;

                //Console.ForegroundColor = config.LogLevels[logLevel];
                //Console.WriteLine($"[{eventId.Id,2}: {logLevel,-12}]");

                //Console.ForegroundColor = originalColor;
                //Console.WriteLine($"     {_name} - {formatter(state, exception)}");
                //_logInFile.Log(LogLevelExtension.ToLogLeavel((int)logLevel), )
                this.Log(LogLevelExtension.ToLogLeavel((int)logLevel), formatter(state, exception), _name,
                    eventId.Id.ToString(), eventId.Name);
            }
        }
    }
}
