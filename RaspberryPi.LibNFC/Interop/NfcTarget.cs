using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace RaspberryPi.LibNFC.Interop
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public sealed class NfcTarget
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public byte[] TargetInfo;
        public NfcModulation Modulation;
    }
}
