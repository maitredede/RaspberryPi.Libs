using System;
using System.Collections.Generic;
using System.Text;

namespace RaspberryPi.Userland.SimpleGL.Interop
{
    [Flags]
    internal enum RTLD
    {
        RTLD_LAZY = 0x0001,
        RTLD_NOW = 0x0002,
        RTLD_GLOBAL = 0x0100,
        RTLD_LOCAL = 0x0000,
        RTLD_NOSHARE = 0x1000,
        RTLD_EXE = 0x2000,
        RTLD_SCRIPT = 0x4000,
    }
}
