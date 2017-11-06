using System;
using System.Collections.Generic;
using System.Text;

namespace RaspberryPi.LibFreefare
{
    [Flags]
    public enum MifareClassicAccessBits
    {
        MCAB_R = 0x8,
        MCAB_W = 0x4,
        MCAB_D = 0x2,
        MCAB_I = 0x1,

        MCAB_READ_KEYA = 0x400,
        MCAB_WRITE_KEYA = 0x100,
        MCAB_READ_ACCESS_BITS = 0x040,
        MCAB_WRITE_ACCESS_BITS = 0x010,
        MCAB_READ_KEYB = 0x004,
        MCAB_WRITE_KEYB = 0x001,
    }
}
