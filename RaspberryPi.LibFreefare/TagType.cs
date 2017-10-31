using System;
using System.Collections.Generic;
using System.Text;

namespace RaspberryPi.LibFreefare
{
    public enum TagType
    {
        FELICA,
        MIFARE_MINI,
        MIFARE_CLASSIC_1K,
        MIFARE_CLASSIC_4K,
        MIFARE_DESFIRE,
        //    MIFARE_PLUS_S2K,
        //    MIFARE_PLUS_S4K,
        //    MIFARE_PLUS_X2K,
        //    MIFARE_PLUS_X4K,
        MIFARE_ULTRALIGHT,
        MIFARE_ULTRALIGHT_C,
        NTAG_21x,
    }
}
