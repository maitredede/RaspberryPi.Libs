using System;
using System.Collections.Generic;
using System.Text;

namespace RaspberryPi.PiGPIO.Drivers.Dede
{
    public sealed class Hub75PinLayout
    {
        internal Hub75PinLayout()
        {
        }

        public string Name { get; private set; }

        public int R1 { get; private set; }
        public int G1 { get; private set; }
        public int B1 { get; private set; }

        public int R2 { get; private set; }
        public int G2 { get; private set; }
        public int B2 { get; private set; }

        public int A { get; private set; }
        public int B { get; private set; }
        public int C { get; private set; }
        public int D { get; private set; }
        public int E { get; private set; }

        public int Clock { get; private set; }
        public int Strobe { get; private set; }
        public int OE { get; private set; }

        private static Hub75PinLayout _regular;
        public static Hub75PinLayout Regular
        {
            get
            {
                if (_regular == null)
                {
                    _regular = new Hub75PinLayout
                    {
                        Name = "Regular",
                        OE = 18,
                        Clock = 17,
                        Strobe = 4,
                        A = 22,
                        B = 23,
                        C = 24,
                        D = 25,
                        E = 15,
                        R1 = 11,
                        G1 = 27,
                        B1 = 7,
                        R2 = 8,
                        G2 = 9,
                        B2 = 10,
                    };
                }
                return _regular;
            }
        }
    }
}
