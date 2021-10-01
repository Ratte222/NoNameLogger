using NoNameLogger.Configs;
using NoNameLogger.Configs.Notification;
using NoNameLogger.Interfaces;
using NoNameLogger.Providers;
using NoNameLogger.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace NoNameLogger.LoggerConfigExtensions
{
    public static class EmailConfigExtension
    {
        
        public static LoggerConfiguration Email(this LoggerSinkConfiguration sinkConfiguration, 
            Func<EmailNotificationConfig, EmailNotificationConfig> func)
        {
            var config = func(new EmailNotificationConfig());
            return sinkConfiguration.AddSinksProviders(new EmailNotificationProvider(config.CheckConfiguration()));
        }
    }
}
