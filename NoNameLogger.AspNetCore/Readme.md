
Configuration example
In the `Program`:
```csharp
using NoNameLogger.AspNetCore;
using NoNameLogger.Formatting;
using NoNameLogger.Services;
using NoNameLogger.Enums;
using NoNameLogger.LoggerConfigExtensions;
using NoNameLoggerMySQL;
...
public static void Main(string[] args)
{
    string connection = "ConnectionString";
    var noNameLoggerConfig = new LoggerConfiguration()
        .WriteTo.Console()
        .WriteTo.File(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
            @"logs", "mylog.json"), new JsonFormatter(), rollingInterval: RollingInterval.Day)
        .WriteTo.MsSQLServer(connection, "TargetTable");
        NoNameLogger.Interfaces.ILogger logger = noNameLoggerConfig.CreateLoggger();
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
using NoNameLoggerUI.Extensions;
using NoNameLoggerMsSqlServerDataProvider;
...
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
    app.UseNoNameLoggerUI();//add for noname logger ui
    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
    });
}

```

