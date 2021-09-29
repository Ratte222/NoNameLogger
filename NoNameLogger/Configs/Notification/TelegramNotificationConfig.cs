using NoNameLogger.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace NoNameLogger.Configs.Notification
{
    public class TelegramNotificationConfig:ICommonNotificationConfig
    {
        public NotificationType NotificationType { get; set; }
        public string TelegramToken { get; set; }
        public long TelegramChat { get; set; }
    }
}
