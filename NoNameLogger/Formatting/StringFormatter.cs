using NoNameLogger.Events;
using NoNameLogger.Interfaces;
using NoNameLogger.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NoNameLogger.Formatting
{
    public class StringFormatter : IFormatter
    {
        public void Serialize(StreamWriter streamWriter, Log log)
        {
            streamWriter.WriteLine(log.ToString());
        }

        public void Serialize(TextWriter textWriter, Log log)
        {
            textWriter.WriteLine(log.ToString());
        }

        public string Serialize(Log log)
        {
            return log.ToString();
        }
    }
}
