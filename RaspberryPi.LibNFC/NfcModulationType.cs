using System;
using System.Collections.Generic;
using System.Text;

namespace RaspberryPi.LibNFC
{
    public enum NfcModulationType
    {
        ISO14443A = 1,
        Jewel,
        Barcode,
        ISO14443B,
        ISO14443BI, // pre-ISO14443B aka ISO/IEC 14443 B' or Type B'
        ISO14443B2SR, // ISO14443-2B ST SRx
        ISO14443B2CT, // ISO14443-2B ASK CTx
        Felica,
        Dep,        // DEP should be kept last one as it's used as end-of-enum
    }
}
