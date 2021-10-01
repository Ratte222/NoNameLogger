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

        public EmailNotificationConfig CheckConfiguration()
        {
            if (String.IsNullOrEmpty(EmailFrom)) throw new ArgumentNullException($"{nameof(EmailFrom)} is null or empty");
            if (String.IsNullOrEmpty(EmailTo)) throw new ArgumentNullException($"{nameof(EmailTo)} is null or empty");
            if (String.IsNullOrEmpty(Password)) throw new ArgumentNullException($"{nameof(Password)} is null or empty");
            if (String.IsNullOrEmpty(SmtpHost)) throw new ArgumentNullException($"{nameof(SmtpHost)} is null or empty");
            if (SmtpPort == 0) throw new ArgumentException($"{nameof(SmtpPort)} is zero!");
            if (String.IsNullOrEmpty(DisplayName))
            {
                DisplayName = "NoNameLogger";
            }
            if (String.IsNullOrEmpty(Subject))
            {
                DisplayName = "Log";
            }
            if (Formatter is null)
            {
                throw new ArgumentNullException($"{nameof(Formatter)} is null");
            }
            return this;
        }
    }
}
