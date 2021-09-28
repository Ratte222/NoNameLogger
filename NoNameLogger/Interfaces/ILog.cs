using NoNameLogger.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace NoNameLogger.Interfaces
{
    public interface ILog:IDisposable
    {
        public void Log(LogEvent logEvent);
        //public void FlushAndClose();
    }
}
