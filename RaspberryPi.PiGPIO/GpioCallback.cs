using System;
using System.Collections.Generic;
using System.Text;

namespace RaspberryPi.PiGPIO
{
    public delegate void GpioCallback(int port, Edge egde, long tick);
}
