using NoNameLogger.Configs;
using NoNameLogger.Interfaces;
using NoNameLogger.Services;
using NoNameLogger.Formatting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using NoNameLogger.Providers;

namespace NoNameLogger.LoggerConfigExtensions
{
    public static class ConsoleConfigExtension
    {
        public static LoggerConfiguration Console(this LoggerSinkConfiguration sinkConfiguration,
            IFormatter formatter = null/*, IFormatProvider formatProvider*/)
        {
            //formatProvider.GetFormat(typeof(CultureInfo));
            //sinkConfiguration.consoleFormatProvider = CultureInfo.CurrentCulture;
            //sinkConfiguration.WritesTo.Add(WriteTo.Console);
            ConsoleConfig consoleConfig = new ConsoleConfig();
            if(formatter is null)
            {
                consoleConfig.Formatter = new StringFormatter();
            }
            else
            {
                consoleConfig.Formatter = formatter;
            }
            //return sinkConfiguration.AddAction(new LogInConsole(consoleConfig));
            return sinkConfiguration.AddSinksProviders(new LogToConsoleProvider(consoleConfig));
        }
    }
}
