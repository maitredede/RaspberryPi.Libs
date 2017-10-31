using System;
using System.Collections.Generic;
using System.Text;

namespace RaspberryPi.LibNFC
{
    public enum NfcModulationType
    {
        ISO14443A = 1,
        Jewel = 2,
        Barcode = 3,
        ISO14443B = 4,
        ISO14443BI = 5, // pre-ISO14443B aka ISO/IEC 14443 B' or Type B'
        ISO14443B2SR = 6, // ISO14443-2B ST SRx
        ISO14443B2CT = 7, // ISO14443-2B ASK CTx
        Felica = 8,
        Dep = 9,        // DEP should be kept last one as it's used as end-of-enum
    }
}
