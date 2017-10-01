using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace RaspberryPi.PiGPIO.Drivers
{
    public static class ImageSharpColorConversionExtensions
    {
        public static Color ToColor(this Rgba32 color)
        {
            return Color.FromArgb(color.A, color.R, color.G, color.B);
        }

        public static Rgba32 ToRgba32(this Color color)
        {
            return new Rgba32(color.R, color.G, color.B, color.A);
        }
    }
}
