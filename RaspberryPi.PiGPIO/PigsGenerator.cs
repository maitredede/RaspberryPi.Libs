using System;
using System.Collections.Generic;
using System.Text;

namespace RaspberryPi.PiGPIO
{
    public sealed class PigsGenerator : IPiGPIO
    {
        private readonly object m_lck;
        private readonly StringBuilder m_sb;
        private readonly string m_separator;

        public PigsGenerator() : this(" ") { }
        public PigsGenerator(string separator)
        {
            this.m_sb = new StringBuilder();
            this.m_lck = new object();
            this.m_separator = separator;
        }

        void IDisposable.Dispose()
        {
        }

        public void Clear()
        {
            lock (this.m_lck)
            {
                this.m_sb.Clear();
            }
        }

        public override string ToString()
        {
            lock (this.m_lck)
            {
                return this.m_sb.ToString();
            }
        }

        CallbackInfo IPiGPIO.AddCallback(int gpio, Edge either, GpioCallback callback)
        {
            throw new NotSupportedException();
        }

        void IPiGPIO.RemoveCallback(CallbackInfo callback)
        {
            throw new NotSupportedException();
        }

        int IPiGPIO.HardwareRevision()
        {
            return 0;
        }

        int IPiGPIO.PigpioVersion()
        {
            return 0;
        }

        private void Append(FormattableString str)
        {
            lock (this.m_lck)
            {
                this.m_sb.Append(str);
                this.m_sb.Append(" ");
            }
        }

        private void Append(string format, params object[] args)
        {
            lock (this.m_lck)
            {
                this.m_sb.AppendFormat(format, args);
                this.m_sb.Append(" ");
            }
        }

        public void BSpiClose(int gpioCS)
        {
            this.Append($"BSPIC {gpioCS}");
        }

        public void BSpiOpen(int gpioCS, int gpioMiso, int gpioMosi, int gpioClk, int bauds, int flags)
        {
            this.Append($"BSPIO {gpioCS} {gpioMiso} {gpioMosi} {gpioClk} {bauds} {flags}");
        }

        public byte[] BSpiXfer(int gpioCS, byte[] txBuffer)
        {
            this.Append($"BSPIX {gpioCS}");
            for (int i = 0; i < txBuffer.Length; i++)
            {
                this.Append(txBuffer[i].ToString());
            }
            return new byte[txBuffer.Length];
        }

        public void ClearBits_0_31(int bits)
        {
            this.Append($"BC1 {bits}");
        }

        public void ClearBits_32_53(int bits)
        {
            this.Append($"BC2 {bits}");
        }

        public void FileClose(int handle)
        {
            throw new NotImplementedException();
        }

        public int FileOpen(string file, int mode)
        {
            throw new NotImplementedException();
        }

        public byte[] FileRead(int handle, int count)
        {
            throw new NotImplementedException();
        }

        public int FileSeek(int handle, int offset, int from)
        {
            throw new NotImplementedException();
        }

        public void FileWrite(int handle, byte[] data)
        {
            throw new NotImplementedException();
        }

        public Mode GetMode(int gpio)
        {
            throw new NotImplementedException();
        }

        public void I2CClose(int handle)
        {
            throw new NotImplementedException();
        }

        public int I2COpen(int bus, int address, int flags)
        {
            throw new NotImplementedException();
        }

        public byte[] I2CReadBytes(int handle, int num)
        {
            throw new NotImplementedException();
        }

        public void NoiseFilter(int gpio, int steady, int active)
        {
            throw new NotImplementedException();
        }

        public bool Read(int gpio)
        {
            throw new NotImplementedException();
        }

        public int ReadBits_0_31()
        {
            throw new NotImplementedException();
        }

        public int ReadBits_32_53()
        {
            throw new NotImplementedException();
        }

        public void SetBits_0_31(int bits)
        {
            throw new NotImplementedException();
        }

        public void SetBits_32_53(int bits)
        {
            throw new NotImplementedException();
        }

        public void SetMode(int gpio, Mode mode)
        {
            this.Append($"MODES {gpio}");
            switch (mode)
            {
                case Mode.Input: this.Append("R"); break;
                case Mode.Output: this.Append("W"); break;
                case Mode.Alt0: this.Append("0"); break;
                case Mode.Alt1: this.Append("1"); break;
                case Mode.Alt2: this.Append("2"); break;
                case Mode.Alt3: this.Append("3"); break;
                case Mode.Alt4: this.Append("4"); break;
                case Mode.Alt5: this.Append("5"); break;
            }
        }

        public void SetPullUpDown(int gpio, PullUpDown pud)
        {
            throw new NotImplementedException();
        }

        public void SpiClose(int handle)
        {
            throw new NotImplementedException();
        }

        public int SpiOpen(int chan, int baud, int flags)
        {
            throw new NotImplementedException();
        }

        public byte[] SpiRead(int handle, int count)
        {
            throw new NotImplementedException();
        }

        public void SpiWrite(int handle, byte[] data)
        {
            throw new NotImplementedException();
        }

        public byte[] SpiXfer(int handle, byte[] txBuffer)
        {
            throw new NotImplementedException();
        }

        public void WaveformAppend(Pulse[] pulse)
        {
            throw new NotImplementedException();
        }

        public int WaveformCreate()
        {
            throw new NotImplementedException();
        }

        public void WaveformDelete(int waveformId)
        {
            throw new NotImplementedException();
        }

        public void WaveFormNew()
        {
            throw new NotImplementedException();
        }

        public void Write(int gpio, bool value)
        {
            this.Append($"WRITE {gpio} {(value ? 1 : 0)}");
        }
    }
}
