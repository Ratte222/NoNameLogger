Logger configuration
```csharp
using NoNameLogger.Services;
using NoNameLoggerMsSql; 
...
var noNameLoggerConfig = new LoggerConfiguration()
    .WriteTo.MsSQLServer("connectionString", "Logs");
    NoNameLogger.Interfaces.ILogger logger = noNameLoggerConfig.CreateLoggger();
    
```

Table schema
```sql
[Id]              BIGINT         IDENTITY (1, 1) NOT NULL,
    [Message]         NVARCHAR (MAX) NULL,
    [MessageTemplate] NVARCHAR (MAX) NULL,
    [Level]           NVARCHAR (128) NULL,
    [TimeStamp]       DATETIME2 (7)  NOT NULL,
    [Exception]       NVARCHAR (MAX) NULL,
    [Properties]      NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_Logs] PRIMARY KEY CLUSTERED ([Id] ASC)
```
```csharp
public long Id { get; set; }
public string Message { get; set; }
public string MessageTemplate { get; set; }
[StringLength(128)]
public string Level { get; set; }
public DateTime TimeStamp { get; set; }
public string Exception { get; set; }
public string Properties { get; set; }
```