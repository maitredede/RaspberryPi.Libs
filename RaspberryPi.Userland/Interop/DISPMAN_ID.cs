using System;
using System.Collections.Generic;
using System.Text;

namespace RaspberryPi.Userland.Interop
{
    internal enum DISPMAN_ID : UInt32
    {
        MAIN_LCD = 0,
        AUX_LCD = 1,
        HDMI = 2,
        SDTV = 3,
        FORCE_LCD = 4,
        FORCE_TV = 5,
        FORCE_OTHER = 6
    }
}
