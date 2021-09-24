using NoNameLogger.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.IO;
using NoNameLogger.Model;

namespace NoNameLogger.Formatting
{
    public class JsonFormatter: IFormatter
    {
        
        JsonSerializer serializer = null;
        JsonTextWriter writer = null;

        //bool isDisposed = false;
        //public void Dispose()
        //{
        //    if(!isDisposed)
        //    {
                
        //    }
        //}

        public void Serialize(StreamWriter streamWriter, Log log)
        {
            if(serializer is null)
            {
                serializer = new JsonSerializer();
                serializer.Converters.Add(new JavaScriptDateTimeConverter());
                serializer.NullValueHandling = NullValueHandling.Ignore;
            }
            if(writer is null)
            {
                writer = new JsonTextWriter(streamWriter);
            }


            //writer.Formatting = Newtonsoft.Json.Formatting.Indented;
            //writer.Indentation = 4;
            //writer.IndentChar = ' ';
            serializer.Serialize(writer, log);
            streamWriter.WriteLine("");
            
        }

        public void Serialize(TextWriter textWriter, Log log)
        {
            if (serializer is null)
            {
                serializer = new JsonSerializer();
                serializer.Converters.Add(new JavaScriptDateTimeConverter());
                serializer.NullValueHandling = NullValueHandling.Ignore;
            }
            if (writer is null)
            {
                writer = new JsonTextWriter(textWriter);
            }

            serializer.Serialize(writer, log);
            textWriter.WriteLine("");
        }
    }
}