using NoNameLogger.Interfaces;
using System;
using System.Collections.Generic;

using System.Text;

namespace NoNameLogger.Configs
{
    public interface IFileConfig: ICommonConfig
    {
        Encoding Encoding { get; set; }
        TimeSpan? FlushToDiskInterval { get; set; }
        long? FileSizeLimitBytes { get; set; }
        string Path { get; set; }
        RollingInterval RollingInterval { get; set; } 
        bool RollOnFileSizeLimit { get; set; }
        IFormatter Formatter { get; set; }
    }
}
