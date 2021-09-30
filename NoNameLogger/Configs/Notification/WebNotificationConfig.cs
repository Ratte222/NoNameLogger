using NoNameLogger.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace NoNameLogger.Configs.Notification
{
    public class WebNotificationConfig:BaseConfig
    {
        //public NotificationType NotificationType { get; set; }
        public string Url { get; set; }

        /// <example>GET, POST</example>
        public string TypeRequest { get; set; }
    }
}
