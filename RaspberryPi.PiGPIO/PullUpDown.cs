using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiGPIO
{
    /// <summary>
    /// Resistor pull-up mode
    /// </summary>
    public enum PullUpDown
    {
        /// <summary>
        /// No pull-up
        /// </summary>
        Off = 0,
        /// <summary>
        /// Pull down
        /// </summary>
        Down = 1,
        /// <summary>
        /// Pull up
        /// </summary>
        Up = 2,
    }
}
