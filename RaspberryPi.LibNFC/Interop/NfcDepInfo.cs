using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace RaspberryPi.LibNFC.Interop
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal sealed class NfcDepInfo
    {
        /** NFCID3*/
        //uint8_t abtNFCID3[10];
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public byte[] abtNFCID3;
        /** DID */
        public byte btDID;
        /** Supported send-bit rate */
        public byte btBS;
        /** Supported receive-bit rate */
        public byte btBR;
        /** Timeout value */
        public byte btTO;
        /** PP Parameters */
        public byte btPP;
        /** General Bytes */
        //public byte abtGB[48];
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 48)]
        public byte[] abtGB;
        public int szGB;
        /** DEP mode */
        public NfcDepMode ndm;
    }
}
