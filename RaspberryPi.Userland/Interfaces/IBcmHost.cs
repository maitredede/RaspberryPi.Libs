using RaspberryPi.Userland.MMAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace RaspberryPi.Userland.Interfaces
{
    public interface IBcmHost
    {
        DisplayManager Dispman { get; }
        MMALManager MMAL { get; }
    }
}
