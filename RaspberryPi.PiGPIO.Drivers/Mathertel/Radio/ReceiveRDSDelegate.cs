using System;
using System.Collections.Generic;
using System.Text;

namespace PiGPIO.Drivers.Mathertel.Radio
{
    /// <summary>
    /// Callback function for passing RDS data
    /// </summary>
    /// <param name="block1"></param>
    /// <param name="block2"></param>
    /// <param name="block3"></param>
    /// <param name="block4"></param>
    public delegate void ReceiveRDSDelegate(ushort block1, ushort block2, ushort block3, ushort block4);
}
