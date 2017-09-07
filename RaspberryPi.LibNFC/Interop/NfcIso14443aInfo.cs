//using System;
//using System.Collections.Generic;
//using System.Runtime.InteropServices;
//using System.Text;

//namespace RaspberryPi.LibNFC.Interop
//{
//    [StructLayout(LayoutKind.Sequential, Pack = 1)]
//    internal sealed class NfcIso14443aInfo : NfcTargetInfo
//    {
//        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
//        public byte[] abtAtqa;
//        public byte btSak;
//        public int szUidLen;
//        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
//        public byte[] abtUid;
//        public int szAtsLen;
//        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 254)]
//        public byte[] abtAts; // Maximal theoretical ATS is FSD-2, FSD=256 for FSDI=8 in RATS
//    }
//}
