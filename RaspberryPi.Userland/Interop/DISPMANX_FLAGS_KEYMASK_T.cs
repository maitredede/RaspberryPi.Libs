using System;
using System.Collections.Generic;
using System.Text;

namespace RaspberryPi.Userland.Interop
{
    [Flags]
    internal enum DISPMANX_FLAGS_KEYMASK_T
    {
        OVERRIDE = 1,
        SMOOTH = 1 << 1,
        CR_INV = 1 << 2,
        CB_INV = 1 << 3,
        YY_INV = 1 << 4
    }
}
