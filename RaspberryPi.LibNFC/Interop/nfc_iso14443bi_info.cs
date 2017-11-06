using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace RaspberryPi.LibNFC.Interop
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Size = nfc_target_info.SIZE, Pack = 1)]
    public struct nfc_iso14443bi_info
    {

        /// uint8_t[4]
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] abtDIV;

        /// uint8_t->unsigned char
        public byte btVerLog;

        /// uint8_t->unsigned char
        public byte btConfig;

        /// size_t->unsigned int
        public uint szAtrLen;

        /// uint8_t[33]
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst = 33)]
        public byte[] abtAtr;
    }
}
