using NoNameLogger.Enums;
using NoNameLogger.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace NoNameLogger.Configs
{
    public class HttpConfig:BaseConfig
    {
        public Encoding Encoding { get; set; }
        public string Url { get; set; }

        public string BeforeLogData { get; set; }
        public string AfterLogData { get; set; }

        /// <example>GET, POST</example>
        public string MethodRequest { get; set; }
        public Dictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();
        public string ContentType { get; set; }


        public HttpConfig CheckConfiguration()
        {
            if (String.IsNullOrEmpty(Url)) throw new ArgumentNullException($"{nameof(Url)} is null or empty");
            if (String.IsNullOrEmpty(MethodRequest))
            {
                MethodRequest = "GET";
            }
            if (Encoding is null)
            {
                Encoding = Encoding.UTF8;
            }
            if (Formatter is null)
            {
                throw new ArgumentNullException($"{nameof(Formatter)} is null");
            }
            return this;
        }
    }
}
