using System;
using System.Collections.Generic;
using System.Text;

namespace RaspberryPi.PiGPIO.Drivers
{
    public interface IPWMChannelDriver
    {
        /// <summary>
        /// Set PWM value for a channel
        /// </summary>
        /// <param name="chan">Channel</param>
        /// <param name="pwm">value</param>
        void SetPWM(int chan, int pwm);

        /// <summary>
        /// Send buffer data to component
        /// </summary>
        void Write();
    }
}
