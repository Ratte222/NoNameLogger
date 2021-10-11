using NoNameLogger.Configs;
using NoNameLogger.Events;
using NoNameLogger.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace NoNameLogger.Services
{
    public class LoggerConfiguration:ILoggerConfiguration
    {
        public LoggerSinkConfiguration WriteTo { get; set; } 



        public LoggerConfiguration()
        {
            WriteTo = new LoggerSinkConfiguration(this);
        }

        public ILogger CreateLoggger()
        {
            //List<ILog> actions = new List<ILog>();
            //foreach(var consoleCong in WriteTo.ConsoleConfigs)
            //{
            //    actions.Add(new LogInConsole.LogInConsole(consoleCong));
            //}
            //foreach(var fileConf in WriteTo.FileConfigs)
            //{
            //    actions.Add(new LogInFile.LogInFile(fileConf));
            //}
            List<ILog> logs = new List<ILog>();
            foreach(var provider in WriteTo.SinksProviders)
            {
                logs.Add(provider.CreateLog());
            }
            return new Logger(logs);
        }
    }
}
