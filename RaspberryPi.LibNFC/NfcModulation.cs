using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace RaspberryPi.LibNFC
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public sealed class NfcModulation
    {
        public NfcModulationType ModulationType { get; set; }
        public NfcBaudRate BaudRate { get; set; }

        public NfcModulation()
        {
        }

        public NfcModulation(NfcModulationType type, NfcBaudRate rate)
        {
            this.ModulationType = type;
            this.BaudRate = rate;
        }
    }
}
