using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using NoNameLoggerUI;
using NoNameLoggerUI.Interface;
using NoNameLoggerMsSqlServerDataProvider.Config;
using NoNameLoggerMsSqlServerDataProvider.Services;

namespace NoNameLoggerMsSqlServerDataProvider
{
    public static class MyLibraryUiBuilderOptions
    {
        public static void UseSqlServer(
            this NoNameLoggerUiOptionsBuilder optionsBuilder,
            string connectionString,
            string tableName,
            string schemaName = "dbo"
        )
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException(nameof(connectionString));

            if (string.IsNullOrEmpty(tableName))
                throw new ArgumentNullException(nameof(tableName));
            var config = new MsSqlServerConfig()
            {
                ConnectionString = connectionString,
                TableName = tableName,
                SchemaName = schemaName
            };
            ((INoNameLoggerUiOptionsBuilder)optionsBuilder).Services.AddSingleton(config);
            ((INoNameLoggerUiOptionsBuilder)optionsBuilder).Services.AddScoped<IDataProvider, MsSQLServerDataProvider>();
        }
    }
}
