using RaspberryPi.PiGPIO;
using RaspberryPi.PiGPIO.Drivers;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestTLC
{
    class TlcTestDriverDirect : ITLC5947
    {
        private readonly IPiGPIO m_pigpio;
        private readonly int numdrivers;
        private readonly int clock;
        private readonly int data;
        private readonly int latch;
        private readonly ushort[] pwmbuffer;

        public IPiGPIO PiGPIO => this.m_pigpio;

        public TlcTestDriverDirect(IPiGPIO pigpio, int numdrivers, int clock, int data, int latch)
        {
            if (pigpio == null)
                throw new ArgumentNullException(nameof(pigpio));
            if (numdrivers <= 0)
                throw new ArgumentOutOfRangeException(nameof(numdrivers));

            this.pwmbuffer = new ushort[24 * numdrivers];
            this.m_pigpio = pigpio;
            this.numdrivers = numdrivers;
            this.clock = clock;
            this.data = data;
            this.latch = latch;
        }

        /// <inheritDoc />
        public void Write()
        {
            this.Write(false);
        }

        public void Write(bool dump)
        {
            if (dump)
            {
                for (int i = 0; i < this.pwmbuffer.Length; i += 2)
                {
                    Console.Write("{0:X2}{1:X2} ", this.pwmbuffer[i], this.pwmbuffer[i + 1]);
                }
                Console.WriteLine();
            }
            this.m_pigpio.Write(this.latch, false);
            // 24 channels per TLC5974
            for (int c = 24 * numdrivers - 1; c >= 0; c--)
            {
                int val = this.pwmbuffer[c];
                if (dump && c == 3)
                    Console.Write("3=>{0}\t", val);
                // 12 bits per channel, send MSB first
                for (int b = 11; b >= 0; b--)
                {
                    this.m_pigpio.Write(this.clock, false);

                    bool bit = (val & (1 << b)) != 0;
                    if (dump && c == 3)
                        Console.Write(bit ? "1" : "0");
                    this.m_pigpio.Write(this.data, bit);
                    this.m_pigpio.Write(this.clock, true);
                }
                if (dump && c == 3)
                    Console.WriteLine();
            }
            this.m_pigpio.Write(this.clock, false);

            this.m_pigpio.Write(this.latch, true);
            this.m_pigpio.Write(this.latch, false);
        }

        /// <inheritDoc />
        public void SetPWM(int chan, int pwm)
        {
            if (pwm < 0)
                pwm = 0;
            if (pwm > 4095)
                pwm = 4095;
            pwmbuffer[chan] = (ushort)pwm;
        }

        public void Begin()
        {
            this.m_pigpio.SetMode(this.clock, Mode.Output);
            this.m_pigpio.SetMode(this.data, Mode.Output);
            this.m_pigpio.SetMode(this.latch, Mode.Output);
        }
    }
}
