using System;
using System.Collections.Generic;
using System.Text;

namespace RaspberryPi.PiGPIO.Drivers.Dede
{
    public abstract class CommonLedMatrixLayout
    {
        internal CommonLedMatrixLayout()
        {
        }

        public CommonLedMatrixLayout(int a, int b, int clock, int strobe, int outputEnabled)
        {
            this.A = a;
            this.B = b;
            this.Clock = clock;
            this.Strobe = strobe;
            this.OE = outputEnabled;
        }

        public int A { get; protected set; }
        public int B { get; protected set; }

        public int Clock { get; protected set; }
        public int Strobe { get; protected set; }
        public int OE { get; protected set; }

    }
}
