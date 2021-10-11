using NoNameLogger.Configs;
using NoNameLogger.Formatting;
using NoNameLogger.Interfaces;
using NoNameLogger.Services;
using NoNameLoggerMySQL.Config;
using NoNameLoggerMySQL.Providers;
using System;
using System.Collections.Generic;
using System.Text;

namespace NoNameLoggerMySQL
{
    public static class MySQLBuilderExtension
    {
        public static LoggerConfiguration MySQL(this LoggerSinkConfiguration sinkConfiguration, string connectionString,
            string tableName, /*IFormatter formatter = null,*/ bool autoCreateTable = true)
        {
            MySQLConfig config = new MySQLConfig();
            if (String.IsNullOrEmpty(connectionString)) throw new ArgumentNullException($"{nameof(connectionString)} is null or empty");
            if (String.IsNullOrEmpty(tableName)) throw new ArgumentNullException($"{nameof(tableName)} is null or empty");
            config.ConnectionString = connectionString;
            config.TableName = tableName;
            config.AutoCreateTable = autoCreateTable;
            //if (formatter is null)
            //{
            //    config.Formatter = new StringFormatter();
            //}
            //else
            //{
            //    config.Formatter = formatter;
            //}
            //return sinkConfiguration.AddAction(new LogToMsSQL(config));
            return sinkConfiguration.AddSinksProviders(new LogToMySQLProvider(config));
        }
    }
}
