Configuration of everething example
```csharp
using NoNameLogger.AspNetCore;
using NoNameLogger.Formatting;
using NoNameLogger.Services;
using NoNameLogger.Enums;
using NoNameLogger.LoggerConfigExtensions;
...
var noNameLoggerConfig = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
        @"logs", "mylog.json"), new JsonFormatter(), rollingInterval: RollingInterval.Day)
    .WriteTo.File(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
        @"logs", "mylog.bin"), new BinaryFormatter(), rollingInterval: RollingInterval.Hour)
    .WriteTo.File(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
            @"logs", "mylog.xml"), new XmlFormatter())
    .WriteTo.Http(config =>
    {
        config.Formatter = new JsonFormatter();
        config.Url = "http://localhost:5341/api/events/raw";
        config.ContentType = "application/json";
        config.BeforeLogData = "{\"Events\": [";
        config.AfterLogData = "]}";
        config.MethodRequest = "POST";
        return config;
    })
    .WriteTo.Email(configuration => 
    {
       configuration.EmailFrom = "emailFrom@gmail.com";
       configuration.EmailTo = "emailTo@gmail.com";
       configuration.Password = "password";
       configuration.SmtpHost = "smtp.gmail.com";
       configuration.SmtpPort = 587;
       configuration.MinLogLevel = NoNameLogger.Enums.LogLevel.Critical;
       configuration.MaxLogLevel = NoNameLogger.Enums.LogLevel.Critical;
       configuration.Formatter = new JsonFormatter();
       return configuration;
    });
    NoNameLogger.Interfaces.ILogger logger = noNameLoggerConfig.CreateLoggger();
    logger.LogDebug("Logger configurated successfully!");
```