//using System;
//using System.Linq;
//using System.Collections.Generic;
//using System.Numerics;
//using System.Text;
//using SixLabors.ImageSharp;
//using SixLabors.Primitives;
//using SixLabors.Fonts;
//using SixLabors;
//using RaspberryPi.PiGPIO.Drivers.Freetronics;
//using RaspberryPi.PiGPIO.Drivers.Dede;
//using RaspberryPi.PiGPIO.Drivers.Dede.PixelFormats;
//using System.IO;

//namespace Dede.DMDTest
//{
//    public static class FreetronicsDMDDrawer
//    {
//        public static void Test(DMD dmd)
//        {
//            //sudo apt-get install fonts-font-awesome fonts-dejavu ttf-mscorefonts-installer
//            foreach (var ff in SystemFonts.Families)
//            {
//                Console.WriteLine("Font: {0}", ff.Name);
//            }
//            SystemFonts.TryFind("Arial", out FontFamily arial);
//            Font font = new Font(arial, 10);

//            using (Image<BitPixel> img = new Image<BitPixel>(32, 16))
//            {
//                PointF[] pts =
//                {
//                    new PointF(0,0),
//                    new PointF(10,10),
//                    new PointF(20,0)
//                };
//                img.DrawLines(BitPixel.On, 1, pts);
//                img.DrawText("Boo", font, BitPixel.On, new PointF(20, 0));

//                Span<BitPixel> pixels = img.Pixels;
//                BitPixel[] pixelsArray = pixels.ToArray();
//                for (int i = 0; i < pixelsArray.Length; i++)
//                {
//                    int y = Math.DivRem(i, 32, out int x);
//                    dmd.SetPixel(x, y, pixelsArray[i].Value);
//                }
//            }
//        }

//        private static void DrawTest(Image<BitPixel> image)
//        {
//            //sudo apt-get install fonts-font-awesome fonts-dejavu ttf-mscorefonts-installer
//            foreach (var ff in SystemFonts.Families)
//            {
//                Console.WriteLine("Font: {0}", ff.Name);
//            }
//            SystemFonts.TryFind("Arial", out FontFamily arial);
//            Font font = new Font(arial, 10);

//            PointF[] pts =
//            {
//                    new PointF(0,0),
//                    new PointF(10,10),
//                    new PointF(20,0)
//                };
//            image.DrawLines(BitPixel.On, 1, pts);
//            image.DrawText("Boo", font, BitPixel.On, new PointF(20, 0));
//        }

//        public static void Test(FreetronicsDMDSurface dmd)
//        {
//            DrawTest(dmd.Surface);
//            //string tmp = Path.GetTempFileName();
//            //using (var fs = File.Create(tmp))
//            //{
//            //    dmd.Surface.SaveAsPng(fs);
//            //}
//            //Console.WriteLine("Saved png: " + tmp);
//        }

//    }
//}
