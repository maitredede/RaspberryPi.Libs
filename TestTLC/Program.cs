using System;
using RaspberryPi.PiGPIO;
using RaspberryPi.PiGPIO.Drivers;
using RaspberryPi.PiGPIO.Drivers.Dede;
using RaspberryPi.PiGPIO.Animations;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.CommandLineUtils;
using System.Globalization;

namespace TestTLC
{
    public static class Program
    {
        static readonly int gpioClock = 17;
        static readonly int gpioData = 10;
        static readonly int gpioLatch = 9;
        static readonly int gpioOE = 4;

        static readonly Random rnd = new Random();

        private static CommandLineApplication app;
        private static CommandLineApplication cmdRemote;
        private static CommandOption optHost;
        private static CommandOption optPort;
        private static CommandLineApplication cmdLib;

        public static int Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("fr-fr", "fr");
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

            app = new CommandLineApplication();
            app.OnExecute(new Func<int>(() =>
            {
                app.ShowHelp();
                return -1;
            }));
            cmdRemote = app.Command("remote", cfg =>
            {
                optHost = cfg.Option("-h|--host", "PiGPIO host", CommandOptionType.SingleValue);
                optPort = cfg.Option("-p|--port", "PiGPIO port", CommandOptionType.SingleValue);
            });
            cmdRemote.OnExecute(new Func<int>(RunRemote));
            cmdLib = app.Command("lib", cfg =>
            {
            });
            cmdLib.OnExecute(new Func<int>(RunLib));

            app.HelpOption("-?|--help");

            return app.Execute(args);
        }

        private static int RunRemote()
        {
            string host = "localhost";
            int port = 8888;

            if (optHost.HasValue())
                host = optHost.Value();
            if (optPort.HasValue())
            {
                if (!int.TryParse(optPort.Value(), out port))
                {
                    Console.Error.WriteLine("Invalid port value");
                    return -1;
                }
            }

            Console.WriteLine($"Connecting to {host}:{port}");
            using (var pigpio = new PigsClient(host, port))
            {
                pigpio.ConnectAsync().Wait();
                return Run(pigpio);
            }
        }

        private static int RunLib()
        {
            Console.WriteLine("Starting as library");
            using (var gpio = new PiGpio())
            {
                return Run(gpio);
            }
        }

        private static int Run(IPiGPIO gpio)
        {
            Console.WriteLine("Starting application...");
            using (gpio)
            {
                var tlc = new TLC5947(gpio, 1, gpioClock, gpioData, gpioLatch, gpioOE);
                {
                    Console.WriteLine("Starting TLC5947 driver...");
                    tlc.Init();

                    bool run = true;
                    Console.CancelKeyPress += (s, e) =>
                    {
                        if (run)
                        {
                            e.Cancel = true;
                            run = false;
                            Console.WriteLine("Soft exiting...");
                        }
                        else
                        {
                            Console.WriteLine("Hard exiting...");
                        }
                    };

                    System.Diagnostics.Stopwatch sw = System.Diagnostics.Stopwatch.StartNew();
                    while (run)
                    {
                        double h = Math.Sin(sw.Elapsed.TotalSeconds) / 2 + .5;
                        Conversions.hsv2rgb(h, 1, 1, out byte r, out byte g, out byte b);
                        tlc.SetLED(0, Color.FromArgb(r, g, b));
                        tlc.Write();
                        Thread.Sleep(1);

                        if (Console.KeyAvailable)
                        {
                            var key = Console.ReadKey(true);
                            if (key.Key == ConsoleKey.Spacebar && key.Modifiers == (ConsoleModifiers)0)
                            {
                                tlc.SetOutputEnabled(!tlc.OutputEnabled);
                                Console.Write(tlc.OutputEnabled ? "/" : "\\");
                            }
                        }
                        Console.Write(".");
                    }
                }
            }
            Console.WriteLine("Exiting application...");
            return 0;
        }

        private static async Task Set(ITLC5947 tlc, Color color, ConsoleColor cc, CancellationToken cancellationToken)
        {
            int x = Console.CursorLeft;
            int y = Console.CursorTop;
            ConsoleColor oldcc = Console.BackgroundColor;
            try
            {
                Console.SetCursorPosition(0, 0);
                Console.BackgroundColor = cc;
                Console.Write(" ");
            }
            finally
            {
                Console.BackgroundColor = oldcc;
                Console.SetCursorPosition(x, y);
            }
            tlc.SetLED(0, color);
            tlc.Write();
            //await Task.Delay(333, cancellationToken);
        }
    }
}
