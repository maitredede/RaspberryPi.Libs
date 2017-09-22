using RaspberryPi.PiGPIO;
using RaspberryPi.PiGPIO.Drivers.Dede;
using System;
using System.Drawing;
using System.Threading;

namespace CoremanDriverPoc
{
    class Program
    {
        private static IPiGPIO gpio;
        private static CoremanHub75LedMatrix drv;

        static void Main(string[] args)
        {
            Console.WriteLine("Starting application...");
            //using (PigsClient pigs = new PigsClient("192.168.20.22", 8888))
            using (PiGpio pigs = new PiGpio())
            {
                //pigs.ConnectAsync().Wait();
                gpio = pigs;

                Console.WriteLine("Starting driver...");
                drv = new CoremanHub75LedMatrix(pigs, Hub75PinLayout.Regular);
                //Set all to 0
                drv.Init();

                int color = 0;
                Action NextColor = () =>
                {
                    color = color << 1;
                    if ((color & 0b0000_0111) == 0)
                    {
                        color = 1;
                    }
                };
                int pos = 0;
                Action NextPos = () =>
                {
                    pos = (pos + 1) % 64;
                };
                int addr = 0;
                Action NextAddress = () =>
                {
                    addr = (addr + 1) % 4;
                };

                Action<int> SerialSetAddress = (int address) =>
                {
                    // 32 = 0b0100_0000;

                    int maskedAddress = address & 31;
                    for (int i = 0; i < 3; i++)
                    {
                        int addressPart = (address & (0b0000_0011 << (i * 2))) >> (i * 2);

                        bool a = (addressPart & 0b10) != 0;
                        bool b = (addressPart & 0b10) != 0;
                        SetAddress(a, b);
                        StrobeNC();
                    }
                };

                Console.WriteLine("Looping...");
                while (true)
                {
                    for (int i = 0; i < 32; i++)
                    {
                        TransmitRowPixel(pos, color);
                        SerialSetAddress(i);
                        Strobe();

                        Console.Write("+");
                        Pause();

                        NextColor();
                        NextPos();
                    }
                    Console.WriteLine($"pos:{pos}");
                    Pause();
                }
            }
        }

        private static void Clock()
        {
            gpio.Write(drv.Layout.Clock, true);
            gpio.Write(drv.Layout.Clock, false);
        }

        private static void Strobe()
        {
            StrobeUp();
            StrobeDown();
        }

        private static void StrobeUp()
        {
            gpio.Write(drv.Layout.Strobe, true);
        }

        private static void StrobeDown()
        {
            gpio.Write(drv.Layout.Strobe, true);
        }

        private static void StrobeNC()
        {
            gpio.Write(drv.Layout.C, true);
            gpio.Write(drv.Layout.C, false);
        }

        private static void TransmitPixel(bool r1, bool g1, bool b1)
        {
            gpio.Write(drv.Layout.R1, r1);
            gpio.Write(drv.Layout.G1, g1);
            gpio.Write(drv.Layout.B1, b1);

            gpio.Write(drv.Layout.R2, false);
            gpio.Write(drv.Layout.G2, false);
            gpio.Write(drv.Layout.B2, false);

            Clock();
        }

        private static void TransmitPixel2(bool r1, bool g1, bool b1)
        {
            gpio.Write(drv.Layout.R1, r1);
            gpio.Write(drv.Layout.G1, g1);
            gpio.Write(drv.Layout.B1, b1);

            gpio.Write(drv.Layout.R2, r1);
            gpio.Write(drv.Layout.G2, g1);
            gpio.Write(drv.Layout.B2, b1);

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

        private static void SetAddress(bool a, bool b)
        {
            gpio.Write(drv.Layout.A, a);
            gpio.Write(drv.Layout.B, b);
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
    }
}
