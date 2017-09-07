using System;
using System.Collections.Generic;
using System.Text;

namespace PiGPIO.Drivers.Mathertel.Radio
{
    /// <summary>
    /// A structure that contains information about the audio features
    /// </summary>
    public struct AudioInfo
    {
        public byte Volume { get; set; }
        public bool Mute { get; set; }
        public bool Softmute { get; set; }
        public bool BassBoost { get; set; }
    }
}
