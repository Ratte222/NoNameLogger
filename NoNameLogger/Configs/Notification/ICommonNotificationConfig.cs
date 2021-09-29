using NoNameLogger.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace NoNameLogger.Configs.Notification
{
    public interface ICommonNotificationConfig
    {
        public NotificationType NotificationType { get; set; }
    }
}
