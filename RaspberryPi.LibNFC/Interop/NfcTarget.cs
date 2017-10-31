using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace RaspberryPi.LibNFC.Interop
{
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 283)]
    internal struct NfcTarget
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 283)]
        public byte[] Data;
    }
}
