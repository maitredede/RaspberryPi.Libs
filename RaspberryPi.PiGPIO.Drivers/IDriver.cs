using System;
using System.Collections.Generic;
using System.Text;

namespace RaspberryPi.PiGPIO.Drivers
{
    public interface IDriver
    {
        /// <summary>
        /// Initialize the driver
        /// </summary>
        void Init();
    }
}
