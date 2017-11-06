using System;
using System.Collections.Generic;
using System.Text;

namespace RaspberryPi.LibNFC.Mifare
{
    public sealed class MifareClassic1kTag : MifareTag
    {
        internal MifareClassic1kTag(NfcSelectedTarget target) : base(target)
        {
        }
    }
}
