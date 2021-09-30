using NoNameLogger.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace NoNameLogger.Configs.Notification
{
    public class EmailNotificationConfig:BaseConfig
    {
        public string DisplayName { get; set; }
        public string Subject { get; set; }
        public string EmailFrom { get; set; }
        public string EmailTo { get; set; }
        public string Password { get; set; }
        public string SmtpHost { get; set; }
        public int SmtpPort { get; set; }
    }
}
