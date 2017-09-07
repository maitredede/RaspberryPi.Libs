using System;
using System.Drawing;

namespace PiGPIO.Drivers
{
    public static class PWMChannelDriverExtensions
    {
        /// <summary>
        /// Set PWM for a RGB LED
        /// </summary>
        /// <param name="comp">The component</param>
        /// <param name="ledNum">LED number</param>
        /// <param name="r">R value</param>
        /// <param name="g">G value</param>
        /// <param name="b">B value</param>
        public static void SetLED(this IPWMChannelDriver comp, int ledNum, int r, int g, int b)
        {
            comp.SetPWM(ledNum * 3, r);
            comp.SetPWM(ledNum * 3 + 1, g);
            comp.SetPWM(ledNum * 3 + 2, b);
        }

        public static void SetLED(this IPWMChannelDriver comp, int ledNum, Color color)
        {
            comp.SetLED(ledNum, color, 4);
        }

        public static void SetLED(this IPWMChannelDriver comp, int ledNum, Color color, int bitshift)
        {
            comp.SetLED(ledNum, color.R << bitshift, color.G << bitshift, color.B << bitshift);
        }
    }
}
