﻿using NoNameLogger.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace NoNameLoggerMsSql.Configs
{
    internal class MsSQLConfig
    {
        public IFormatter Formatter { get; set; }
        public string ConnectionString { get; set; }
        public string TableName { get; set; }
        public string SchemaName { get; set; }
        public bool CreateTable { get; set; }
    }
}
