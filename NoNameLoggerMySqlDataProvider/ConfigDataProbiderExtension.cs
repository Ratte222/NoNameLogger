using Microsoft.Extensions.DependencyInjection;
using NoNameLoggerMySqlDataProvider.Config;
using NoNameLoggerMySqlDataProvider.Services;
using NoNameLoggerUI;
using NoNameLoggerUI.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace NoNameLoggerMySqlDataProvider
{
    public static class ConfigDataProbiderExtension
    {
        public static void UseMySql(
            this NoNameLoggerUiOptionsBuilder optionsBuilder,
            string connectionString,
            string tableName)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException(nameof(connectionString));

            if (string.IsNullOrEmpty(tableName))
                throw new ArgumentNullException(nameof(tableName));
            var config = new MySqlDataProviderConfig()
            {
                ConnectionString = connectionString,
                TableName = tableName
            };
            ((INoNameLoggerUiOptionsBuilder)optionsBuilder).Services.AddSingleton(config);
            ((INoNameLoggerUiOptionsBuilder)optionsBuilder).Services.AddScoped<IDataProvider, MySqlDataProvider>();
        }
    }
}
