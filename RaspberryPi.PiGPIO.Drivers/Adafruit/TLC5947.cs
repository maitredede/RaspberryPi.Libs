using System;
using System.Collections.Generic;
using System.Text;

namespace PiGPIO.Drivers.Adafruit
{
    public sealed class TLC5947 : ITLC5947
    {
        private readonly IPiGPIO m_pigpio;
        private readonly int numdrivers;
        private readonly int clock;
        private readonly int data;
        private readonly int latch;
        private readonly int[] pwmbuffer;

        public TLC5947(IPiGPIO pigpio, int numdrivers, int clock, int data, int latch)
        {
            if (pigpio == null)
                throw new ArgumentNullException(nameof(pigpio));
            if (numdrivers <= 0)
                throw new ArgumentOutOfRangeException(nameof(numdrivers));

            this.pwmbuffer = new int[2 * 24 * numdrivers];
            this.m_pigpio = pigpio;
            this.numdrivers = numdrivers;
            this.clock = clock;
            this.data = data;
            this.latch = latch;
        }

        /// <inheritDoc />
        public void Write()
        {
            this.m_pigpio.Write(this.latch, false);
            // 24 channels per TLC5974
            for (var c = 24 * numdrivers - 1; c >= 0; c--)
            {
                // 12 bits per channel, send MSB first
                for (byte b = 11; b >= 0; b--)
                {
                    this.m_pigpio.Write(this.clock, false);

                    if ((pwmbuffer[c] & (1 << b)) != 0)
                        this.m_pigpio.Write(this.data, true);
                    else
                        this.m_pigpio.Write(this.data, false);

                    this.m_pigpio.Write(this.clock, true);
                }
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
            pwmbuffer[chan] = pwm;
        }

        public void Begin()
        {
            this.m_pigpio.SetMode(this.clock, Mode.Output);
            this.m_pigpio.SetMode(this.data, Mode.Output);
            this.m_pigpio.SetMode(this.latch, Mode.Output);
        }
    }
}
