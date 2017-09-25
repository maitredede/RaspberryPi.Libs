using RaspberryPi.PiGPIO.Drivers.Dede;
using System;
using System.Collections.Generic;
using System.Text;

namespace RaspberryPi.PiGPIO.Drivers.Freetronics
{
    public sealed class DMDPinLayout : CommonLedMatrixLayout
    {
        public DMDPinLayout(int data, int a, int b, int clock, int strobe, int outputEnabled) : base(a, b, clock, strobe, outputEnabled)
        {
            this.Data = data;
        }

        public int Data { get; private set; }
    }
}
