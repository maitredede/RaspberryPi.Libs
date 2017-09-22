using System;
using System.Linq;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using ImageSharp;
using ImageSharp.Drawing;
using ImageSharp.Drawing.Pens;
using ImageSharp.Drawing.Brushes;
using ImageSharp.PixelFormats;
using SixLabors.Primitives;
using SixLabors.Fonts;
using SixLabors;
using RaspberryPi.PiGPIO.Drivers.Dede;

namespace Dede.DMD
{
    [System.Diagnostics.DebuggerDisplay("[{Value}]")]
    public struct DmdPixelFormat : IPixel<DmdPixelFormat>
    {
        public bool Value { get; set; }
        public DmdPixelFormat(bool value)
        {
            this.Value = value;
        }

        public PixelOperations<DmdPixelFormat> CreatePixelOperations()
        {
            return new PixelOperations<DmdPixelFormat>();
        }

        public bool Equals(DmdPixelFormat other)
        {
            return this.Value == other.Value;
        }

        public void PackFromRgba32(Rgba32 source)
        {
            throw new NotImplementedException();
        }

        public void PackFromVector4(Vector4 vector)
        {
            this.Value = (((vector.X + vector.Y + vector.Z + vector.W) / 4) > 0.5);
        }

        public void ToBgr24(ref Bgr24 dest)
        {
            throw new NotImplementedException();
        }

        public void ToBgra32(ref Bgra32 dest)
        {
            throw new NotImplementedException();
        }

        public void ToRgb24(ref Rgb24 dest)
        {
            throw new NotImplementedException();
        }

        public void ToRgba32(ref Rgba32 dest)
        {
            dest = this.Value ? Rgba32.White : Rgba32.Black;
        }

        public Vector4 ToVector4()
        {
            return this.Value ? Vector4.One : Vector4.Zero;
        }

        public static readonly DmdPixelFormat On = new DmdPixelFormat(true);
        public static readonly DmdPixelFormat Off = new DmdPixelFormat(false);
    }

    public static class FreetronicsDMDDrawer
    {
        public static void Test(FreetronicsDMD dmd)
        {
            //sudo apt-get install fonts-font-awesome fonts-dejavu ttf-mscorefonts-installer
            foreach (var ff in SystemFonts.Families)
            {
                Console.WriteLine("Font: {0}", ff.Name);
            }
            SystemFonts.TryFind("Arial", out FontFamily arial);
            Font font = new Font(arial, 10);

            using (Image<DmdPixelFormat> img = new Image<DmdPixelFormat>(32, 16))
            {
                PointF[] pts =
                {
                    new PointF(0,0),
                    new PointF(10,10),
                    new PointF(20,0)
                };
                img.DrawLines(DmdPixelFormat.On, 1, pts);
                img.DrawText("Boo", font, DmdPixelFormat.On, new PointF(20, 0));

                Span<DmdPixelFormat> pixels = img.Pixels;
                DmdPixelFormat[] pixelsArray = pixels.ToArray();
                for (int i = 0; i < pixelsArray.Length; i++)
                {
                    int y = Math.DivRem(i, 32, out int x);
                    dmd.SetPixel(x, y, pixelsArray[i].Value);
                }
            }

        }
    }
}
