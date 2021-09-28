using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NoNameLogger.AspNetCore.Config;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using loging = Microsoft.Extensions.Logging;

namespace NoNameLogger.AspNetCore.Providers
{
    public sealed class NoNameLoggerProvider : ILoggerProvider
    {
        private readonly IDisposable _onChangeToken;
        private NoNameLoggerConfig _currentConfig;
        private readonly ConcurrentDictionary<string, Loggers.NoNameLogger> _loggers 
            = new ConcurrentDictionary<string, Loggers.NoNameLogger>();

        public NoNameLoggerProvider(
            IOptionsMonitor<NoNameLoggerConfig> config)
        {
            _currentConfig = config.CurrentValue;
            _onChangeToken = config.OnChange(updatedConfig => _currentConfig = updatedConfig);
        }

        public loging.ILogger CreateLogger(string categoryName) =>
            _loggers.GetOrAdd(categoryName, name => new Loggers.NoNameLogger(name, GetCurrentConfig));

        private NoNameLoggerConfig GetCurrentConfig() => _currentConfig;

        public void Dispose()
        {
            _currentConfig.Logger.Dispose();
            _loggers.Clear();
            _onChangeToken.Dispose();
        }
    }
}
