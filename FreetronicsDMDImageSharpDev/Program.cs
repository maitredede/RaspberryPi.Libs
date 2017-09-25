//using RaspberryPi.PiGPIO;
//using RaspberryPi.PiGPIO.Drivers.Dede;
//using RaspberryPi.PiGPIO.Drivers.Freetronics;
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

//namespace FreetronicsDMDImageSharpDev
//{
//    class Program
//    {
//        static void Main(string[] args)
//        {
//            const int panelsWidth = 1;
//            const int panelsHeight = 1;

//            DMDPinLayout layout = new DMDPinLayout(data: 1, a: 2, b: 3, clock: 4, strobe: 5, outputEnabled: 6);

//            PigsGenerator gpioRef = new PigsGenerator();
//            DMD dmdRef = new DMD(gpioRef, layout, panelsWidth, panelsHeight);
//            dmdRef.Init();
//            PigsGenerator gpioTest = new PigsGenerator();
//            FreetronicsDMDSurface dmdTest = new FreetronicsDMDSurface(gpioTest, layout, panelsWidth, panelsHeight);
//            dmdTest.Init(false);

//            if (gpioRef.ToString() != gpioTest.ToString())
//            {
//                Console.WriteLine("Differences in init");
//                return;
//            }
//            gpioRef.Clear();
//            gpioTest.Clear();
//            DrawPattern(dmdRef);
//            DrawPattern(dmdTest);

//            if (gpioRef.ToString() != gpioTest.ToString())
//            {
//                Console.WriteLine("Differences after drawPattern");
//                return;
//            }
//            TestScan(dmdRef, dmdTest, 0);
//            TestScan(dmdRef, dmdTest, 1);
//            TestScan(dmdRef, dmdTest, 2);
//            TestScan(dmdRef, dmdTest, 3);
//        }

//        private static void TestScan(IDMDInternals dmdRef, IDMDInternals dmdTest, int scan)
//        {
//            ((PigsGenerator)dmdRef.Gpio).Clear();
//            ((PigsGenerator)dmdTest.Gpio).Clear();

//            dmdRef.Scan(scan);
//            dmdTest.Scan(scan);

//            string sRef = dmdRef.Gpio.ToString();
//            string sTest = dmdTest.Gpio.ToString();
//            if (sRef != sTest)
//            {
//                Console.WriteLine($"Differences in scan {scan}");

//                int len = Math.Min(sRef.Length, sTest.Length);
//                if (sRef.Length != sTest.Length)
//                {
//                    Console.WriteLine($"Different sizes ref={sRef.Length} test={sTest.Length}");
//                }
//                for (int i = 0; i < len; i++)
//                {
//                    if (sRef[i] != sTest[i])
//                    {
//                        Console.WriteLine($"First change at index {i}");
//                        Console.WriteLine($"ref: {sRef.Substring(i, 10)}");
//                        Console.WriteLine($"tst: {sTest.Substring(i, 10)}");
//                        break;
//                    }
//                }
//            }
//        }

//        private static void DrawPattern(IDMDInternals dmd)
//        {
//            int panelsWidth = dmd.PanelsWide;
//            int panelsHeight = dmd.PanelsTall;

//            for (int i = 0; i < Math.Max(panelsWidth * 32, panelsHeight * 16); i++)
//            {
//                ((IDMDInternals)dmd).SetPixel(i, i, true);
//            }
//        }
//    }
//}
