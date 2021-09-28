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
    class LogInMsSQL : ILog
    {
        private readonly MsSQLConfig _config;

        public LogInMsSQL(MsSQLConfig config)
        {
            _config = config;
        }

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
            using (SqlCommand command = new SqlCommand())
            {
                using(SqlConnection sqlConnection = new SqlConnection(_config.ConnectionString))
                {
                    command.CommandText = $"INSERT INTO [{_config.TableName}] ([{nameof(log.Level)}], [{nameof(log.TimeStamp)}], " +
                        $"[{nameof(log.Message)}], [{nameof(log.MessageTemplate)}], [{nameof(log.Exception)}], " +
                        $"[{nameof(log.Properties)}]) VALUES(@{nameof(log.Level)}, @{nameof(log.TimeStamp)}, " +
                        $"@{nameof(log.Message)}, @{nameof(log.MessageTemplate)}, @{nameof(log.Exception)}, " +
                        $"@{nameof(log.Properties)})";
                    sqlConnection.Open();
                    command.Connection = sqlConnection;
                    command.Parameters.AddWithValue(nameof(log.Level), log.Level);
                    command.Parameters.AddWithValue(nameof(log.TimeStamp), log.TimeStamp);
                    command.Parameters.AddWithValue(nameof(log.Message), log.Message);
                    command.Parameters.AddWithValue(nameof(log.MessageTemplate), log.MessageTemplate);
                    command.Parameters.AddWithValue(nameof(log.Exception), log.Exception);
                    command.Parameters.AddWithValue(nameof(log.Properties), log.Properties);
                    command.ExecuteNonQuery();
                    sqlConnection.Close();
                }
            }
        }
    }
}
