using RaspberryPi.LibFreefare;
using RaspberryPi.LibFreefare.Interop;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace RaspberryPi.LibNFC
{
    public static class FreefareExtensions
    {
        public static FreefareTagList FreefareGetTags(this NfcDevice device)
        {
            IntPtr arrPtr = NativeMethods.freefare_get_tags(device.Handle);
            if (arrPtr == IntPtr.Zero)
                throw new FreefareException("Error listing tags");
            return new FreefareTagList(arrPtr);
        }
    }
}
