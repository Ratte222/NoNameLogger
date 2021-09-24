﻿using NoNameLogger.Events;
using NoNameLogger.Interfaces;
using NoNameLogger.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NoNameLogger.Services
{
    public class Logger : ILogger
    {
        //delegate void ActionLog(LogEvent logEvent, TextWriter textWriter);
        event Action<LogEvent> _Log;
        public Logger(List<ILog> logs)
        {
            foreach(var act in logs)
            {
                _Log += act.Log;
            }
        }

        public void Log(LogLevel logLevel, string message, Exception exception = null, params object[] args)
        {
            if (message is null) return;
            //if (args != null &&
            //    args.GetType() != typeof(object[]))
            //    args = new object[] { args };
            StringBuilder stringBuilder = new StringBuilder();
            if ((args is null)||(args?.Length == 0))
            {
                var stackFrame = FindStackFrame();
                var methodBase = GetCallingMethodBase(stackFrame);
                var callingMethod = methodBase.Name;
                var callingClass = methodBase.ReflectedType.Name;
                //args = new object[] { callingClass, callingMethod };
                stringBuilder.Append(callingClass);
            }
            else
            {
                foreach (var arg in args)
                {
                    stringBuilder.Append($"{args.GetType()} ");
                }
            }
            
            
            LogEvent logEvent = new LogEvent(logLevel, exception, message, stringBuilder.ToString());
            _Log?.Invoke(logEvent);
        }

        public void LogCritical(string message, params object[] args)
        {            
            Log(LogLevel.Critical, message, null, args);
        }

        public void LogDebug(string message, params object[] args)
        {
            if(args is null)
            {
                var stackFrame = FindStackFrame();
                var methodBase = GetCallingMethodBase(stackFrame);
                var callingMethod = methodBase.Name;
                var callingClass = methodBase.ReflectedType.Name;
                args = new object[] { callingClass, callingMethod };
            }
            Log(LogLevel.Debug, message, null, args);
        }

        

        public void LogErrore(string message, params object[] args)
        {
            Log(LogLevel.Errore, message, null, args);
        }

        public void LogInfo(string message, params object[] args)
        {
            Log(LogLevel.Info, message, null, args);
        }

        public void LogWarning(string message, params object[] args)
        {
            Log(LogLevel.Warning, message, null, args);
        }


        private StackFrame FindStackFrame()
        {
            var stackTrace = new StackTrace();
            List<string> classMethod = typeof(Logger).GetAllPublicInstance();
            classMethod = classMethod.Except(new[] { "GetHashCode", "Equals", "ToString", "GetType" }).ToList();
            for (var i = 0; i < stackTrace.GetFrames().Count(); i++)
            {
                var methodBase = stackTrace.GetFrame(i).GetMethod();
                var name = MethodBase.GetCurrentMethod().Name;
                //if (!methodBase.Name.Equals("Log") && !methodBase.Name.Equals(name))
                if (!classMethod.Any(i=>i == methodBase.Name) && !methodBase.Name.Equals(name))
                    return new StackFrame(i, true);
            }
            return null;
        }

        private static MethodBase GetCallingMethodBase(StackFrame stackFrame)
        {
            return stackFrame == null
                ? MethodBase.GetCurrentMethod() : stackFrame.GetMethod();
        }
    }
}
