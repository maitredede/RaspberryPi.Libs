using System;
using System.Collections.Generic;
using System.Text;

namespace RaspberryPi.PiGPIO.Drivers.Mathertel.Radio
{
    /// <summary>
    /// The BANDs a receiver probably can implement
    /// </summary>
    public enum RadioBand
    {
        /// <summary>
        /// No band selected
        /// </summary>
        None = 0,
        /// <summary>
        /// FM band 87.5 – 108 MHz (USA, Europe) selected.
        /// </summary>
        FM = 1,
        /// <summary>
        /// FM band 76 – 108 MHz (Japan, Worldwide) selected
        /// </summary>
        FMWorld = 2,
        /// <summary>
        /// AM band selected
        /// </summary>
        AM = 3,
        /// <summary>
        /// KW band selected.
        /// </summary>
        KW = 4
    }
}
