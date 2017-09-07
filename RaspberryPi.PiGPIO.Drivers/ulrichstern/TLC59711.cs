using System;
using System.Collections.Generic;
using System.Text;

namespace PiGPIO.Drivers.ulrichstern
{
    public class TLC59711 : ITLC59711
    {
        void IPWMChannelDriver.SetPWM(int chan, int pwm)
        {
            throw new NotImplementedException();
        }

        void IPWMChannelDriver.Write()
        {
            throw new NotImplementedException();
        }
    }
}
