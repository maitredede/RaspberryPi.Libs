using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaspberryPi.PiGPIO
{
    /// <summary>
    /// Port mode
    /// </summary>
    public enum Mode
    {
        /// <summary>
        /// Input
        /// </summary>
        Input = 0,
        /// <summary>
        /// Output
        /// </summary>
        Output = 1,
        Alt0 = 4,
        Alt1 = 5,
        Alt2 = 6,
        Alt3 = 7,
        Alt4 = 3,
        Alt5 = 2,
    }
}
