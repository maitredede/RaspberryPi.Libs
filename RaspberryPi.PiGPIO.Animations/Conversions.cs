using System;
using System.Collections.Generic;
using System.Text;

namespace RaspberryPi.PiGPIO.Animations
{
    public static class Conversions
    {
        public static void hsv2rgb(double h, double s, double v, out double r, out double g, out double b)
        {
            int i = (int)(h * 6);
            double f = h * 6 - i;
            double p = v * (1 - s);
            double q = v * (1 - f * s);
            double t = v * (1 - (1 - f) * s);

            switch (i % 6)
            {
                case 0: r = v; g = t; b = p; break;
                case 1: r = q; g = v; b = p; break;
                case 2: r = p; g = v; b = t; break;
                case 3: r = p; g = q; b = v; break;
                case 4: r = t; g = p; b = v; break;
                case 5: r = v; g = p; b = q; break;
                default: r = 0; g = 0; b = 0; break;
            }
        }

        public static void hsv2rgb(double h, double s, double v, out byte r, out byte g, out byte b)
        {
            hsv2rgb(h, s, v, out double dr, out double dg, out double db);
            r = (byte)(dr * byte.MaxValue);
            g = (byte)(dg * byte.MaxValue);
            b = (byte)(db * byte.MaxValue);
        }

        public static void hsv2rgb(double h, double s, double v, out int r, out int g, out int b, int range)
        {
            hsv2rgb(h, s, v, out double dr, out double dg, out double db);
            r = (int)(dr * range);
            g = (int)(dg * range);
            b = (int)(db * range);
        }
    }
}
