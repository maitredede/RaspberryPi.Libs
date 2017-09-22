using System;
using System.Collections.Generic;
using System.Text;

namespace RaspberryPi.PiGPIO.Drivers.Dede
{
    public sealed class FreetronicsDMDPinLayout : CommonLedMatrixLayout
    {
        public FreetronicsDMDPinLayout(int data, int a, int b, int clock, int strobe, int outputEnabled) : base(a, b, clock, strobe, outputEnabled)
        {
            this.Data = data;
        }

        public int Data { get; private set; }
    }
}
