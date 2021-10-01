using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NoNameLogger.Model
{
    [Serializable]
    public class Log
    {
        public long Id { get; set; }
        public string Message { get; set; }
        public string MessageTemplate { get; set; }
        [StringLength(128)]
        public string Level { get; set; }
        [JsonConverter(typeof(Newtonsoft.Json.Converters.IsoDateTimeConverter))]
        public DateTime Timestamp { get; set; }
        public string Exception { get; set; }
        public string Properties { get; set; }

        public override string ToString()
        {
            //return base.ToString();
            return $"{Timestamp.ToString()}, {Level}, {Message}, " +
                $"{Exception}, {Properties}";
        }
    }
}
