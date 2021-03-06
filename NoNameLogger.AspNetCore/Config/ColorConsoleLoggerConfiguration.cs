using System;
using System.Collections.Generic;
using System.Text;
using logging = Microsoft.Extensions.Logging;

namespace NoNameLogger.AspNetCore.Config
{
    public class ColorConsoleLoggerConfiguration
    {
        public int EventId { get; set; }

        public Dictionary<logging.LogLevel, ConsoleColor> LogLevels { get; set; } = new Dictionary<logging.LogLevel, ConsoleColor>()
        {
            [logging.LogLevel.Information] = ConsoleColor.Green
        };
    }
}
