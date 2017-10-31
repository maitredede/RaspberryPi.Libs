using System;
using System.Collections.Generic;
using System.Text;

namespace RaspberryPi.PiGPIO.Drivers.Adafruit
{
    public sealed class TLC5947 : ITLC5947
    {
        private readonly IPiGPIO m_pigpio;
        private readonly int numdrivers;
        private readonly int clock;
        private readonly int data;
        private readonly int latch;
        private readonly int oe;
        private readonly int[] pwmbuffer;
        private bool oeValue;

        public IPiGPIO PiGPIO => this.m_pigpio;

        public TLC5947(IPiGPIO pigpio, int numdrivers, int clock, int data, int latch, int oe = int.MinValue)
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
            this.oe = oe;
        }

        public bool OutputEnabled => this.oeValue;

        public void SetOutputEnabled(bool value)
        {
            this.oeValue = value;
            if (this.oe != int.MinValue)
            {
                this.m_pigpio.Write(this.oe, !this.oeValue);
            }
        }

        /// <inheritDoc />
        public void Write()
        {
            this.m_pigpio.Write(this.latch, false);
            // 24 channels per TLC5974
            for (int c = 24 * numdrivers - 1; c >= 0; c--)
            {
                // 12 bits per channel, send MSB first
                for (int b = 11; b >= 0; b--)
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

        void IDriver.Init()
        {
            this.Begin();
        }

        public void Begin()
        {
            this.m_pigpio.SetMode(this.clock, Mode.Output);
            this.m_pigpio.SetMode(this.data, Mode.Output);
            this.m_pigpio.SetMode(this.latch, Mode.Output);
            if (this.oe != int.MinValue)
            {
                this.m_pigpio.SetMode(this.oe, Mode.Output);
                this.m_pigpio.Write(this.oe, !this.oeValue);
            }
        }
    }
}
