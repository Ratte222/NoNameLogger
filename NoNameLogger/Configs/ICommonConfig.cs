using System;
using System.Collections.Generic;
using System.Text;

namespace NoNameLogger.Configs
{
    public interface ICommonConfig
    {
        LogLevel MinLogLevel { get; set; }
        LogLevel MaxLogLevel { get; set; }
    }
}
