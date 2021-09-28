using System;
using System.Collections.Generic;
using System.Text;
using logging = Microsoft.Extensions.Logging;

namespace NoNameLogger.AspNetCore.Config
{
    public class NoNameLoggerConfig
    {
        public Interfaces.ILogger Logger { get; set; }

        public int EventId { get; set; }

        public List<logging.LogLevel> LogLevels { get; set; } = new List<logging.LogLevel>(
            new[] {logging.LogLevel.Trace, logging.LogLevel.Debug, logging.LogLevel.Information,
            logging.LogLevel.Warning, logging.LogLevel.Error, logging.LogLevel.Critical, logging.LogLevel.None});
    }
}
