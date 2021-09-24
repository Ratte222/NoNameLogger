using NoNameLogger.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace NoNameLogger.Interfaces
{
    public interface ILog
    {
        public void Log(LogEvent logEvent);
    }
}
