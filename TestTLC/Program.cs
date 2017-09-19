using System;
using RaspberryPi.PiGPIO;
using RaspberryPi.PiGPIO.Drivers;
using RaspberryPi.PiGPIO.Drivers.Dede;
using RaspberryPi.PiGPIO.Animations;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;

namespace TestTLC
{
    public static class Program
    {
        //const int gpioClock = 2;
        //const int gpioData = 3;
        //const int gpioLatch = 4;
        //const int gpioDummyMiso = 18;
        //const int gpioOE = 17;

        static readonly int gpioClock = 17;
        static readonly int gpioData = 10;
        static readonly int gpioLatch = 9;
        static readonly int gpioDummyMiso = 2;
        static readonly int gpioOE = 4;

        static readonly Random rnd = new Random();

        static Task Main(string[] args)
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            Console.CancelKeyPress += (s, e) =>
            {
                if (!cts.IsCancellationRequested)
                {
                    Console.WriteLine("Soft application exit...");
                    e.Cancel = true;
                    cts.Cancel();
                }
                else
                {
                    Console.WriteLine("Hard application exit...");
                }
            };

            return Application(cts.Token);
        }

        private static async Task Application(CancellationToken cancellationToken)
        {
            Console.WriteLine("Starting application...");
            using (PigsClient pigs = new PigsClient("192.168.20.22", 8888))
            {
                Console.WriteLine("Connecting gpio...");
                await pigs.ConnectAsync();
                var tlc = new RaspberryPi.PiGPIO.Drivers.Dede.TLC5947(pigs, 1, gpioClock, gpioData, gpioLatch, gpioDummyMiso, gpioOE);
                //var tlc = new RaspberryPi.PiGPIO.Drivers.Adafruit.TLC5947(pigs, 1, gpioClock, gpioData, gpioLatch, gpioOE);
                {
                    Console.WriteLine("Starting TLC5947 driver...");
                    tlc.Begin();

                    Action time = () =>
                    {
                        DateTime now = DateTime.Now;
                        tlc.Write();
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
                        DateTime newNow = DateTime.Now;
                        TimeSpan diff = newNow - now;
                        if (diff < TimeSpan.FromSeconds(.5))
                        {
                            Thread.Sleep(TimeSpan.FromSeconds(.5) - diff);
                        }
                    };
                    while (!cancellationToken.IsCancellationRequested)
                    {
                        //tlc.SetPWM(0, val ? 4095 : 0);
                        //tlc.SetPWM(1, val ? 0 : 4095);
                        //Console.Write(val ? "+" : ".");
                        tlc.SetLED(0, Color.FromArgb(255, 0, 0));
                        System.Diagnostics.Debug.WriteLine("Red");
                        time();
                        tlc.SetLED(0, Color.FromArgb(0, 255, 0));
                        System.Diagnostics.Debug.WriteLine("Green");
                        time();
                        tlc.SetLED(0, Color.FromArgb(0, 0, 255));
                        System.Diagnostics.Debug.WriteLine("Blue");
                        time();
                    }
                }
            }
            Console.WriteLine("Exiting application...");
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
