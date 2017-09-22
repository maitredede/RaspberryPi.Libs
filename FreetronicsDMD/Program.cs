using RaspberryPi.PiGPIO;
using RaspberryPi.PiGPIO.Drivers;
using RaspberryPi.PiGPIO.Drivers.Dede;
using System;
using System.Threading;

namespace Dede.DMD
{
    public static class Program
    {
        private static readonly int gpioData = 19;
        private static readonly int gpioA = 12;
        private static readonly int gpioB = 16;
        private static readonly int gpioClock = 26;
        private static readonly int gpioStrobe = 6;
        private static readonly int gpioOE = 13;

        private static IPiGPIO gpio;

        static void Main(string[] args)
        {
            using (gpio = new PiGpio())
            //using (var pigpio = new PigsClient("192.168.20.22", 8888))
            {
                //    pigpio.ConnectAsync().Wait();
                //gpio = pigpio;

                FreetronicsDMDPinLayout layout = new FreetronicsDMDPinLayout(gpioData, gpioA, gpioB, gpioClock, gpioStrobe, gpioOE);
                FreetronicsDMD dmd = new FreetronicsDMD(gpio, layout, 1, 1);
                dmd.Init();

                FreetronicsDMDDrawer.Test(dmd);
                while (true)
                {
                    dmd.ScanFull();
                    Console.Write(".");
                }

                //while (true)
                //{
                //    for (int i = 1; i < 15; i++)
                //    {
                //        dmd.SetPixel(i, i, true);
                //    }
                //    for (int i = 0; i < 4; i++)
                //    {
                //        Console.Write(i);
                //        dmd.Scan();
                //        //Thread.Sleep(500);
                //    }
                //    dmd.Clear();
                //    for (int i = 0; i < 4; i++)
                //    {
                //        Console.Write(i);
                //        dmd.Scan();
                //        //Thread.Sleep(500);
                //    }
                //}
            }
        }
    }
}