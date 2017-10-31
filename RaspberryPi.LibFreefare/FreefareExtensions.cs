using RaspberryPi.LibFreefare.Interop;
using RaspberryPi.LibNFC;
using System;
using System.Collections.Generic;
using System.Text;

namespace RaspberryPi.LibFreefare
{
    public static class FreefareExtensions
    {
        public static FreefareTag[] FreefareGetTags(this NfcDevice device)
        {
            IntPtr[] ptr = NativeMethods.freefare_get_tags(device.Handle);
            for(int i = 0; i < ptr.Length; i++)
            {

            }
        }
    }
}
