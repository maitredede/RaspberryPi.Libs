using System;
using System.Collections.Generic;
using System.Text;
using RaspberryPi.LibNFC;
using RaspberryPi.LibFreefare.Interop;

namespace RaspberryPi.LibFreefare
{
    public abstract class MifareClassicTag : FreefareTag
    {
        internal MifareClassicTag(IntPtr ptr, bool dispose, NfcDevice device, NfcTarget target) : base(ptr, dispose, device, target)
        {
        }

        public MifareClassicConnection Connect()
        {
            int code = NativeMethods.mifare_classic_connect(this.Handle);
            if (code < 0)
                throw new FreefareException("Can't connect to tag: " + this.LastError());
            return new MifareClassicConnection(this);
        }
    }
}
