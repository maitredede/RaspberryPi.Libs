using System;
using System.Collections.Generic;
using System.Text;

namespace RaspberryPi.PiGPIO
{
    internal sealed class BitBangSpiHelper : BaseHelper, ISpiHelper
    {
        internal BitBangSpiHelper(IPiGPIO pigs, int handle) : base(pigs, handle)
        {
        }

        protected override void Dispose(bool disposing)
        {
            this.m_pigs.BSpiClose(this.m_handle);
        }


        public byte[] Xfer(byte[] txBuffer)
        {
            return this.m_pigs.BSpiXfer(this.m_handle, txBuffer);
        }

        public void Write(byte[] txBuffer)
        {
            this.Xfer(txBuffer);
        }

        public byte[] Read(int count)
        {
            byte[] tx = new byte[count];
            return this.Xfer(tx);
        }
    }
}
