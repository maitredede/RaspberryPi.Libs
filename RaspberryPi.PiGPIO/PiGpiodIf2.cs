using RaspberryPi.PiGPIO.Interop;
using System;
using System.Collections.Generic;
using System.Text;

namespace RaspberryPi.PiGPIO
{
    public sealed class PiGpiodIf2 : IPiGPIO, IDisposable
    {
        private short m_handle = short.MinValue;

        public PiGpiodIf2() : this(null, null)
        {
        }

        public PiGpiodIf2(string address, int port) : this(address, port.ToString())
        {
        }

        private PiGpiodIf2(string address, string port)
        {
            short ret = PiGpiodIf2NativeMethods.Start(address, port);
            if (ret < 0)
                throw new PiGPIOException(ret);
            this.m_handle = ret;
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.m_handle != short.MinValue)
                {
                    PiGpiodIf2NativeMethods.Stop(this.m_handle);
                    this.m_handle = short.MinValue;
                }
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~PiGpiodIf2()
        {
            this.Dispose(false);
        }

        public int HardwareRevision()
        {
            return unchecked((int)PiGpiodIf2NativeMethods.HardwareRevision(this.m_handle));
        }


        public int PigpioVersion()
        {
            return unchecked((int)PiGpiodIf2NativeMethods.PiGpioVersion(this.m_handle));
        }

        CallbackInfo IPiGPIO.AddCallback(int gpio, Edge either, GpioCallback callback)
        {
            throw new NotImplementedException();
        }

        void IPiGPIO.BSpiClose(int gpioCS)
        {
            throw new NotImplementedException();
        }

        void IPiGPIO.BSpiOpen(int gpioCS, int gpioMiso, int gpioMosi, int gpioClk, int bauds, int flags)
        {
            throw new NotImplementedException();
        }

        byte[] IPiGPIO.BSpiXfer(int gpioCS, byte[] txBuffer)
        {
            throw new NotImplementedException();
        }

        void IPiGPIO.ClearBits_0_31(int bits)
        {
            throw new NotImplementedException();
        }

        void IPiGPIO.ClearBits_32_53(int bits)
        {
            throw new NotImplementedException();
        }

        void IPiGPIO.FileClose(int handle)
        {
            throw new NotImplementedException();
        }

        int IPiGPIO.FileOpen(string file, int mode)
        {
            throw new NotImplementedException();
        }

        byte[] IPiGPIO.FileRead(int handle, int count)
        {
            throw new NotImplementedException();
        }

        int IPiGPIO.FileSeek(int handle, int offset, int from)
        {
            throw new NotImplementedException();
        }

        void IPiGPIO.FileWrite(int handle, byte[] data)
        {
            throw new NotImplementedException();
        }

        Mode IPiGPIO.GetMode(int gpio)
        {
            throw new NotImplementedException();
        }

        void IPiGPIO.I2CClose(int handle)
        {
            throw new NotImplementedException();
        }

        int IPiGPIO.I2COpen(int bus, int address, int flags)
        {
            throw new NotImplementedException();
        }

        byte[] IPiGPIO.I2CReadBytes(int handle, int num)
        {
            throw new NotImplementedException();
        }

        void IPiGPIO.NoiseFilter(int gpio, int steady, int active)
        {
            throw new NotImplementedException();
        }


        bool IPiGPIO.Read(int gpio)
        {
            throw new NotImplementedException();
        }

        int IPiGPIO.ReadBits_0_31()
        {
            throw new NotImplementedException();
        }

        int IPiGPIO.ReadBits_32_53()
        {
            throw new NotImplementedException();
        }

        void IPiGPIO.RemoveCallback(CallbackInfo callback)
        {
            throw new NotImplementedException();
        }

        void IPiGPIO.SetBits_0_31(int bits)
        {
            throw new NotImplementedException();
        }

        void IPiGPIO.SetBits_32_53(int bits)
        {
            throw new NotImplementedException();
        }

        void IPiGPIO.SetMode(int gpio, Mode mode)
        {
            throw new NotImplementedException();
        }

        void IPiGPIO.SetPullUpDown(int gpio, PullUpDown pud)
        {
            throw new NotImplementedException();
        }

        void IPiGPIO.SpiClose(int handle)
        {
            throw new NotImplementedException();
        }

        int IPiGPIO.SpiOpen(int chan, int baud, int flags)
        {
            throw new NotImplementedException();
        }

        byte[] IPiGPIO.SpiRead(int handle, int count)
        {
            throw new NotImplementedException();
        }

        void IPiGPIO.SpiWrite(int handle, byte[] data)
        {
            throw new NotImplementedException();
        }

        byte[] IPiGPIO.SpiXfer(int handle, byte[] txBuffer)
        {
            throw new NotImplementedException();
        }

        void IPiGPIO.WaveformAppend(Pulse[] pulse)
        {
            throw new NotImplementedException();
        }

        int IPiGPIO.WaveformCreate()
        {
            throw new NotImplementedException();
        }

        void IPiGPIO.WaveformDelete(int waveformId)
        {
            throw new NotImplementedException();
        }

        void IPiGPIO.WaveFormNew()
        {
            throw new NotImplementedException();
        }

        void IPiGPIO.Write(int gpio, bool value)
        {
            throw new NotImplementedException();
        }
    }
}
