using System;
using System.Collections.Generic;
using System.Text;

namespace RaspberryPi.PiGPIO
{
    /// <summary>
    /// I2C helper class
    /// </summary>
    public sealed class I2CHelper
    {
        private readonly IPiGPIO m_pigs;

        internal I2CHelper(IPiGPIO pigs)
        {
            this.m_pigs = pigs;
        }
    }
}
