using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Reflection;
using System.ServiceProcess;
using System.Threading;
using WindowsServiceEDU.Net48.Logging;

namespace WindowsServiceEDU.Net48
{
    internal static class Program
    {
        private static bool _keepRunning = true;

        /// <summary>
        /// Host object with dependency injection registration
        /// </summary>
        public static IHost AppHost { get; set; }

        private static void Main()
        {
            var serilogLogger = LogInitializer.CreateLogger();
            Log.Logger = serilogLogger;

            Log.Information("{ApplicationName} start", ThisAssembly.AssemblyName);

            AppHost = ConfigureHost();

            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                // Your Service instance here ...
                ActivatorUtilities.GetServiceOrCreateInstance<WindowsServiceEDU>(AppHost.Services)
            };

            if (Environment.UserInteractive)
            {
                try
                {
                    RunInteractive(ServicesToRun);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "An error occured while running the service in interactive mode: {Message}", ex.Message);
                    throw;
                }
            }
            else
            {
                try
                {
                    ServiceBase.Run(ServicesToRun);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "An error occured while running the service: {Message}", ex.Message);
                    throw;
                }
            }

            Log.Information("{ApplicationName} stop", ThisAssembly.AssemblyName);
            Log.CloseAndFlush();
        }

        /// <summary>
        /// Configures the host with registering the interfaces and class types.
        /// Sets the Serilog logger as logging provider for typed ILogger injections.
        /// </summary>
        /// <returns>configured host to access its services</returns>
        private static IHost ConfigureHost()
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    // DI registry here ...
                })
                .UseSerilog()
                .Build();
            return host;
        }

        /// <summary>
        /// Runs the application in interactive mode as console application instead of a win serivce.
        /// </summary>
        private static void RunInteractive(ServiceBase[] servicesToRun)
        {
            // Add handler for [Ctrl]+[C] press
            Console.CancelKeyPress += delegate (object sender, ConsoleCancelEventArgs e)
            {
                e.Cancel = true;
                _keepRunning = false;
                Console.WriteLine("Received stop signal, will exit the application ...");
            };

            Console.WriteLine("Services running in interactive mode.");
            Console.WriteLine();

            MethodInfo onStartMethod = typeof(ServiceBase).GetMethod("OnStart", BindingFlags.Instance | BindingFlags.NonPublic);
            foreach (ServiceBase service in servicesToRun)
            {
                Console.WriteLine("Starting {0}...", service.ServiceName);
                onStartMethod.Invoke(service, new object[] { new string[] { } });
                Console.WriteLine("Started");
            }

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Press [Ctrl]+[C] to exit the application ...");
            while (_keepRunning)
                Thread.Sleep(1000);
            Console.WriteLine();

            MethodInfo onStopMethod = typeof(ServiceBase).GetMethod("OnStop", BindingFlags.Instance | BindingFlags.NonPublic);
            foreach (ServiceBase service in servicesToRun)
            {
                Console.WriteLine("Stopping {0}...", service.ServiceName);
                onStopMethod.Invoke(service, null);
                Console.WriteLine("{0} Stopped", service.ServiceName);
            }

            Console.WriteLine("All services stopped.");
            // Keep the console alive for a second to allow the user to see the message.
            Thread.Sleep(1000);
        }
    }
}