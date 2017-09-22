using System;
using System.Collections.Generic;
using System.Text;

namespace RaspberryPi.PiGPIO.Drivers
{
    public class GpioAlreadyUsedException : Exception
    {
        public int Pin { get; }

        internal GpioAlreadyUsedException(int gpio) : this(gpio, $"GPIO {gpio} already used")
        {

        }

        internal GpioAlreadyUsedException(int gpio, string message) : base(message)
        {
            this.Pin = gpio;
        }
    }
}
