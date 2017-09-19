using System;
using System.Collections.Generic;
using System.Text;

namespace RaspberryPi.PiGPIO
{
    internal abstract class BaseHelper : IDisposable
    {
        protected readonly IPiGPIO m_pigs;
        protected readonly int m_handle;

        internal BaseHelper(IPiGPIO pigs, int handle)
        {
            this.m_pigs = pigs;
            this.m_handle = handle;
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        protected abstract void Dispose(bool disposing);
    }
}
