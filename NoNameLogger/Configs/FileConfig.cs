using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using NoNameLogger.Formatting;
using NoNameLogger.Interfaces;

namespace NoNameLogger.Configs
{
    internal class FileConfig:ICommonConfig
    {
        public Encoding Encoding { get; set; }
        public TimeSpan? FlushToDiskInterval { get; set; }
        public long? FileSizeLimitBytes { get; set; }
        public string Path { get; set; }
        public RollingInterval RollingInterval { get; set; }
        public bool RollOnFileSizeLimit { get; set; }
        public LogLevel MinLogLevel { get; set; }
        public LogLevel MaxLogLevel { get; set; }
        //public TextWriter TextWriter { get; set; }
        public IFormatter Formatter{get;set;}
    }
}
