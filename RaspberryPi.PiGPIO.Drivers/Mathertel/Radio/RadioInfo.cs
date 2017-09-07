using System;
using System.Collections.Generic;
using System.Text;

namespace PiGPIO.Drivers.Mathertel.Radio
{
    /// <summary>
    /// A structure that contains information about the radio features from the chip
    /// </summary>
    public struct RadioInfo
    {
        /// <summary>
        /// receiving is active.
        /// </summary>
        public bool Active { get; set; }
        /// <summary>
        /// Radio Station Strength Information.
        /// </summary>
        public byte RSSI { get; set; }
        /// <summary>
        /// Signal Noise Ratio.
        /// </summary>
        public byte SNR { get; set; }
        /// <summary>
        /// RDS information is available.
        /// </summary>
        public bool RDS { get; set; }
        /// <summary>
        ///  A stable frequency is tuned.
        /// </summary>
        public bool Tuned { get; set; }
        /// <summary>
        /// Mono mode is on.
        /// </summary>
        public bool Mono { get; set; }
        /// <summary>
        /// Stereo audio is available
        /// </summary>
        public bool Stereo { get; set; }
    }
}
