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
            var queryBuilder = new StringBuilder();
            queryBuilder.Append($"SELECT [{nameof(Log.Id)}], [{nameof(Log.Message)}], [{nameof(Log.Level)}], " +
                $"[{nameof(Log.Timestamp)}], [{nameof(Log.Exception)}], [{nameof(Log.Properties)}] FROM [");
            queryBuilder.Append(_config.SchemaName);
            queryBuilder.Append("].[");
            queryBuilder.Append(_config.TableName);
            queryBuilder.Append("] ");
            CheckFilter(logFilter);
            GenerateWhereClause(queryBuilder, logFilter);
            queryBuilder.Append($" ORDER BY {logFilter.OrderByField} {logFilter.OrderBy} " +
            $"OFFSET {pageResponse.Skip} ROWS FETCH NEXT {pageResponse.Take} ROWS ONLY");
            using (IDbConnection db = new SqlConnection(_config.ConnectionString))
            {
                return db.Query<Log>(queryBuilder.ToString(), new
                {
                    StartDate = logFilter.StartDate,
                    EndDate = logFilter.EndDate
                });
            }
        }

        
        public long CountLogs(LogFilter logFilter)
        {
            CheckFilter(logFilter);
            var queryBuilder = new StringBuilder();
            queryBuilder.Append($"SELECT Count({nameof(Log.Id)}) FROM [");
            queryBuilder.Append(_config.SchemaName);
            queryBuilder.Append("].[");
            queryBuilder.Append(_config.TableName);
            queryBuilder.Append("] ");
            GenerateWhereClause(queryBuilder, logFilter);
            using (IDbConnection connection = new SqlConnection(_config.ConnectionString))
            {
                return connection.ExecuteScalar<long>(queryBuilder.ToString(), new
                {
                    StartDate = logFilter.StartDate,
                    EndDate = logFilter.EndDate
                });
            }
            
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
                logFilter.OrderByField = nameof(log.Timestamp);
            }
        }

        private void GenerateWhereClause(
            StringBuilder queryBuilder,
            LogFilter logFilter)
        {
            bool firstWhere = true;
            if (logFilter.StartDate.HasValue || logFilter.EndDate.HasValue ||
                !String.IsNullOrEmpty(logFilter.SearchString) || !String.IsNullOrEmpty(logFilter.LevelString))
            {
                queryBuilder.Append("WHERE ");
            }
            if (!String.IsNullOrEmpty(logFilter.SearchString))
            {
                if (!firstWhere)
                { queryBuilder.Append("AND "); }
                queryBuilder.Append($"[{nameof(Log.Message)}] LIKE '%{logFilter.SearchString}%' " +
                    $"OR [{nameof(Log.Exception)}] LIKE '%{logFilter.SearchString}%' ");
                firstWhere = false;
            }
            if (!String.IsNullOrEmpty(logFilter.LevelString))
            {
                if (!firstWhere)
                { queryBuilder.Append("AND "); }
                queryBuilder.Append($"[{nameof(Log.Level)}] = '{logFilter.LevelString}' ");
                firstWhere = false;
            }
            if(logFilter.StartDate.HasValue)
            {
                if (!firstWhere)
                { queryBuilder.Append("AND "); }
                //queryBuilder.Append($"[{nameof(Log.TimeStamp)}] >= {logFilter.StartDate.Value} ");
                queryBuilder.Append($"[{nameof(Log.Timestamp)}] >= @StartDate ");
                firstWhere = false;
            }
            if (logFilter.EndDate.HasValue)
            {
                if (!firstWhere)
                { queryBuilder.Append("AND "); }
                queryBuilder.Append($"[{nameof(Log.Timestamp)}] <= @EndDate ");
                firstWhere = false;
            }
        }
    }
}
