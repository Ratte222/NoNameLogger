using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace NoNameLogger.AspNetCore.Config
{
    public class ColorConsoleLoggerConfiguration
    {
        public int EventId { get; set; }

        public Dictionary<LogLevel, ConsoleColor> LogLevels { get; set; } = new Dictionary<LogLevel, ConsoleColor>()
        {
            [LogLevel.Information] = ConsoleColor.Green
        };
    }
}
