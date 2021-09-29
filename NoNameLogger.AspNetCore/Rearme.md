
Configuration of everything
In the `Program`:
```csharp
public static void Main(string[] args)
{
    string connection = "ConnectionString";
    var myLoggerConfig = new LoggerConfiguration()
        .WriteTo.Console()
        .WriteTo.File(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
            @"logs", "mylog.json"), new JsonFormatter(), rollingInterval: RollingInterval.Day)
        .WriteTo.File(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
            @"logs", "mylog.bin"), new BinaryFormatter(), rollingInterval: RollingInterval.Hour)
        .WriteTo.File(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                @"logs", "mylog.xml"), new XmlFormatter(), rollingInterval: RollingInterval.Minute)
        .WriteTo.MsSQLServer(connection, "TargetTable");
    NoNameLogger.Interfaces.ILogger logger = myLoggerConfig.CreateLoggger();
    try
    {
        var host = CreateHostBuilder(args, logger).Build();
        host.Run();
    }
    catch(Exception ex)
    {
        logger.LogCritical($"{ex?.Message} " +
            $"{ex?.StackTrace} {ex?.HResult}");
    }
}

public static IHostBuilder CreateHostBuilder(string[] args, NoNameLogger.Interfaces.ILogger logger) =>
    Host.CreateDefaultBuilder(args)
    .ConfigureLogging(log =>
    {
        log.ClearProviders();
        log.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
        log.AddNoNameLogger(configuration =>
        {
            configuration.Logger = logger;
        });
    })
    .ConfigureWebHostDefaults(webBuilder =>
    {
        webBuilder.UseStartup<Startup>();
    })
    .ConfigureServices(services =>
    {
        services.AddHostedService<UpdateRedisService>();
    });
```
In the `Startup`
```csharp
public void ConfigureServices(IServiceCollection services)
{
    .
    .
    services.AddNoNameLoggerUi(options => options.UseSqlServer("ConnectingString", "TargetTable"));
}

public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    .
    .
    .
    .
    .
    .

    app.UseRouting();
    app.UseAuthentication();
    app.UseAuthorization();
        
    // Enable middleware to serve log-ui (HTML, JS, CSS, etc.).
    app.UseNoNameLoggerUI();

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
    });
}

```

Table schema:
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