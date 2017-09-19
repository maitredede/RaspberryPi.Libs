using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Text;

namespace RaspberryPi.PiGPIO.Drivers
{
    public static class ColorConversionExtensions
    {
        /// <summary>
        /// Convert ConsoleColor to Color
        /// </summary>
        /// <param name="consoleColor"></param>
        /// <returns></returns>
        public static Color ToColor(this ConsoleColor consoleColor)
        {
            Color c = Color.FromName(consoleColor.ToString());
            if (c.IsNamedColor)
                return c;
            throw new ArgumentException("Unknown color: " + consoleColor.ToString(), paramName: nameof(consoleColor));
        }
    }
}
