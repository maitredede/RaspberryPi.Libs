using System;
using System.Collections.Generic;
using System.Text;

namespace RaspberryPi.PiGPIO.Drivers
{
    public interface ITLC5947 : IDriver
    {
        /// <summary>
        /// Set PWM value for a channel
        /// </summary>
        /// <param name="chan">Channel</param>
        /// <param name="pwm">value (0-4096)</param>
        void SetPWM(int chan, int pwm);

        /// <summary>
        /// Send buffer data to component
        /// </summary>
        void Write();
    }
}
