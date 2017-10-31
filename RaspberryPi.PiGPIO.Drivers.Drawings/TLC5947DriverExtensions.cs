using System;
using System.Drawing;

namespace RaspberryPi.PiGPIO.Drivers
{
    public static class TLC5947DriverExtensions
    {
        /// <summary>
        /// Set PWM for a RGB LED
        /// </summary>
        /// <param name="comp">The component</param>
        /// <param name="ledNum">LED number</param>
        /// <param name="r">R value</param>
        /// <param name="g">G value</param>
        /// <param name="b">B value</param>
        public static void SetLED(this ITLC5947 comp, int ledNum, byte r, byte g, byte b)
        {
            comp.SetPWM(ledNum * 3, (int)(r * colorRatio));
            comp.SetPWM(ledNum * 3 + 1, (int)(g * colorRatio));
            comp.SetPWM(ledNum * 3 + 2, (int)(b * colorRatio));
        }

        private const double colorRatio = 4095.0 / 255.0;
        /// <summary>
        /// Set RGB led color (for a 12 bits PWM)
        /// </summary>
        /// <param name="comp">The component</param>
        /// <param name="ledNum">LED number</param>
        /// <param name="color"></param>
        public static void SetLED(this ITLC5947 comp, int ledNum, Color color)
        {
            comp.SetLED(ledNum, color.R, color.G, color.B);
        }
    }
}
