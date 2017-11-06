using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace RaspberryPi.LibNFC
{
    [StructLayout(LayoutKind.Sequential)]
    public struct NfcModulation
    {
        [MarshalAs(UnmanagedType.I4)]
        public NfcModulationType ModulationType;
        [MarshalAs(UnmanagedType.I4)]
        public NfcBaudRate BaudRate;

        public NfcModulation(NfcModulationType type, NfcBaudRate rate)
        {
            this.ModulationType = type;
            this.BaudRate = rate;
        }

        public override string ToString()
        {
            return string.Format("Modulation: {0} at {1}", this.ModulationType, this.BaudRate);
        }
    }
}
