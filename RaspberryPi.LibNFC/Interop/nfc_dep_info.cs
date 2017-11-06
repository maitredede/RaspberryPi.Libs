using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace RaspberryPi.LibNFC.Interop
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Size = nfc_target_info.SIZE, Pack = 1)]
    public struct nfc_dep_info
    {

        /// uint8_t[10]
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst = 10)]
        public byte[] abtNFCID3;

        /// uint8_t->unsigned char
        public byte btDID;

        /// uint8_t->unsigned char
        public byte btBS;

        /// uint8_t->unsigned char
        public byte btBR;

        /// uint8_t->unsigned char
        public byte btTO;

        /// uint8_t->unsigned char
        public byte btPP;

        /// uint8_t[48]
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst = 48)]
        public byte[] abtGB;

        /// size_t->unsigned int
        public uint szGB;

        /// nfc_dep_mode->Anonymous_e738f66f_6fcc_4606_9eeb_ccd26a34ed3b
        public NfcDepMode ndm;
    }

}
