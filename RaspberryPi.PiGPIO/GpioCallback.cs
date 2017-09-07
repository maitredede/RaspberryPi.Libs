using System;
using System.Collections.Generic;
using System.Text;

namespace PiGPIO
{
    public delegate void GpioCallback(int port, Edge egde, long tick);
}
