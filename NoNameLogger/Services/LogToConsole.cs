using NoNameLogger.Configs;
using NoNameLogger.Events;
using NoNameLogger.Extensions;
using NoNameLogger.Interfaces;
using NoNameLogger.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NoNameLogger.Services
{
    internal class LogToConsole:ILog
    {
        private readonly ConsoleConfig _consoleConfig;

        public LogToConsole(ConsoleConfig consoleConfig)
        {
            _consoleConfig = consoleConfig;
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        public void FlushAndClose()
        {
            //throw new NotImplementedException();
        }

        public void Log(LogEvent logEvent)
        {
            try
            {
                if (logEvent.LogLevel.CheckLogLeavel(_consoleConfig))
                { _consoleConfig.Formatter.Serialize(Console.Out, logEvent.ToLog()); }
            }
            catch(Exception ex)
            { }
            //_consoleConfig.TextWriter.WriteLine(logEvent.ToString());
        }

    }
}
