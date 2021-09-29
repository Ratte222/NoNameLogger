using NoNameLogger.Configs;
using NoNameLogger.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace NoNameLogger.Extensions
{
    static class CommonLogExtension
    {
        public static bool CheckLogLeavel(this LogLevel logLevel, ICommonConfig config)
        {
            if ((logLevel <= config.MaxLogLevel) && (logLevel >= config.MinLogLevel))
                return true;
            else
                return false;
        }
    }
}
