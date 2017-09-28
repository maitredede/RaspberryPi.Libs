using RaspberryPi.PiGPIO;
using RaspberryPi.PiGPIO.Drivers;
using RaspberryPi.PiGPIO.Drivers.Dede;
using RaspberryPi.PiGPIO.Drivers.Freetronics;
using System;
using System.Threading;
using SixLabors.Fonts;
using System.Linq;
using RaspberryPi.PiGPIO.Drivers.Dede.PixelFormats;
using SixLabors.Primitives;
using SixLabors.ImageSharp;
using System.Globalization;
using Microsoft.Extensions.CommandLineUtils;
using System.IO;

namespace Dede.DMDTest
{
    public static class Program
    {
        private static readonly int gpioData = 19;
        private static readonly int gpioA = 12;
        private static readonly int gpioB = 16;
        private static readonly int gpioClock = 26;
        private static readonly int gpioStrobe = 6;
        private static readonly int gpioOE = 13;

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
            FontFamily fontFamily;
            if (!SystemFonts.TryFind("Arial", out fontFamily))
            {
                Console.WriteLine("Arial not found, taking first");
                fontFamily = SystemFonts.Families.FirstOrDefault();
            }
            Font font = new Font(fontFamily, 14);

            Console.WriteLine($"Board : {gpio.HardwareName()} (rev {gpio.HardwareRevision()})");
            Console.WriteLine($"PiGPIO version {gpio.PigpioVersion()}");

            DMDPinLayout layout = new DMDPinLayout(gpioData, gpioA, gpioB, gpioClock, gpioStrobe, gpioOE);
            string str = "";

            try
            {
                using (FreetronicsDMDSurface dmd2 = new FreetronicsDMDSurface(gpio, layout))
                {
                    dmd2.Init();

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
                    int offset = 0;
                    Action Save = () =>
                    {
                        Console.Write("Filename=>");
                        string filename;
                        do
                        {
                            filename = $"img{offset.ToString("00000")}.png";
                            offset++;
                        }
                        while (File.Exists(filename));
                        Console.WriteLine($"Saving {filename}");
                        dmd2.SavePNG(filename);
                    };

                    while (run)
                    {
                        if (Console.KeyAvailable)
                        {
                            var key = Console.ReadKey();
                            if (key.Key == ConsoleKey.S)
                            {
                                Save();
                            }
                        }
                        Thread.Sleep(10);
                        string str2 = DateTime.Now.ToLongTimeString();
                        if (str != str2)
                        {
                            str = str2;
                            dmd2.UpdateSurface(img =>
                            {
                                img.Fill(BitPixel.Off);
                                img.DrawText(str, font, BitPixel.On, new PointF(0, 0));
                            });
                            Console.WriteLine(str);
                        }
                    }

                    dmd2.UpdateSurface(u =>
                    {
                        u.Fill(BitPixel.Off);
                    });

                    Thread.Sleep(100);
                }
                return 0;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.GetType().FullName + ": " + ex.Message);
                Console.Error.WriteLine(ex.StackTrace);
                return -1;
            }
        }
    }
}