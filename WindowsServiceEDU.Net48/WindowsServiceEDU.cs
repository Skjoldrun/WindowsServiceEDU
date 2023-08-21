using Microsoft.Extensions.Logging;
using System;
using System.ServiceProcess;
using System.Timers;

namespace WindowsServiceEDU.Net48
{
    internal partial class WindowsServiceEDU : ServiceBase
    {
        private readonly ILogger<WindowsServiceEDU> _logger;
        private readonly Timer _timer = new Timer();

        public WindowsServiceEDU(ILogger<WindowsServiceEDU> logger)
        {
            InitializeComponent();

            _logger = logger;
            _timer.Interval = Properties.Settings.Default.ServiceTimerIntervalInSec * 1000;
            _timer.Elapsed += new ElapsedEventHandler(OnTimerTick);
        }

        private void OnTimerTick(object sender, ElapsedEventArgs e)
        {
            // do something here ...
            _logger.LogInformation("Service running at: {time}", DateTimeOffset.Now);
        }

        protected override void OnStart(string[] args)
        {
            _timer.Start();
            _logger.LogInformation("Service {name} start", nameof(WindowsServiceEDU));
        }

        protected override void OnStop()
        {
            _timer.Stop();
            _logger.LogInformation("Service {name} stop", nameof(WindowsServiceEDU));
        }
    }
}