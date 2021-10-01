using MySql.Data.MySqlClient;
using NoNameLogger.Events;
using NoNameLogger.Extensions;
using NoNameLogger.Interfaces;
using NoNameLogger.Model;
using NoNameLoggerMySQL.Config;
using System;
using System.Collections.Generic;
using System.Text;

namespace NoNameLoggerMySQL.Services
{
    class LogToMySQL : ILog
    {
        private readonly MySQLConfig _config;

        public LogToMySQL(MySQLConfig config)
        {
            _config = config;
            if(config.CreateTable)
            {
                CreateTable(GetSqlConnection());
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Log(LogEvent logEvent)
        {
            WriteLogEvent(logEvent);
        }

        private MySqlConnection GetSqlConnection()
        {
            try
            {
                var conn = new MySqlConnection(_config.ConnectionString);
                conn.Open();

                return conn;
            }
            catch (Exception ex)
            {
                //SelfLog.WriteLine(ex.Message);

                return null;
            }
        }

        private void CreateTable(MySqlConnection sqlConnection)
        {
            try
            {
                var tableCommandBuilder = new StringBuilder();
                tableCommandBuilder.Append($"CREATE TABLE IF NOT EXISTS {_config.TableName} (");
                tableCommandBuilder.Append("Id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,");
                tableCommandBuilder.Append("Timestamp VARCHAR(100),");
                tableCommandBuilder.Append("Level VARCHAR(15),");
                tableCommandBuilder.Append("MessageTemplate TEXT,");
                tableCommandBuilder.Append("Message TEXT,");
                tableCommandBuilder.Append("Exception TEXT,");
                tableCommandBuilder.Append("Properties TEXT,");
                tableCommandBuilder.Append("_ts TIMESTAMP DEFAULT CURRENT_TIMESTAMP)");

                var cmd = sqlConnection.CreateCommand();
                cmd.CommandText = tableCommandBuilder.ToString();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //SelfLog.WriteLine(ex.Message);
            }
            finally
            {
                sqlConnection.Dispose();
            }
        }

        private MySqlCommand GetInsertCommand(MySqlConnection sqlConnection)
        {
            var tableCommandBuilder = new StringBuilder();
            tableCommandBuilder.Append($"INSERT INTO  {_config.TableName} (");
            tableCommandBuilder.Append("Timestamp, Level, MessageTemplate, Message, Exception, Properties) ");
            tableCommandBuilder.Append("VALUES (@ts, @level,@template, @msg, @ex, @prop)");

            var cmd = sqlConnection.CreateCommand();
            cmd.CommandText = tableCommandBuilder.ToString();

            cmd.Parameters.Add(new MySqlParameter("@ts", MySqlDbType.VarChar));
            cmd.Parameters.Add(new MySqlParameter("@level", MySqlDbType.VarChar));
            cmd.Parameters.Add(new MySqlParameter("@template", MySqlDbType.VarChar));
            cmd.Parameters.Add(new MySqlParameter("@msg", MySqlDbType.VarChar));
            cmd.Parameters.Add(new MySqlParameter("@ex", MySqlDbType.VarChar));
            cmd.Parameters.Add(new MySqlParameter("@prop", MySqlDbType.VarChar));

            return cmd;
        }

        protected bool WriteLogEvent(LogEvent logEvent)
        {
            try
            {
                Log log = logEvent.ToLog();
                using (var sqlCon = GetSqlConnection())
                {
                    var insertCommand = GetInsertCommand(sqlCon);
                    
                    
                    //var logMessageString = new StringWriter(new StringBuilder());
                    //logEvent.RenderMessage(logMessageString);

                    insertCommand.Parameters["@ts"].Value = log.Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fffzzz");

                    insertCommand.Parameters["@level"].Value = log.Level.ToString();
                    insertCommand.Parameters["@template"].Value = log.MessageTemplate.ToString();
                    insertCommand.Parameters["@msg"].Value = log.Message;
                    insertCommand.Parameters["@ex"].Value = log.Exception;
                    insertCommand.Parameters["@prop"].Value = log.Properties;

                    insertCommand.ExecuteNonQuery();
                    
                                                
                    return true;
                    
                }
            }
            catch (Exception ex)
            {
                //SelfLog.WriteLine(ex.Message);

                return false;
            }
        }
    }
}
