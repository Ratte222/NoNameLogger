using NoNameLogger.Enums;
using NoNameLogger.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace NoNameLogger.Configs
{
    public class BaseConfig
    {
        public LogLevel MinLogLevel { get; set; } = LogLevel.Trace;
        public LogLevel MaxLogLevel { get; set; } = LogLevel.None;
        public IFormatter Formatter { get; set; }
    }
}
