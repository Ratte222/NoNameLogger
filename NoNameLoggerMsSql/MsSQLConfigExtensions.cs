using NoNameLogger.Configs;
using NoNameLogger.Formatting;
using NoNameLogger.Interfaces;
using NoNameLogger.Services;
using NoNameLoggerMsSql.Configs;
using NoNameLoggerMsSql.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace NoNameLoggerMsSql
{
    public static class MsSQLConfigExtensions
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sinkConfiguration"></param>
        /// <param name="connectionString"></param>
        /// <param name="tableName"></param>
        /// <param name="schemaName"></param>
        /// <param name="formatter"></param>
        /// <param name="createTable">Do not work!!!</param>
        /// <returns></returns>
        public static LoggerConfiguration MsSQLServer(this LoggerSinkConfiguration sinkConfiguration, string connectionString,
            string tableName, string schemaName = "dbo", IFormatter formatter = null, bool createTable = false)
        {
            MsSQLConfig config = new MsSQLConfig();
            if (String.IsNullOrEmpty(connectionString)) throw new ArgumentNullException($"{nameof(connectionString)} is null or empty");
            if (String.IsNullOrEmpty(tableName)) throw new ArgumentNullException($"{nameof(tableName)} is null or empty");
            if (String.IsNullOrEmpty(schemaName)) throw new ArgumentNullException($"{nameof(schemaName)} is null or empty");
            config.ConnectionString = connectionString;
            config.TableName = tableName;
            config.SchemaName = schemaName;
            config.CreateTable = createTable;
            if(formatter is null)
            {
                config.Formatter = new StringFormatter();
            }
            else
            {
                config.Formatter = formatter;
            }
            return sinkConfiguration.AddAction(new LogInMsSQL(config));
        }
    }
}
