using Serilog;

namespace WindowsServiceEDU.Net.Logging;

public static class LogInitializer
{
    /// <summary>
    /// Creates the logger with inline settings.
    /// </summary>
    /// <returns>Logger with inline settings</returns>
    public static Serilog.ILogger CreateLogger()
    {
        return new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Async(a =>
                {
                    a.File("logs/log.txt", rollingInterval: RollingInterval.Hour);
                })
#if DEBUG
                .WriteTo.Console()
                .WriteTo.Debug()
#endif
                .CreateLogger();
    }

    /// <summary>
    /// Creates the logger with settings from appconfig and enrichments from code.
    /// </summary>
    /// <param name="appConfig">appConfig built from appsettings.json</param>
    /// <returns>Logger with inline and app.config settings</returns>
    public static Serilog.ILogger CreateLogger(IConfiguration appConfig)
    {
        return new LoggerConfiguration()
            .ReadFrom.Configuration(appConfig)
            .Enrich.FromLogContext()
            .CreateLogger();
    }

    /// <summary>
    /// Creates a logger of type T for injecting it manually into a constructor.
    /// </summary>
    /// <typeparam name="T">type of class to inject the logger to</typeparam>
    /// <returns>ILogger instance of type T for the class constructor of type T</returns>
    public static ILogger<T> CreateLoggerForInjection<T>()
    {
        // Create a MS extensions ILogger instance of the serilog logger
        var loggerFactory = (ILoggerFactory)new LoggerFactory();
        loggerFactory.AddSerilog(Log.Logger);

        var logger = loggerFactory.CreateLogger<T>();

        return logger;
    }
}