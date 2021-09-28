using System;
using System.Collections.Generic;
using System.Text;

namespace NoNameLoggerUI.Filters
{
    public class LogFilter
    {        
        public string OrderBy { get; set; }
        public string OrderByField { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string LevelString { get; set; }
        public string SearchString { get; set; }
    }
}
