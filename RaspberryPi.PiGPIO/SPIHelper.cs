using System;
using System.Collections.Generic;
using System.Text;

namespace RaspberryPi.PiGPIO
{
    /// <summary>
    /// SPI Helper class
    /// </summary>
    internal sealed class SpiHelper : BaseHelper, ISpiHelper
    {
        internal SpiHelper(IPiGPIO pigs, int handle) : base(pigs, handle)
        {
        }

        protected override void Dispose(bool disposing)
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

        public byte[] Read(int count)
        {
            return this.m_pigs.SpiRead(this.m_handle, count);
        }

        public byte[] Xfer(byte[] txBuffer)
        {
            return this.m_pigs.SpiXfer(this.m_handle, txBuffer);
        }
    }
}
