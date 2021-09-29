using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using NoNameLogger.Enums;
using NoNameLogger.Interfaces;

namespace NoNameLogger.Configs
{
    public class FileConfig:IFileConfig
    {
        public Encoding Encoding { get; set; }
        public TimeSpan? FlushToDiskInterval { get; set; }
        public long? FileSizeLimitBytes { get; set; } = 1073741824;
        /// <summary>
        /// Required
        /// </summary>
        public string Path { get; set; }
        public RollingInterval RollingInterval { get; set; } = RollingInterval.Infinite;
        public bool RollOnFileSizeLimit { get; set; } = false;
        public LogLevel MinLogLevel { get; set; } = LogLevel.Trace;
        public LogLevel MaxLogLevel { get; set; } = LogLevel.None;
        //public TextWriter TextWriter { get; set; }

        /// <summary>
        /// Required
        /// </summary>
        public IFormatter Formatter{get;set;}


        public FileConfig CheckConfiguration()
        {
            if (String.IsNullOrEmpty(Path)) throw new ArgumentNullException("Path is null or empty");
            else if (Path.Contains('(') || Path.Contains(')')) throw new ArgumentException("Path contains \'(\' or \')\'");
            if(Encoding is null)
            {
                Encoding = Encoding.UTF8;
            }
            if (Formatter is null)
            { 
                throw new ArgumentNullException($"{nameof(Formatter)} is null"); 
            }
            return this;
        }
    }
}
