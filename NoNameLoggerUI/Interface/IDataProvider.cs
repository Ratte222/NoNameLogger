using NoNameLogger.Model;
using NoNameLoggerUI.Filters;
using NoNameLoggerUI.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace NoNameLoggerUI.Interface
{
    public interface IDataProvider
    {
        IEnumerable<Log> FetchLogs(LogFilter logFilter, PageResponse<Log> pageResponse);
        long CountLogs(LogFilter logFilter);
    }
}
