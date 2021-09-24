using NoNameLogger.Configs;
using NoNameLogger.Events;
using NoNameLogger.Extensions;
using NoNameLogger.Interfaces;
using System;
using System.IO;
using System.Threading;
using System.Linq;

namespace NoNameLogger.Services
{
    class LogInFile:ILog, IDisposable
    {
        public static AutoResetEvent _waitHandler = new AutoResetEvent(true);
        private FileConfig _fileConfig;
        StreamWriter _streamWriter = null;
        private bool _isDisposed = false;
        private string _fileNameWithoutExtension = "";
        private Timer _timerStreamWriterFlush = null;
        private FileInfo _fileInfo;
        private DateTime _dateTimeLastCreatedFile;
        public LogInFile(FileConfig fileConfig)
        {
            _fileConfig = fileConfig;
            

            
        }

        

        public void Dispose()
        {
            if(!_isDisposed)
            {
                _timerStreamWriterFlush?.Dispose();
                _streamWriter?.Flush();
                _streamWriter?.Dispose();
                _isDisposed = true;
            }
        }

        public void Log(LogEvent logEvent)
        {
            try
            {
                if(logEvent.LogLevel.CheckLogLeavel(_fileConfig))
                {
                    PrepareToSave();
                    CheckFileSize();
                    CheckRollingInterval();
                    _fileConfig.Formatter.Serialize(_streamWriter, logEvent.ToLog());
                    AfterSave();
                }                
            }
            catch(Exception ex)
            {

            }            
        }

        private void AfterSave()
        {
            if ((_timerStreamWriterFlush is null) &&
                (_fileConfig.FlushToDiskInterval.HasValue))
            {
                _timerStreamWriterFlush = new Timer(new TimerCallback(StreamWriterFlush), null,
                   1000, Convert.ToInt32(_fileConfig.FlushToDiskInterval.Value.TotalSeconds));
            }
            else
            {
                StreamWriterFlush(null);
            }
        }

        private void PrepareToSave()
        {
            PrepareFileNameAndPath();
            if (_streamWriter is null)
            {
                try
                {
                    _waitHandler.WaitOne();
                    _streamWriter = new StreamWriter(_fileInfo.FullName,
                    true, _fileConfig.Encoding, 2048);
                }
                finally
                {
                    _waitHandler.Set();
                }
                
            }
            
            //_streamWriter.WriteLine(content);
            
        }

        private void PrepareFileNameAndPath()
        {
            string path = Path.GetDirectoryName(_fileConfig.Path);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            string fullPath = GenerateFileName(path);
            _fileInfo = new FileInfo(fullPath);
        }

        private string GenerateFileName(string path = null, bool overSize = false)
        {
            if(String.IsNullOrEmpty(path))
            {
                path = Path.GetDirectoryName(_fileConfig.Path);
            }
            string temp = "";
            if (!overSize)
            {
                _dateTimeLastCreatedFile = DateTime.Now;
                temp = Path.GetFileNameWithoutExtension(_fileConfig.Path);
                if (_fileConfig.RollingInterval == RollingInterval.Year)
                { temp = $"{temp}{_dateTimeLastCreatedFile.ToString("yyyy")}"; }
                else if (_fileConfig.RollingInterval == RollingInterval.Month)
                { temp = $"{temp}{_dateTimeLastCreatedFile.ToString("yyyyMM")}"; }
                else if (_fileConfig.RollingInterval == RollingInterval.Day)
                { temp = $"{temp}{_dateTimeLastCreatedFile.ToString("yyyyMMdd")}"; }
                else if (_fileConfig.RollingInterval == RollingInterval.Hour)
                { temp = $"{temp}{_dateTimeLastCreatedFile.ToString("yyyyMMddhh")}"; }
                else if (_fileConfig.RollingInterval == RollingInterval.Minute)
                { temp = $"{temp}{_dateTimeLastCreatedFile.ToString("yyyyMMddhhmm")}"; }
                string[] vs = Directory.GetFiles(path);
                foreach(string s in vs)//if file name contain number. Example: log20210921_(1).json
                {
                    string s2 = Path.GetFileNameWithoutExtension(s);
                    if (s2.Contains(_fileNameWithoutExtension))
                    {
                        _fileNameWithoutExtension = s2;
                    }
                    else
                        _fileNameWithoutExtension = temp;
                }
                
            }
            else
            {
                int indexStart = _fileNameWithoutExtension.IndexOf('('), indexEnd = _fileNameWithoutExtension.IndexOf(')');
                if ((indexStart > -1) &&(indexEnd > -1))
                {
                    int count;
                    if(!int.TryParse(_fileNameWithoutExtension.Substring(indexStart, indexEnd), out count))
                    {
                        count = int.MinValue;
                    }
                    
                    temp = $"{_fileNameWithoutExtension.Substring(0, indexStart)}({count})";

                }
                else
                {
                    temp = $"{_fileNameWithoutExtension}_({1})";
                }
            }
            
            return Path.Combine(path, $"{temp}{Path.GetExtension(_fileConfig.Path)}");
        }

        private void CheckFileSize()
        {            
            if (_fileConfig.RollOnFileSizeLimit)
            { 
                if (_fileInfo.Length >= _fileConfig.FileSizeLimitBytes)
                {
                    UpdateStreamWriter(true);
                }
            }
        }

        private void UpdateStreamWriter(bool oversize = false)
        {
            try
            {
                _waitHandler.WaitOne();
                _fileInfo = new FileInfo(GenerateFileName(null, oversize));
                _streamWriter.Flush();
                _streamWriter.Dispose();
                _streamWriter = new StreamWriter(_fileInfo.FullName);
            }
            finally
            {
                _waitHandler.Set();
            }
        }

        private void CheckRollingInterval()
        {
            if(_fileConfig.RollingInterval != RollingInterval.Infinite)
            {
                bool updateFileName = false;
                if ((_fileConfig.RollingInterval == RollingInterval.Year) &&
                    (DateTime.Now.Year != _dateTimeLastCreatedFile.Year))
                {
                    updateFileName = true;
                }
                else if ((_fileConfig.RollingInterval == RollingInterval.Month) &&
                    (DateTime.Now.Month != _dateTimeLastCreatedFile.Month) &&
                    (DateTime.Now.Year != _dateTimeLastCreatedFile.Year))
                {
                    updateFileName = true;
                }
                else if ((_fileConfig.RollingInterval == RollingInterval.Day) &&
                    ((DateTime.Now - _dateTimeLastCreatedFile) > TimeSpan.FromDays(1)))
                {
                    updateFileName = true;
                }
                else if ((_fileConfig.RollingInterval == RollingInterval.Hour) &&
                    ((DateTime.Now - _dateTimeLastCreatedFile) > TimeSpan.FromHours(1)))
                {
                    updateFileName = true;
                }
                else if ((_fileConfig.RollingInterval == RollingInterval.Minute) &&
                    ((DateTime.Now - _dateTimeLastCreatedFile) > TimeSpan.FromMinutes(1)))
                {
                    updateFileName = true;
                }
                if (updateFileName)
                    UpdateStreamWriter();
            }            
        }

        void StreamWriterFlush(object state)
        {
            _streamWriter?.Flush();
        }
    }
}