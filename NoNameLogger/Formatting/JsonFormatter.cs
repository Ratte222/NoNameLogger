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
        
        JsonSerializer _serializer = null;
        JsonTextWriter _writer = null;
        StreamWriter _streamWriter = null;
        TextWriter _textWriter = null;
        //bool isDisposed = false;
        //public void Dispose()
        //{
        //    if(!isDisposed)
        //    {
                
        //    }
        //}

        public void Serialize(StreamWriter streamWriter, Log log)
        {
            if(_serializer is null)
            {
                _serializer = new JsonSerializer();
                _serializer.Converters.Add(new JavaScriptDateTimeConverter());
                _serializer.NullValueHandling = NullValueHandling.Ignore;
            }            
            if(_writer is null)
            {
                _streamWriter = streamWriter;
                _writer = new JsonTextWriter(streamWriter);
            }
            else if (!streamWriter.Equals(_streamWriter))
            {
                _streamWriter = streamWriter;
                _writer = new JsonTextWriter(streamWriter);
            }

                //writer.Formatting = Newtonsoft.Json.Formatting.Indented;
                //writer.Indentation = 4;
                //writer.IndentChar = ' ';
                _serializer.Serialize(_writer, log);
            streamWriter.WriteLine("");
        }

        public void Serialize(TextWriter textWriter, Log log)
        {
            if (_serializer is null)
            {
                _serializer = new JsonSerializer();
                _serializer.Converters.Add(new JavaScriptDateTimeConverter());
                _serializer.NullValueHandling = NullValueHandling.Ignore;
            }
            if (_writer is null)
            {
                _writer = new JsonTextWriter(textWriter);
            }
            else if (!textWriter.Equals(_textWriter))
            {
                _textWriter = textWriter;
                _writer = new JsonTextWriter(textWriter);
            }

            _serializer.Serialize(_writer, log);
            textWriter.WriteLine("");
        }

        public string Serialize(Log log)
        {
            return JsonConvert.SerializeObject(log);
        }
    }
}