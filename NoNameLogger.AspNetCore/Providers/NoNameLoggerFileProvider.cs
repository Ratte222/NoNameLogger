using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NoNameLogger.AspNetCore.Config;
using NoNameLogger.AspNetCore.Loggers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace NoNameLogger.AspNetCore.Providers
{
    public sealed class NoNameLoggerFileProvider : ILoggerProvider
    {
        private readonly IDisposable _onChangeToken;
        private FileConfiguration _currentConfig;
        private readonly ConcurrentDictionary<string, NoNameFileLogger> _loggers = new ConcurrentDictionary<string, NoNameFileLogger>();

        public NoNameLoggerFileProvider(
            IOptionsMonitor<FileConfiguration> config)
        {
            _currentConfig = config.CurrentValue;
            _onChangeToken = config.OnChange(updatedConfig => _currentConfig = updatedConfig);
        }

        public ILogger CreateLogger(string categoryName) =>
            _loggers.GetOrAdd(categoryName, name => new NoNameFileLogger(name, GetCurrentConfig,
                new List<Interfaces.ILog>(new[] { new Services.LogInFile(GetCurrentConfig().CheckConfiguration()) })));

        private FileConfiguration GetCurrentConfig() => _currentConfig;
        

        

        public void Dispose()
        {
            _loggers.Clear();
            _onChangeToken.Dispose();
        }
    }
}
