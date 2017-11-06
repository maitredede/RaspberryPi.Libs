using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace RaspberryPi.LibNFC.Interop
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Size = nfc_target_info.SIZE, Pack = 1)]
    public struct nfc_iso14443a_info
    {

        /// uint8_t[2]
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] abtAtqa;

        /// uint8_t->unsigned char
        public byte btSak;

        /// size_t->unsigned int
        public uint szUidLen;

        /// uint8_t[10]
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst = 10)]
        public byte[] abtUid;

        /// size_t->unsigned int
        public uint szAtsLen;

        /// uint8_t[254]
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst = 254)]
        public byte[] abtAts;
    }
}
