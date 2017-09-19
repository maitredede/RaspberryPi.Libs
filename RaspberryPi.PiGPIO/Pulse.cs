using System;
using System.Collections.Generic;
using System.Text;

namespace RaspberryPi.PiGPIO
{
    public struct Pulse
    {
        public int gpioOn;
        public int gpioOff;
        public long usDelay;

        public Pulse(int gpioOn, int gpioOff, long usDelay) : this()
        {
            this.gpioOn = gpioOn;
            this.gpioOff = gpioOff;
            this.usDelay = usDelay;
        }
    }
}
