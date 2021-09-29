using System;
using System.Collections.Generic;
using System.Text;

namespace NoNameLoggerUI.DTO
{
    class LogDTO
    {
        public string PropertyType { get; set; }
        public int RowNo { get; set; }
        public string Level { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }
        public string Properties { get; set; }
    }
}
