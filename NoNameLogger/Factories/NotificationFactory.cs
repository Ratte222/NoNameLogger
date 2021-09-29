using NoNameLogger.Configs.Notification;
using NoNameLogger.Interfaces.Notification;
using NoNameLogger.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace NoNameLogger.Factories
{
    public class NotificationFactory
    {
        private NotificationFactoryConfig _config;

        public NotificationFactory(NotificationFactoryConfig configs)
        {
            _config = configs;
        }

        public List<INotification> CreateNotifications()
        {
            List<INotification> notifications = new List<INotification>();
            foreach(var config in _config.configs)
            {
                ICommonNotificationConfig commonNotificationConfig = config as ICommonNotificationConfig;
                if(commonNotificationConfig != null)
                {
                    switch(commonNotificationConfig.NotificationType)
                    {
                        case NotificationType.Email:

                            break;
                        case NotificationType.Telegram:

                            break;
                        case NotificationType.Web:

                            break;
                    }
                }
            }

            return notifications;
        }
    }
}
