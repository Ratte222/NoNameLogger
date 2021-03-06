using NoNameLogger.Interfaces;
using NoNameLogger.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace NoNameLogger.Configs
{
    public enum WriteTo
    {
        Console,
        File
    }
    public class LoggerSinkConfiguration
    {
        readonly LoggerConfiguration _loggerConfiguration;

        public LoggerSinkConfiguration(LoggerConfiguration loggerConfiguration)
        {
            _loggerConfiguration = loggerConfiguration;
        }

        #region fileConfiguration
        //internal List<FileConfig> FileConfigs { get; } = new List<FileConfig>();
        //internal IFormatProvider fileFormatProvider;
        #endregion
        #region consoleConfiguration
        //internal List<ConsoleConfig> ConsoleConfigs { get; } = new List<ConsoleConfig>();
        #endregion
        protected List<WriteTo> WritesTo { get; set; } = new List<WriteTo>();

        //internal List<ILog> actions { get; private set; } = new List<ILog>();

        internal List<ISinksProvider> SinksProviders { get; set; } = new List<ISinksProvider>();

        public LoggerConfiguration AddSinksProviders(ISinksProvider sinksProvider)
        {
            SinksProviders.Add(sinksProvider);
            return _loggerConfiguration;
        }

        //public LoggerConfiguration AddAction(ILog log)
        //{
        //    actions.Add(log);
        //    return _loggerConfiguration;
        //}
        //internal LoggerConfiguration AddFileConfig(FileConfig fileConfig)
        //{
        //    WritesTo.Add(WriteTo.File);
        //    FileConfigs.Add(fileConfig);
        //    return _loggerConfiguration;
        //}

        //internal LoggerConfiguration AddConsoleConfig(ConsoleConfig consoleConfig)
        //{
        //    //WritesTo.Add(WriteTo.Console);
        //    ConsoleConfigs.Add(consoleConfig);
        //    return _loggerConfiguration;
        //}

    }
}
