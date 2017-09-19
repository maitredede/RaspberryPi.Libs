using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using NativeMethods = RaspberryPi.PiGPIO.Interop.PiGpioNativeMethods;

namespace RaspberryPi.PiGPIO
{
    public sealed class PiGpio : IPiGPIO, IDisposable
    {
        private readonly Dictionary<int, List<CallbackInfo>> m_callbacks = new Dictionary<int, List<CallbackInfo>>();

        public PiGpio()
        {
            int ret = NativeMethods.Initialise();
            if (ret < 0)
                throw new PiGPIOException(ret);
        }

        public void Dispose()
        {
            NativeMethods.Terminate();
        }

        /// <inheritDoc />
        public Mode GetMode(int gpio)
        {
            short value = NativeMethods.GetMode((ushort)gpio);
            if (value < 0)
                throw new PiGPIOException(value);
            return (Mode)value;
        }

        /// <inheritDoc />
        public void NoiseFilter(int gpio, int steady, int active)
        {
            short ret = NativeMethods.NoiseFilter((ushort)gpio, (ushort)steady, (ushort)active);
            if (ret < 0)
                throw new PiGPIOException(ret);
        }

        /// <inheritDoc />
        public void SetMode(int gpio, Mode mode)
        {
            short ret = NativeMethods.SetMode((ushort)gpio, (ushort)mode);
            if (ret < 0)
                throw new PiGPIOException(ret);
        }

        /// <inheritDoc />
        public void SetPullUpDown(int gpio, PullUpDown pud)
        {
            short ret = NativeMethods.SetPullUpDown((ushort)gpio, (ushort)pud);
            if (ret < 0)
                throw new PiGPIOException(ret);
        }

        private void GpioAlertCallback(int gpio, int level, uint tick)
        {
            List<CallbackInfo> callbacks;
            lock (this.m_callbacks)
            {
                if (!this.m_callbacks.ContainsKey(gpio))
                    return;
                callbacks = this.m_callbacks[gpio];
            }
            CallbackInfo[] cbs;
            lock (callbacks)
            {
                cbs = callbacks.ToArray();
            }
            foreach (var cb in cbs)
            {
                //TODO
            }
        }

        public CallbackInfo AddCallback(int gpio, Edge edge, GpioCallback callback)
        {
            List<CallbackInfo> callbacks;
            lock (this.m_callbacks)
            {
                if (this.m_callbacks.ContainsKey(gpio))
                {
                    callbacks = this.m_callbacks[gpio];
                }
                else
                {
                    short ret = NativeMethods.SetAlertFunc((ushort)gpio, this.GpioAlertCallback);
                    if (ret < 0)
                        throw new PiGPIOException(ret);
                    callbacks = new List<CallbackInfo>();
                    this.m_callbacks.Add(gpio, callbacks);
                }
            }

            CallbackInfo cbi = new CallbackInfo((uint)gpio, 0, edge, callback);
            lock (callbacks)
            {
                callbacks.Add(cbi);
            }
            return cbi;
        }

        public void RemoveCallback(CallbackInfo callback)
        {
            List<CallbackInfo> callbacks;
            lock (this.m_callbacks)
            {
                if (!this.m_callbacks.ContainsKey(callback.Port))
                    return;
                callbacks = this.m_callbacks[callback.Port];
            }
            lock (callbacks)
            {
                callbacks.Remove(callback);
            }
        }

        public void Write(int gpio, bool value)
        {
            short ret = NativeMethods.Write((ushort)gpio, (ushort)(value ? 1 : 0));
            if (ret < 0)
                throw new PiGPIOException(ret);
        }

        public bool Read(int gpio)
        {
            short ret = NativeMethods.Read((ushort)gpio);
            if (ret < 0)
                throw new PiGPIOException(ret);
            return ret != 0;
        }

        public int I2COpen(int bus, int address, int flags)
        {
            short ret = NativeMethods.I2COpen((ushort)bus, (ushort)address, (ushort)flags);
            if (ret < 0)
                throw new PiGPIOException(ret);
            return ret;
        }

        public void I2CClose(int handle)
        {
            short ret = NativeMethods.I2CClose((ushort)handle);
            if (ret < 0)
                throw new PiGPIOException(ret);
        }

        public int HardwareRevision()
        {
            return NativeMethods.HardwareRevision();
        }

        int IPiGPIO.FileOpen(string file, int mode)
        {
            throw new NotImplementedException();
        }

        int IPiGPIO.FileClose(int handle)
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

        public byte[] I2CReadBytes(int handle, int num)
        {
            byte[] data = new byte[num];
            GCHandle h = GCHandle.Alloc(data, GCHandleType.Pinned);
            try
            {
                short ret = NativeMethods.i2cReadDevice((ushort)handle, h.AddrOfPinnedObject(), (ushort)num);
                if (ret < 0)
                    throw new PiGPIOException(ret);
                if (ret < num)
                {
                    byte[] real = new byte[ret];
                    Array.Copy(data, real, ret);
                    return real;
                }
                else
                {
                    return data;
                }
            }
            finally
            {
                h.Free();
            }
        }

        public int SpiOpen(int chan, int baud, int flags)
        {
            short ret = NativeMethods.spiOpen((ushort)chan, (ushort)baud, (ushort)flags);
            if (ret < 0)
                throw new PiGPIOException(ret);
            return ret;
        }

        public void SpiClose(int handle)
        {
            short ret = NativeMethods.spiClose((ushort)handle);
            if (ret < 0)
                throw new PiGPIOException(ret);
        }

        public void SpiWrite(int handle, byte[] data)
        {
            GCHandle h = GCHandle.Alloc(data, GCHandleType.Pinned);
            try
            {
                short ret = NativeMethods.spiWrite((ushort)handle, h.AddrOfPinnedObject(), (ushort)data.Length);
                if (ret < 0)
                    throw new PiGPIOException(ret);
            }
            finally
            {
                h.Free();
            }
        }

        public byte[] SpiRead(int handle, int count)
        {
            byte[] data = new byte[count];
            GCHandle h = GCHandle.Alloc(data, GCHandleType.Pinned);
            try
            {
                short ret = NativeMethods.spiRead((ushort)handle, h.AddrOfPinnedObject(), (ushort)data.Length);
                if (ret < 0)
                    throw new PiGPIOException(ret);
            }
            finally
            {
                h.Free();
            }
            return data;
        }

        public byte[] SpiXfer(int handle, byte[] txBuff)
        {
            byte[] rxBuff = new byte[txBuff.Length];
            GCHandle tx = GCHandle.Alloc(txBuff, GCHandleType.Pinned);
            try
            {
                GCHandle rx = GCHandle.Alloc(rxBuff, GCHandleType.Pinned);
                try
                {
                    short ret = NativeMethods.spiXfer((ushort)handle, tx.AddrOfPinnedObject(), tx.AddrOfPinnedObject(), (ushort)txBuff.Length);
                    if (ret < 0)
                        throw new PiGPIOException(ret);
                }
                finally
                {
                    rx.Free();
                }
            }
            finally
            {
                tx.Free();
            }
            return rxBuff;
        }

        void IPiGPIO.WaveFormNew()
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

        void IPiGPIO.BSpiOpen(int gpioCS, int gpioMiso, int gpioMosi, int gpioClk, int bauds, int flags)
        {
            throw new NotImplementedException();
        }

        void IPiGPIO.BSpiClose(int gpioCS)
        {
            throw new NotImplementedException();
        }

        byte[] IPiGPIO.BSpiXfer(int gpioCS, byte[] txBuffer)
        {
            throw new NotImplementedException();
        }
    }
}
