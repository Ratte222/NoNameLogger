using NoNameLogger.Configs.Notification;
using NoNameLogger.Interfaces;
using NoNameLogger.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace NoNameLogger.Providers
{
    public class EmailNotificationProvider:ISinksProvider
    {
        private readonly EmailNotificationConfig _currentConfig;

        public EmailNotificationProvider(EmailNotificationConfig config)
        {
            _currentConfig = config;
        }

        public ILog CreateLog()
        {
            return new EmailNotification(_currentConfig);
        }
    }
}
