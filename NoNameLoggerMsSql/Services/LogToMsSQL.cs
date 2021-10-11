using NoNameLogger.Events;
using NoNameLogger.Interfaces;
using NoNameLogger.Extensions;
using NoNameLoggerMsSql.Configs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using NoNameLogger.Model;

namespace NoNameLoggerMsSql.Services
{
    class LogToMsSQL : ILog
    {
        private readonly MsSQLConfig _config;

        public LogToMsSQL(MsSQLConfig config)
        {
            _config = config;
            if(_config.AutoCreateTable)
            {
                CreateTable();
            }
        }

        private void CreateTable()
        {
            try
            {
                using (var sqlConnection = GetSqlConnection())
                {
                    using (var command = sqlConnection.CreateCommand())
                    {
                        command.CommandText = GetCreateTableCommand();
                        sqlConnection.Open();
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch
            {

            }            
        }

        private string GetCreateTableCommand()
        {
            var sql = new StringBuilder();
            var ix = new StringBuilder();
            var logForNameof = new Log();

            // start schema check and DDL (wrap in EXEC to make a separate batch)
            sql.AppendLine($"IF(NOT EXISTS(SELECT * FROM sys.schemas WHERE name = '{_config.SchemaName}'))");
            sql.AppendLine("BEGIN");
            sql.AppendLine($"EXEC('CREATE SCHEMA [{_config.SchemaName}] AUTHORIZATION [dbo]')");
            sql.AppendLine("END");

            // start table-creatin batch and DDL
            sql.AppendLine($"IF NOT EXISTS (SELECT s.name, t.name FROM sys.tables t JOIN sys.schemas s ON t.schema_id = s.schema_id WHERE s.name = '{_config.SchemaName}' AND t.name = '{_config.TableName}')");
            sql.AppendLine("BEGIN");
            sql.AppendLine($"CREATE TABLE [{_config.SchemaName}].[{_config.TableName}] ( ");


            sql.AppendLine($"[{nameof(logForNameof.Id)}] BIGINT IDENTITY(1,1) NOT NULL, ");
            sql.AppendLine($"[{nameof(logForNameof.Message)}] nvarchar(max) NULL, ");
            sql.AppendLine($"[{nameof(logForNameof.MessageTemplate)}] nvarchar(max) NULL, ");
            sql.AppendLine($"[{nameof(logForNameof.Level)}] nvarchar(100) NULL, ");
            sql.AppendLine($"[{nameof(logForNameof.Timestamp)}] datetime2 NOT NULL, ");
            sql.AppendLine($"[{nameof(logForNameof.Exception)}] nvarchar(MAX) NULL, ");
            sql.AppendLine($"[{nameof(logForNameof.Properties)}] nvarchar(MAX) NULL ");
            // primary key 
            sql.AppendLine($"CONSTRAINT [PK_{_config.TableName}] PRIMARY KEY CLUSTERED " +
                $"([{nameof(logForNameof.Id)}] ASC)");

            // end of CREATE TABLE
            sql.AppendLine(");");

            
            // end of batch
            sql.AppendLine("END");

            return sql.ToString();
        }

        private SqlConnection GetSqlConnection() =>
            new SqlConnection(_config.ConnectionString);

        public void Dispose()
        {
            
        }

        public void Log(LogEvent logEvent)
        {
            try
            { Create(logEvent); }
            catch(Exception ex)
            { }
        }

        private void Create(LogEvent logEvent)
        {
            Log log = logEvent.ToLog();
            using (var sqlConnection = GetSqlConnection())
            {
                using (SqlCommand command = sqlConnection.CreateCommand())
                {
                    command.CommandText = $"INSERT INTO [{_config.TableName}] ([{nameof(log.Level)}], [{nameof(log.Timestamp)}], " +
                            $"[{nameof(log.Message)}], [{nameof(log.MessageTemplate)}], [{nameof(log.Exception)}], " +
                            $"[{nameof(log.Properties)}]) VALUES(@{nameof(log.Level)}, @{nameof(log.Timestamp)}, " +
                            $"@{nameof(log.Message)}, @{nameof(log.MessageTemplate)}, @{nameof(log.Exception)}, " +
                            $"@{nameof(log.Properties)})";
                    command.Parameters.AddWithValue(nameof(log.Level), log.Level);
                    command.Parameters.AddWithValue(nameof(log.Timestamp), log.Timestamp);
                    command.Parameters.AddWithValue(nameof(log.Message), log.Message);
                    command.Parameters.AddWithValue(nameof(log.MessageTemplate), log.MessageTemplate);
                    command.Parameters.AddWithValue(nameof(log.Exception), log.Exception);
                    command.Parameters.AddWithValue(nameof(log.Properties), log.Properties);
                    sqlConnection.Open();
                    command.ExecuteNonQuery();
                    //sqlConnection.Close();
                }                    
            }           
        }
    }
}
