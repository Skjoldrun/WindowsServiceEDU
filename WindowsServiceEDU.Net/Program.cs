using Serilog;
using WindowsServiceEDU.Net.Logging;
using WindowsServiceEDU.Net.Utilities;

namespace WindowsServiceEDU.Net;

public class Program
{
    public static void Main(string[] args)
    {
        var config = AppSettingsHelper.GetAppConfigBuilder().Build();
        Log.Logger = LogInitializer.CreateLogger(config);

        IHost host = Host.CreateDefaultBuilder(args)
            .UseWindowsService()  // activate to get installable Windows Service
            .ConfigureServices(services =>
            {
                services.AddHostedService<Worker>();
            })
            .UseSerilog()
            .Build();

        host.Run();
        Log.CloseAndFlush();
    }
}