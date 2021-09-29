using NoNameLogger.Enums;
using NoNameLogger.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NoNameLogger.Configs
{
    public class ConsoleConfig:ICommonConfig
    {
        public LogLevel MinLogLevel { get; set; }
        public LogLevel MaxLogLevel { get; set; }
        public IFormatter Formatter { get; set; }
    }
}
