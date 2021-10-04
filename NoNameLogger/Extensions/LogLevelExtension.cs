using System;
using System.Collections.Generic;
using System.Text;
using NoNameLogger.Enums;

namespace NoNameLogger.Extensions
{
    public static class LogLevelExtension
    {
        public static string ToStringFast(this LogLevel logLevel)
        {
            return logLevel switch
            {
                LogLevel.Trace => nameof(LogLevel.Trace),
                LogLevel.Debug => nameof(LogLevel.Debug),
                LogLevel.Information => nameof(LogLevel.Information),
                LogLevel.Warning => nameof(LogLevel.Warning),
                LogLevel.Error => nameof(LogLevel.Error),
                LogLevel.Critical => nameof(LogLevel.Critical),
                LogLevel.None => nameof(LogLevel.None),
                _ => throw new NotImplementedException(),
                //_ => "",
            };
        }
        public static LogLevel ToLogLeavel(int position)
        {
            return position switch
            {
                0 => LogLevel.Trace,
                1 => LogLevel.Debug,
                2 => LogLevel.Information,
                3 => LogLevel.Warning,
                4 => LogLevel.Error,
                5 => LogLevel.Critical,
                6 => LogLevel.None,
                _ => throw new ArgumentOutOfRangeException($"variable {nameof(position)}")
            };
        }
    }
}
