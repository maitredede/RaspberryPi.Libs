using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace RaspberryPi.LibNFC.Interop
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Size = nfc_target_info.SIZE, Pack = 1)]
    public struct nfc_iso14443b2sr_info
    {

        /// uint8_t[8]
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] abtUID;
    }
}
