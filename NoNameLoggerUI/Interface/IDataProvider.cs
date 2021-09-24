using NoNameLogger.Model;
using NoNameLoggerUI.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace NoNameLoggerUI.Interface
{
    public interface IDataProvider
    {
        IEnumerable<Log> FetchLogs(LogFilter logFilter);
        long CountLogs(LogFilter logFilter);
    }
}
