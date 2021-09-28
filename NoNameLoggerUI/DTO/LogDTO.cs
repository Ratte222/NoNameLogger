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
        public DateTime Timestemp { get; set; }
        public string Properties { get; set; }
    }
}
