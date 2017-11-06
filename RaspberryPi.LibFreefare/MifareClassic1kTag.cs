using System;
using System.Collections.Generic;
using System.Text;
using RaspberryPi.LibNFC;

namespace RaspberryPi.LibFreefare
{
    public sealed class MifareClassic1kTag : MifareClassicTag
    {
        internal MifareClassic1kTag(IntPtr ptr, bool dispose, NfcDevice device, NfcTarget target) : base(ptr, dispose, device, target)
        {
        }
    }
}
