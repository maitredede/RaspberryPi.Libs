using System;
using System.Threading;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace RaspberryPi.PiGPIO.Drivers.Dede
{
    [Obsolete("Not tested")]
    public sealed class TLC5947Thread : ITLC5947, IDisposable
    {
        private readonly IPiGPIO m_gpio;
        private readonly int m_num;
        private readonly int m_spi;
        private readonly int m_cs;
        private readonly int m_latch;
        private readonly int m_oeGpio;
        private bool m_oeValue;

        private readonly ReaderWriterLockSlim m_rwlock;
        private readonly short[] m_buffer;

        private readonly short[] m_writeBuffer;
        private bool m_dirty;

        public TLC5947Thread(IPiGPIO gpio, int num, int spi, int cs, int latchGpio, int outputEnabledGpio = int.MinValue)
        {
            this.m_gpio = gpio ?? throw new ArgumentNullException(nameof(gpio));
            this.m_num = num;
            this.m_spi = spi;
            this.m_cs = cs;
            this.m_latch = latchGpio;
            this.m_oeGpio = outputEnabledGpio;

            this.m_rwlock = new ReaderWriterLockSlim();
            this.m_buffer = new short[24 * num];
            this.m_writeBuffer = new short[24 * num];
            this.m_dirty = true;
        }

        public void Begin()
        {
            this.m_gpio.SetMode(this.m_latch, Mode.Output);
            if (this.m_oeGpio != int.MinValue)
            {
                this.m_gpio.SetMode(this.m_oeGpio, Mode.Output);
                this.SetOutputEnabled(false);
            }
        }

        public void SetOutputEnabled(bool outputEnabled)
        {
            if (this.m_oeGpio == int.MinValue)
                throw new NotSupportedException();
            this.m_oeValue = outputEnabled;
            this.m_gpio.Write(this.m_oeGpio, !this.m_oeValue);
        }

        public void Dispose()
        {
            this.m_rwlock.Dispose();
        }

        public void SetPWM(int chan, int pwm)
        {
            if (pwm < 0 || pwm > 4095)
                throw new ArgumentOutOfRangeException(nameof(pwm));
            this.m_rwlock.EnterWriteLock();
            try
            {
                this.m_buffer[chan] = (short)pwm;
                this.m_dirty = true;
            }
            finally
            {
                this.m_rwlock.ExitWriteLock();
            }
        }

        public void Write()
        {
            this.m_rwlock.EnterUpgradeableReadLock();
            try
            {
                if (!this.m_dirty)
                    return;
                this.m_rwlock.EnterWriteLock();
                try
                {
                    if (!this.m_dirty)
                        return;

                    lock (this.m_writeBuffer)
                    {
                        Array.ConstrainedCopy(this.m_buffer, 0, this.m_writeBuffer, 0, this.m_buffer.Length);
                        this.m_dirty = false;
                        ThreadPool.QueueUserWorkItem(this.WriteAsync);
                    }
                }
                finally
                {
                    this.m_rwlock.ExitWriteLock();
                }
            }
            finally
            {
                this.m_rwlock.ExitUpgradeableReadLock();
            }
        }

        private void WriteAsync(object state)
        {
            lock (this.m_writeBuffer)
            {
                int shortLen = sizeof(short);
                int byteLen = shortLen * this.m_writeBuffer.Length;
                byte[] data = new byte[byteLen];
                //for (int i = 0; i < this.m_writeBuffer.Length; i++)
                //{
                //    byte[] item = BitConverter.GetBytes(this.m_writeBuffer[i]);
                //    Array.Copy(item, 0, data, i * shortLen, shortLen);
                //}
                GCHandle h = GCHandle.Alloc(this.m_writeBuffer, GCHandleType.Pinned);
                try
                {
                    Marshal.Copy(h.AddrOfPinnedObject(), data, 0, byteLen);
                }
                finally
                {
                    h.Free();
                }
                using (var spi = this.m_gpio.OpenSpi(this.m_spi, 10_000_000, 0))
                {
                    spi.Write(data);
                }
            }
        }
    }
}
