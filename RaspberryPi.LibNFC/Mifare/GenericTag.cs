using System;
using System.Collections.Generic;
using System.Text;

namespace RaspberryPi.LibNFC.Mifare
{
    public sealed class GenericTag : MifareTag
    {
        internal GenericTag(NfcSelectedTarget target) : base(target)
        {
        }
    }
}
