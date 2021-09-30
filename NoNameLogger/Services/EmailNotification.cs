using NoNameLogger.Configs.Notification;
using NoNameLogger.Events;
using NoNameLogger.Extensions;
using NoNameLogger.Interfaces;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace NoNameLogger.Services
{
    class EmailNotification : ILog
    {
        private readonly EmailNotificationConfig _config;

        public EmailNotification(EmailNotificationConfig config)
        {
            _config = config;
        }

        public void Dispose()
        {
            
        }

        public void Log(LogEvent logEvent)
        {
            try
            {
                if (logEvent.LogLevel.CheckLogLeavel(_config))
                {
                    SendEmail(_config.EmailTo, _config.Subject, 
                        _config.Formatter.Serialize(logEvent.ToLog()));
                }
            }
            catch (Exception ex)
            {

            }
        }

        public void SendEmail(string email, string subject, string message, bool isHtml = false)
        {
            var from = new MailAddress(_config.EmailFrom, _config.DisplayName);
            var to = new MailAddress(email);
            var msg = new MailMessage(from, to)
            {
                Subject = subject,
                Body = message,
                IsBodyHtml = isHtml
            };
            using (SmtpClient smtp = new SmtpClient(_config.SmtpHost, _config.SmtpPort))
            {
                smtp.Credentials = new NetworkCredential(_config.EmailFrom, _config.Password);
                smtp.EnableSsl = true;
                smtp.Send(msg);
            }
        }
    }
}
