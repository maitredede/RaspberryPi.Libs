using System;
using System.Collections.Generic;
using System.Text;

namespace PiGPIO
{
    /// <summary>
    /// SPI Helper class
    /// </summary>
    public sealed class SPIHelper : IDisposable
    {
        private readonly IPiGPIO m_pigs;
        private readonly int m_handle;

        internal SPIHelper(IPiGPIO pigs, int handle)
        {
            this.m_pigs = pigs;
            this.m_handle = handle;
        }

        public void Dispose()
        {
            this.m_pigs.SpiClose(this.m_handle);
        }

        public void Write(byte[] data)
        {
            this.m_pigs.SpiWrite(this.m_handle, data);
        }

        public void Write(byte[] data, int offset, int count)
        {
            byte[] realData = new byte[count];
            Array.Copy(data, offset, realData, 0, count);
            this.m_pigs.SpiWrite(this.m_handle, realData);
        }
    }
}
