Logger configuration
```csharp
using NoNameLogger.Services;
using NoNameLoggerMySql; 
...
var noNameLoggerConfig = new LoggerConfiguration()
    .WriteTo.MySql("connectionString", "Logs");
    NoNameLogger.Interfaces.ILogger logger = noNameLoggerConfig.CreateLoggger();
    
```

Table schema
```sql
+-----------------+--------------+------+-----+-------------------+-------------------+
| Field           | Type         | Null | Key | Default           | Extra             |
+-----------------+--------------+------+-----+-------------------+-------------------+
| Id              | bigint       | NO   | PRI | NULL              | auto_increment    |
| Timestamp       | varchar(100) | YES  |     | NULL              |                   |
| Level           | varchar(15)  | YES  |     | NULL              |                   |
| MessageTemplate | text         | YES  |     | NULL              |                   |
| Message         | text         | YES  |     | NULL              |                   |
| Exception       | text         | YES  |     | NULL              |                   |
| Properties      | text         | YES  |     | NULL              |                   |
| _ts             | timestamp    | YES  |     | CURRENT_TIMESTAMP | DEFAULT_GENERATED |
+-----------------+--------------+------+-----+-------------------+-------------------+
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