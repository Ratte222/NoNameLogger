using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using NoNameLogger.Enums;

namespace NoNameLogger.Events
{
    [Serializable]
    public class LogEvent
    {
        public LogEvent() { }
        public LogEvent(LogLevel level, Exception ex, string message, string properties)
        {
            (TimeStamp, LogLevel, Exception, Message, Properties) = (DateTime.Now, level,
                Exception is null ? null : /*ex.TargetSite.DeclaringType*/ $"InnerException = {ex?.InnerException?.ToString()} \r\n" +
                            $"Message = {ex?.Message?.ToString()} \r\n" +
                            $"Source = {ex?.Source?.ToString()} \r\n" +
                            $"StackTrace = {ex?.StackTrace?.ToString()} \r\n" +
                            $"TargetSite = {ex?.TargetSite?.ToString()}", message, properties);
        }
        //[JsonConverter(typeof(Newtonsoft.Json.Converters.IsoDateTimeConverter))]
        //public DateTimeOffset Timestamp { get; set; }
        public DateTime TimeStamp { get; set; }
        public LogLevel LogLevel { get; set; }
        //[XmlIgnore]
        //public Exception Exception { get; set; }
        public string Exception { get; set; }
        public string Message { get; set; }
        public string Properties { get; set; }

        public override string ToString()
        {
            //return base.ToString();
            return $"{TimeStamp}, {LogLevel}, {Message}, " +
                $"{Exception}, {Properties}";
        }
    }
}
