using Microsoft.Extensions.CommandLineUtils;
using RaspberryPi.PiGPIO;
using RaspberryPi.PiGPIO.Drivers.Dede;
using System;
using System.Drawing;
using System.Globalization;
using System.Threading;

namespace CoremanDriverPoc
{
    class Program
    {
        private static CoremanHub75LedMatrix drv;

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

        private static IPiGPIO g;
        private static int Run(IPiGPIO gpio)
        {
            Console.WriteLine("Starting application...");
            using (gpio)
            {
                g = gpio;
                Console.WriteLine("Starting driver...");
                drv = new CoremanHub75LedMatrix(gpio, Hub75PinLayout.Regular);
                //Set all to 0
                drv.Init();

                Console.WriteLine("Looping...");
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

                KeyHelp(false);

                bool a = false;
                bool b = false;
                bool o = false;
                bool c = false;
                bool l = false;


                g.Write(drv.Layout.A, a);
                g.Write(drv.Layout.B, b);
                g.Write(drv.Layout.OE, o);
                g.Write(drv.Layout.Strobe, l);
                g.Write(drv.Layout.Clock, c);

                int pos = 0;
                while (run)
                {
                    if (!Console.KeyAvailable)
                    {
                        Thread.Sleep(10);
                        continue;
                    }
                    var key = Console.ReadKey(true);
                    switch (key.Key)
                    {
                        case ConsoleKey.F1: KeyHelp(); break;
                        case ConsoleKey.D: TransmitRowPixel(pos, false, false, true); Console.Write("d"); break;
                        case ConsoleKey.A: a = !a; gpio.Write(drv.Layout.A, a); Console.Write(a ? "A" : "a"); break;
                        case ConsoleKey.B: b = !b; gpio.Write(drv.Layout.B, b); Console.Write(b ? "B" : "b"); break;
                        case ConsoleKey.C: c = !c; gpio.Write(drv.Layout.C, c); Console.Write(c ? "C" : "c"); break;
                        case ConsoleKey.O: o = !o; gpio.Write(drv.Layout.OE, o); Console.Write(o ? "O" : "o"); break;
                        case ConsoleKey.L: l = !l; gpio.Write(drv.Layout.Strobe, l); Console.Write(l ? "L" : "l"); break;
                        case ConsoleKey.Add: pos = Math.Min(63, pos + 1); Console.Write("+{0:X2}", pos); break;
                        case ConsoleKey.Subtract: pos = Math.Max(0, pos - 1); Console.Write("-{0:X2}", pos); break;
                        case ConsoleKey.I: Interleaved(); Console.Write("i"); break;
                        case ConsoleKey.U: Interleaved2(); Console.Write("u"); break;
                        case ConsoleKey.X: ChessBoardScan(); break;
                    }
                }
            }
            return 0;
        }

        private static void KeyHelp() => KeyHelp(true);
        private static void KeyHelp(bool clear)
        {
            if (clear)
                Console.Clear();
            Console.WriteLine("F1\tHelp");
            Console.WriteLine("D\tTransmit 64 row data (set RGB + clock x64)");
            Console.WriteLine("A\tToggle A");
            Console.WriteLine("B\tToggle B");
            Console.WriteLine("O\tToggle OE");
            Console.WriteLine("L\tTootle Strobe");
            Console.WriteLine("+\tPixel +1");
            Console.WriteLine("-\tPixel -1");
            Console.WriteLine("i\tInterleaved test 1");
            Console.WriteLine("u\tInterleaved test 2");
            Console.WriteLine("n\tNext color");
        }

        private static void Clock()
        {
            g.Write(drv.Layout.Clock, true);
            g.Write(drv.Layout.Clock, false);
        }

        private static void Strobe()
        {
            StrobeUp();
            StrobeDown();
        }

        private static void StrobeUp()
        {
            g.Write(drv.Layout.Strobe, true);
        }

        private static void StrobeDown()
        {
            g.Write(drv.Layout.Strobe, true);
        }

        private static void StrobeNC()
        {
            g.Write(drv.Layout.C, true);
            g.Write(drv.Layout.C, false);
        }

        private static void TransmitPixel(bool r1, bool g1, bool b1)
        {

            Clock();
        }

        private static void TransmitPixel2(bool r1, bool g1, bool b1)
        {
            g.Write(drv.Layout.R1, r1);
            g.Write(drv.Layout.G1, g1);
            g.Write(drv.Layout.B1, b1);

            g.Write(drv.Layout.R2, r1);
            g.Write(drv.Layout.G2, g1);
            g.Write(drv.Layout.B2, b1);

            Clock();
        }

        private static void TransmitRowPixel(int pix, int color)
        {
            TransmitRowPixel(pix, (color & 1) != 0, (color & 2) != 0, (color & 4) != 0);
        }

        private static void TransmitRowPixel(int pix, bool r1, bool g1, bool b1)
        {
            for (int col = 0; col < 64; col++)
            {
                bool disp = pix == col;

                TransmitPixel(disp && r1, disp && g1, disp && b1);
            }
        }

