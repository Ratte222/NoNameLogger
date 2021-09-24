using System;
using System.Collections.Generic;
using System.Text;

namespace NoNameLogger.Interfaces
{
    public interface ILoggerConfiguration
    {
        public ILogger CreateLoggger();
    }
}
