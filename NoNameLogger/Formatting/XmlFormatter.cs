using NoNameLogger.Model;
using NoNameLogger.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace NoNameLogger.Formatting
{
    public class XmlFormatter : IFormatter
    {
        public void Serialize(StreamWriter streamWriter, Log log)
        {
            var serializer = new XmlSerializer(log.GetType());
                serializer.Serialize(streamWriter, log);            
        }

        public void Serialize(TextWriter textWriter, Log log)
        {
            var serializer = new XmlSerializer(log.GetType());
            serializer.Serialize(textWriter, log);
        }
    }
}
