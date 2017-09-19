using RaspberryPi.PiGPIO;
using RaspberryPi.PiGPIO.Drivers;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestTLC
{
    internal sealed class TlcTestDriverBSpi : ITLC5947
    {
        private readonly IPiGPIO m_pigpio;
        private readonly int numdrivers;
        private readonly int clock;
        private readonly int data;
        private readonly int latch;
        private readonly int dummyInput;
        private readonly ushort[] pwmbuffer;

        public IPiGPIO PiGPIO => this.m_pigpio;

        public TlcTestDriverBSpi(IPiGPIO pigpio, int numdrivers, int clock, int data, int latch, int dummyInput)
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
            this.dummyInput = dummyInput;
        }

        /// <inheritDoc />
        public void Write()
        {
            byte[] buffer = new byte[this.pwmbuffer.Length * 3 / 2];
            for (int i = 0; i < 12 * numdrivers; i++)
            {
                ushort pwm0 = this.pwmbuffer[i * 2 + 0];
                ushort pwm1 = this.pwmbuffer[i * 2 + 1];

                byte b0 = (byte)((pwm0 & 0x0FF0) >> 4);
                byte b1 = (byte)((pwm0 & 0x000F) << 4 | ((pwm1 & 0x0F00) >> 8));
                byte b2 = (byte)(pwm0 & 0x00FF);

                buffer[i * 3 + 0] = b0;
                buffer[i * 3 + 1] = b1;
                buffer[i * 3 + 2] = b2;
            }
            byte[] revBuff = new byte[buffer.Length];
            for (int i = 0; i < buffer.Length; i++)
            {
                int b = buffer[i];

                int revB = 0;
                revB += (b & 0b0000_0001) << 7;
                revB += (b & 0b0000_0010) << 5;
                revB += (b & 0b0000_0100) << 3;
                revB += (b & 0b0000_1000) << 1;
                revB += (b & 0b0001_0000) >> 1;
                revB += (b & 0b0010_0000) >> 3;
                revB += (b & 0b0100_0000) >> 5;
                revB += (b & 0b1000_0000) >> 7;
                revBuff[buffer.Length - i - 1] = (byte)revB;
            }
            //                      210 9876 5432 1098 7654 3210
            //                      ... .... RT.. .... .... .pmm
            int flag = 0b0000_0000_0000_0000_0000_0000_0000_0000;
            using (var spi = this.m_pigpio.OpenBitBangSpi(this.latch, this.dummyInput, this.data, this.clock, 250000, flag))
            {
                spi.Write(revBuff);
            }
            //this.m_pigpio.Write(this.latch, true);
            //this.m_pigpio.Write(this.latch, false);
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
