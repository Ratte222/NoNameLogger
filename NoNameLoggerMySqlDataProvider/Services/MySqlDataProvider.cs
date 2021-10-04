using NoNameLogger.Model;
using NoNameLoggerMySqlDataProvider.Config;
using NoNameLoggerUI.Filters;
using NoNameLoggerUI.Helpers;
using NoNameLoggerUI.Interface;
using NoNameLogger.Extensions;
using MySql.Data.MySqlClient;
using Dapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace NoNameLoggerMySqlDataProvider.Services
{
    class MySqlDataProvider : IDataProvider
    {
        private readonly MySqlDataProviderConfig _config;

        public MySqlDataProvider(MySqlDataProviderConfig config)
        {
            _config = config;
        }

        public long CountLogs(LogFilter logFilter)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append($"SELECT COUNT({nameof(Log.Id)}) FROM `");
            queryBuilder.Append(_config.TableName);
            queryBuilder.Append("` ");
            CheckFilter(logFilter);
            using (var connection = new MySqlConnection(_config.ConnectionString))
            {
                return connection.ExecuteScalar<long>(queryBuilder.ToString(), new
                {
                    StartDate = logFilter.StartDate,
                    EndDate = logFilter.EndDate
                });
            }
        }

        public IEnumerable<Log> FetchLogs(LogFilter logFilter, PageResponse<Log> pageResponse)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append($"SELECT {nameof(Log.Id)}, {nameof(Log.Message)}, `{nameof(Log.Level)}`, " +
                $"{nameof(Log.Timestamp)}, {nameof(Log.Exception)}, {nameof(Log.Properties)} FROM `");
            queryBuilder.Append(_config.TableName);
            queryBuilder.Append("` ");
            CheckFilter(logFilter);
            GenerateWhereClause(queryBuilder, logFilter);
            queryBuilder.Append($" ORDER BY {logFilter.OrderByField} {logFilter.OrderBy} " +
            $"LIMIT {pageResponse.Take} OFFSET {pageResponse.Skip} ");
            using (var connection = new MySqlConnection(_config.ConnectionString))
            {
                return connection.Query<Log>(queryBuilder.ToString(), new
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
                logFilter.OrderBy = "DESC";
            }
            else if (!String.Equals(logFilter.OrderBy, "ASC") && !String.Equals(logFilter.OrderBy, "ASC"))
            {
                logFilter.OrderBy = "DESC";
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
                queryBuilder.Append($"{nameof(Log.Message)} LIKE '%{logFilter.SearchString}%' " +
                    $"OR {nameof(Log.Exception)} LIKE '%{logFilter.SearchString}%' ");
                firstWhere = false;
            }
            if (!String.IsNullOrEmpty(logFilter.LevelString))
            {
                if (!firstWhere)
                { queryBuilder.Append("AND "); }
                queryBuilder.Append($"`{nameof(Log.Level)}` = '{logFilter.LevelString}' ");
                firstWhere = false;
            }
            if (logFilter.StartDate.HasValue)
            {
                if (!firstWhere)
                { queryBuilder.Append("AND "); }
                //queryBuilder.Append($"[{nameof(Log.TimeStamp)}] >= {logFilter.StartDate.Value} ");
                queryBuilder.Append($"{nameof(Log.Timestamp)} >= @StartDate ");
                firstWhere = false;
            }
            if (logFilter.EndDate.HasValue)
            {
                if (!firstWhere)
                { queryBuilder.Append("AND "); }
                queryBuilder.Append($"{nameof(Log.Timestamp)} <= @EndDate ");
                firstWhere = false;
            }
        }

        
    }
}
