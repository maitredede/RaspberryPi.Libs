//using RaspberryPi.PiGPIO;
//using RaspberryPi.PiGPIO.Drivers;
//using RaspberryPi.PiGPIO.Drivers.Dede;
//using RaspberryPi.PiGPIO.Drivers.Freetronics;
//using System;
//using System.Threading;
//using SixLabors.Fonts;
//using ImageSharp;
//using System.Linq;
//using RaspberryPi.PiGPIO.Drivers.Dede.PixelFormats;
//using SixLabors.Primitives;

//namespace Dede.DMDTest
//{
//    public static class Program
//    {
//        private static readonly int gpioData = 19;
//        private static readonly int gpioA = 12;
//        private static readonly int gpioB = 16;
//        private static readonly int gpioClock = 26;
//        private static readonly int gpioStrobe = 6;
//        private static readonly int gpioOE = 13;

//        private static IPiGPIO gpio;

//        static void Main(string[] args)
//        {
//            //using (var pigpio = new PigsClient("192.168.20.22", 8888))
//            //{
//            //    pigpio.ConnectAsync().Wait();
//            //    gpio = pigpio;
//            using (gpio = new PiGpio())
//            {

//                DMDPinLayout layout = new DMDPinLayout(gpioData, gpioA, gpioB, gpioClock, gpioStrobe, gpioOE);
//                string str = "";
//                FontFamily fontFamily;
//                if (!SystemFonts.TryFind("Arial", out fontFamily))
//                {
//                    Console.WriteLine("Arial not found, taking first");
//                    fontFamily = SystemFonts.Families.FirstOrDefault();
//                }
//                Font font = new Font(fontFamily, 10);

//                try
//                {
//                    using (FreetronicsDMDSurface dmd2 = new FreetronicsDMDSurface(gpio, layout))
//                    {
//                        dmd2.Init(false);

//                        while (true)
//                        {
//                            string str2 = DateTime.Now.ToLongTimeString();
//                            if (str != str2)
//                            {
//                                str = str2;
//                                lock (dmd2.Surface)
//                                {
//                                    dmd2.Clear();
//                                    dmd2.Surface.DrawText(str, font, BitPixel.On, new PointF(0, 0));
//                                }
//                                Console.Title = str;
//                            }
//                            ((IDMDInternals)dmd2).ScanFull();
//                        }
//                    }
//                }
//                catch (Exception ex)
//                {
//                    Console.Error.WriteLine(ex.GetType().FullName + ": " + ex.Message);
//                    Console.Error.WriteLine(ex.StackTrace);
//                }
//            }
//        }
//    }
//}


using System;
using ImageSharp;
using SixLabors.Fonts;
using SixLabors.Primitives;
using System.Threading;

public static class Program
{
    private static Image<Rgba32> img;

    public static void Main(string[] args)
    {
        string dt = "";
        FontFamily arialFamily = SystemFonts.Find("Arial");
        Font font = new Font(arialFamily, 10);
        Thread th = new Thread(ThreadMethod);
        using (img = new Image<Rgba32>(100, 100))
        {
            th.Start();
            while (true)
            {
                string dt2 = DateTime.Now.ToLongTimeString();
                if (dt != dt2)
                {
                    dt = dt2;
                    img.Fill(Rgba32.Black);
                    img.DrawText(dt, font, Rgba32.Blue, PointF.Empty);
                }
            }
        }
    }

    private static void ThreadMethod()
    {
        while (true)
        {
            for (int i = 0; i < img.Height; i++)
            {
                var rowData = img.GetRowSpan(i).ToArray();
                //Simulate working on data
                Thread.Sleep(1);
                for (int j = 0; j < rowData.Length; j++)
                {
                    rowData[j].ToString();
                }
            }
        }
    }
}