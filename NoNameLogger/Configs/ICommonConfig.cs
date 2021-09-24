using System;
using System.Collections.Generic;
using System.Text;

namespace NoNameLogger.Configs
{
    interface ICommonConfig
    {
        LogLevel MinLogLevel { get; set; }
        LogLevel MaxLogLevel { get; set; }
    }
}
