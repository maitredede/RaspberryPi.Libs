using System;
using System.Collections.Generic;
using System.Text;

namespace RaspberryPi.PiGPIO
{
    internal delegate void gpioAlertFunc_t(int gpio, int level, uint tick);
}
