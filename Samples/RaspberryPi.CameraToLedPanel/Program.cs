using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.CommandLineUtils;
using System;
using RaspberryPi.Camera;
using RaspberryPi.LibLedMatrix;
using RaspberryPi.Userland;
using RaspberryPi.Userland.Interfaces;

namespace RaspberryPi.CameraToLedPanel
{
    public static class Program
    {
        private static CommandLineApplication app;

        public static int Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            app = new CommandLineApplication();
            app.OnExecute(new Func<int>(RunApplication));
            return app.Execute(args);
        }

        private static int RunApplication()
        {
            ServiceCollection serviceConfig = new ServiceCollection();
            serviceConfig.AddSingleton<IBcmHost, BcmHost>();
            serviceConfig.AddSingleton<LedMatrixOptions>(svc =>
            {
                return new LedMatrixOptions()
                {
                    chain_length = 2,
                    rows = 64,
                    led_rgb_sequence = "RBG"
                };
            });
            serviceConfig.AddSingleton<LedMatrix>(svc =>
            {
                return new LedMatrix(svc.GetRequiredService<LedMatrixOptions>());
            });

            using (ServiceProvider svcProvider = serviceConfig.BuildServiceProvider())
            {
                return RunApplication(svcProvider);
            }
        }

        private static int RunApplication(IServiceProvider svc)
        {
            IBcmHost host = svc.GetRequiredService<IBcmHost>();
            using()
        }
    }
}
