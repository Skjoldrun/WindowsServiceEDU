using Microsoft.Extensions.Logging.Abstractions;
using WindowsServiceEDU.Net.Utilities;

namespace WindowsServiceEDU.Net;

public class Worker : BackgroundService
{
    private readonly IConfiguration _config;
    private readonly ILogger<Worker> _logger;
    private int _workerIntervalInSec;

    public Worker(IConfiguration config, ILogger<Worker> logger = null)
    {
        _config = config;
        _logger = logger ?? NullLogger<Worker>.Instance;
        _workerIntervalInSec = _config.GetSection("AppSettings").GetValue<int>("WorkerIntervalInSec");
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            // do something here ...
            _logger.LogInformation("Worker running every {interval} sec in {Env} envronment at: {time}",
                _workerIntervalInSec, DateTimeOffset.Now, AppSettingsHelper.GetEnvVarName());

            await Task.Delay(_workerIntervalInSec * 1000, stoppingToken);
        }
    }

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Application {name} start", ThisAssembly.AssemblyName);
        return base.StartAsync(cancellationToken);
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Application {name} stop", ThisAssembly.AssemblyName);
        return base.StopAsync(cancellationToken);
    }
}