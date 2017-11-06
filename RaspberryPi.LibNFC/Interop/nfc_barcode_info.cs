using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace RaspberryPi.LibNFC.Interop
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Size = nfc_target_info.SIZE, Pack = 1)]
    public struct nfc_barcode_info
    {

        /// size_t->unsigned int
        public uint szDataLen;

        /// uint8_t[32]
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] abtData;
    }
}
