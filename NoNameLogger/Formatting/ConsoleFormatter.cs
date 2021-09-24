using NoNameLogger.Events;
using NoNameLogger.Interfaces;
using NoNameLogger.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NoNameLogger.Formatting
{
    public class ConsoleFormatter : IFormatter
    {
        public void Serialize(StreamWriter streamWriter, Log log)
        {
            streamWriter.WriteLine(log.ToString());
        }

        public void Serialize(TextWriter textWriter, Log log)
        {
            textWriter.WriteLine(log.ToString());
        }
    }
}