        private static void Interleaved()
        {
            RGB[][] pattern = new RGB[4][];
            for (int row = 0; row < pattern.Length; row++)
            {
                Console.WriteLine();
                pattern[row] = new RGB[64];
                for (int col = 0; col < pattern[row].Length; col++)
                {
                    int y = (pattern.Length - row - 1) - ((col / 2) % pattern.Length);
                    if ((row + col * 2) % 5 == 0)
                    {
                        Console.Write("+");
                        pattern[row][col] = new RGB { R = false, G = true, B = false };
                    }
                    else
                    {
                        pattern[row][col] = new RGB { R = true, G = false, B = false };
                        Console.Write(".");
                    }
                }
            }
            for (int i = 0; i < pattern.Length; i++)
            {
                Console.WriteLine();
                for (int col = 0; col < pattern[i].Length; col++)
                {
                    RGB c = pattern[i][col];
                    if (c.G)
                    {
                        Console.Write("|");
                    }
                    else
                    {
                        Console.Write("_");
                    }
                    SetC1(c);
                    Clock();
                }

                g.Write(drv.Layout.A, true);
                g.Write(drv.Layout.A, false);

            }
            g.Write(drv.Layout.B, true);
            Strobe();
            g.Write(drv.Layout.B, false);
            Console.WriteLine();
        }


        private static int cSeq = 0;

        private static bool cr = true;
        private static bool cg = false;
        private static bool cb = false;

        private static void NextIColor()
        {
            switch (cSeq)
            {
                case 0: cr = true; cg = false; cb = false; break;
                case 1: cr = false; cg = true; cb = false; break;
                case 2: cr = false; cg = false; cb = true; break;
                case 3: cr = true; cg = true; cb = true; break;
                case 4: cr = false; cg = false; cb = false; break;
            }
            cSeq = (cSeq + 1) % 5;

            g.Write(drv.Layout.R1, cr);
            g.Write(drv.Layout.G1, cg);
            g.Write(drv.Layout.B1, cb);

            g.Write(drv.Layout.R2, false);
            g.Write(drv.Layout.G2, false);
            g.Write(drv.Layout.B2, false);
        }

        //private static void Interleaved2(int pix)
        //{

        //    for (int col = 0; col < 64; col++)
        //    {
        //        NextIColor();
        //        Clock();

        //        g.Write(drv.Layout.A, true);
        //        g.Write(drv.Layout.A, false);

        //    }
        //}

        private static void SetAddress(bool a, bool b)
        {
            g.Write(drv.Layout.A, a);
            g.Write(drv.Layout.B, b);
            //gpio.Write(drv.Layout.C, (value & (1 << 2)) != 0);
            //gpio.Write(drv.Layout.D, (value & (1 << 3)) != 0);
            //gpio.Write(drv.Layout.E, (value & (1 << 4)) != 0);
        }

        private static void SetAddress(int value)
        {
            bool a = (value & (1 << 0)) != 0;
            bool b = (value & (1 << 1)) != 0;
            SetAddress(a, b);
        }

        private static void Pause()
        {
            while (true)
            {
                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Enter)
                {
                    break;
                }
            }
        }

        private static void Interleaved2()
        {
            bool a = false;
            bool b = false;

            g.Write(drv.Layout.A, a);
            g.Write(drv.Layout.B, b);

            Action setWriteA = () =>
            {
                a = !a;
                g.Write(drv.Layout.A, a);
            };
            Action setWriteB = () =>
            {
                b = !b;
                g.Write(drv.Layout.B, b);
            };

            for (int scan = 0; scan < 32; scan++)
            {
                for (int col = 0; col < 64; col++)
                {
                    NextIColor();
                    Clock();
                }
                setWriteA();
                setWriteA();
                setWriteB();
            }

            Strobe();
        }

        private struct RGB
        {
            public bool R;
            public bool G;
            public bool B;

            public static readonly RGB Red = new RGB { R = true };
            public static readonly RGB Blue = new RGB { B = true };
            public static readonly RGB Green = new RGB { G = true };
        }

        private static void SetC1(RGB rgb)
        {
            g.Write(drv.Layout.R1, rgb.R);
            g.Write(drv.Layout.G1, rgb.G);
            g.Write(drv.Layout.B1, rgb.B);
        }

        private static void SetC2(RGB rgb)
        {
            g.Write(drv.Layout.R2, rgb.R);
            g.Write(drv.Layout.G2, rgb.G);
            g.Write(drv.Layout.B2, rgb.B);
        }

        private static void ChessBoardScan()
        {
            RGB[][] board = new RGB[64][];
            try
            {
                for (int row = 0; row < board.Length; row++)
                {
                    Console.ResetColor();
                    Console.WriteLine();
                    board[row] = new RGB[64];
                    for (int col = 0; col < board[row].Length; col++)
                    {
                        int a = row / 8;
                        int b = col / 8;
                        if (((a + b) % 2) == 0)
                        {
                            board[row][col] = RGB.Green;
                            Console.BackgroundColor = ConsoleColor.DarkGreen;
                        }
                        else
                        {
                            board[row][col] = RGB.Red;
                            Console.BackgroundColor = ConsoleColor.DarkRed;
                        }
                        Console.Write(" ");
                    }
                }
            }
            finally
            {
                Console.ResetColor();
            }
            Console.WriteLine();

            while (true)
            {
                Console.Write(".");
                for (int scan = 0; scan < 32; scan++)
                {
                    g.Write(drv.Layout.OE, true);
                    for (int row = 0; row < 32; row++)
                    {
                        if (row == scan)
                        {
                            for (int col = 0; col < 64; col++)
                            {
                                SetC1(board[row][col]);
                                SetC2(board[row + 32][col]);
                                Clock();
                            }
                            g.Write(drv.Layout.A, true);
                            g.Write(drv.Layout.A, false);
                        }
                        else
                        {
                            g.Write(drv.Layout.B, true);
                            g.Write(drv.Layout.A, true);
                            g.Write(drv.Layout.A, false);
                            g.Write(drv.Layout.B, false);
                        }
                    }
                    Strobe();
                    g.Write(drv.Layout.OE, false);
                    Thread.Sleep(0);
                }
            }
        }
    }
}
