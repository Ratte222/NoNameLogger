using System;
using System.Collections.Generic;
using System.Linq;
using NoNameLogger.Model;
using NoNameLoggerUI.Filters;
using NoNameLoggerUI.Interface;
using NoNameLoggerMsSqlServerDataProvider.Config;
using NoNameLogger.Extensions;
using System.Data.SqlClient;
using System.Data;
using Dapper;
using NoNameLoggerUI.Helpers;
using System.Text;

namespace NoNameLoggerMsSqlServerDataProvider.Services
{
    class MsSQLServerDataProvider : IDataProvider
    {
        private readonly MsSqlServerConfig _config;

        public MsSQLServerDataProvider(MsSqlServerConfig config)
        {
            _config = config;
        }

        public IEnumerable<Log> FetchLogs(LogFilter logFilter, PageResponse<Log> pageResponse)
        {
            bool firstWhere = true;
            CheckFilter(logFilter);
            StringBuilder query = new StringBuilder();
            query.Append($"SELECT * FROM {_config.TableName} ");
            if (logFilter.StartDate.HasValue || logFilter.EndDate.HasValue || 
                !String.IsNullOrEmpty(logFilter.SearchString) || !String.IsNullOrEmpty(logFilter.LevelString))
            {
                query.Append("WHERE ");
            }
            if(!String.IsNullOrEmpty(logFilter.SearchString))
            {
                if(!firstWhere)
                { query.Append("AND "); }
                query.Append($"[{nameof(Log.Message)}] = '{logFilter.SearchString}' ");
                firstWhere = false;
            }
            if (!String.IsNullOrEmpty(logFilter.LevelString))
            {
                if (!firstWhere)
                { query.Append("AND "); }
                query.Append($"[{nameof(Log.Level)}] = '{logFilter.LevelString}' ");
                firstWhere = false;
            }
            query.Append($" ORDER BY {logFilter.OrderByField} {logFilter.OrderBy} " +
            $"OFFSET {pageResponse.Skip} ROWS FETCH NEXT {pageResponse.Take} ROWS ONLY");
            using (IDbConnection db = new SqlConnection(_config.ConnectionString))
            {
                return db.Query<Log>(query.ToString());
            }
        }

        public long CountLogs(LogFilter logFilter)
        {
            CheckFilter(logFilter);
            long count = 0;
            using(SqlCommand command = new SqlCommand())
            {
                using (SqlConnection sqlConnection = new SqlConnection(_config.ConnectionString))
                {
                    command.CommandText = $"SELECT Count(*) FROM {_config.TableName}";
                    sqlConnection.Open();
                    command.Connection = sqlConnection;
                    string response = "";
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            response = reader[0].ToString();
                        }
                    }
                    long.TryParse(response, out count);
                }
            }
            return count;
        }

        private void CheckFilter(LogFilter logFilter)
        {
            if (String.IsNullOrEmpty(logFilter.OrderBy))
            {
                logFilter.OrderBy = "desc";
            }
            else if (!String.Equals(logFilter.OrderBy, "asc") && !String.Equals(logFilter.OrderBy, "desc"))
            {
                logFilter.OrderBy = "desc";
            }
            if (!(typeof(Log).GetAllPublicGetProperty<string>().Any(i => i == logFilter.OrderByField)))
            {
                Log log = new Log();
                logFilter.OrderByField = nameof(log.TimeStamp);
            }
        }
    }
}
