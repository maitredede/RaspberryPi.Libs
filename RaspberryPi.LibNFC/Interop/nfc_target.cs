using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace RaspberryPi.LibNFC.Interop
{
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
    internal struct nfc_target
    {

        /// nfc_target_info->Anonymous_f3355045_dd81_4cc3_a4ef_b18e4d3694ad
        //public nfc_target_info nti;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = nfc_target_info.SIZE)]
        public byte[] ntiData;

        /// nfc_modulation->Anonymous_6f34bfa1_e761_4ac7_b173_298f476317a3
        public NfcModulation nm;
    }

}
