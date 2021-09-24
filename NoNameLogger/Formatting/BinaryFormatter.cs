using NoNameLogger.Interfaces;
using NoNameLogger.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using bin = System.Runtime.Serialization.Formatters.Binary;

namespace NoNameLogger.Formatting
{
    public class BinaryFormatter : IFormatter
    {
        public void Serialize(StreamWriter streamWriter, Log log)
        {
            bin.BinaryFormatter formatter = new bin.BinaryFormatter();
            formatter.Serialize(streamWriter.BaseStream, log);
            streamWriter.WriteLine("");
        }

        public void Serialize(TextWriter textWriter, Log log)
        {
            using(var stream = new MemoryStream())
            {
                bin.BinaryFormatter formatter = new bin.BinaryFormatter();
                formatter.Serialize(stream, log);
                using (var reader = new StreamReader(stream))
                {
                    textWriter.WriteLine(reader.ReadToEnd());
                }
            }
        }
    }
}
