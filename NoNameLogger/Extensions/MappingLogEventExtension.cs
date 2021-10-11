using NoNameLogger.Events;
using NoNameLogger.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace NoNameLogger.Extensions
{
    public static class MappingLogEventExtension
    {
        public static Log ToLog(this LogEvent logEvent)
        {
            return new Log()
            {
                Level = logEvent.LogLevel.ToString(),
                Timestamp = logEvent.TimeStamp,
                Message = String.IsNullOrEmpty(logEvent.Message) ? "" : logEvent.Message,
                Exception = String.IsNullOrEmpty(logEvent.Exception) ? "" : logEvent.Exception,
                Properties = String.IsNullOrEmpty(logEvent.Properties) ? "" : logEvent.Properties,
                MessageTemplate = ""
            };
        }
    }
}
