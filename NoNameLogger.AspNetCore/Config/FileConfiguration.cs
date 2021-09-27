using System;
using System.Collections.Generic;
using System.Text;
using NoNameLogger.Configs;
using logging = Microsoft.Extensions.Logging;

namespace NoNameLogger.AspNetCore.Config
{
    public class FileConfiguration : FileConfig
    {
        public int EventId { get; set; }

        public List<logging.LogLevel> LogLevels { get; set; } = new List<logging.LogLevel>(
            new[] {logging.LogLevel.Trace, logging.LogLevel.Debug, logging.LogLevel.Information,
            logging.LogLevel.Warning, logging.LogLevel.Error, logging.LogLevel.Critical, logging.LogLevel.None});
        
    }
}
