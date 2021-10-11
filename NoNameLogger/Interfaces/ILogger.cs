using NoNameLogger.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace NoNameLogger.Interfaces
{
    public interface ILogger:IDisposable
    {
        public void LogDebug(string message, params object[] args);
        public void LogInformation(string message, params object[] args);
        public void LogWarning(string message, params object[] args);
        public void LogError(string message, params object[] args);
        public void LogCritical(string message, params object[] args);
        public void Log(LogLevel logLevel, string message, Exception exception = null, params object[] args);
        public void Log(LogLevel logLevel, string message, params string[] args);
        //public void FlushAndClosed();
    }
}
