using RaspberryPi.LibNFC.Interop;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace RaspberryPi.LibNFC
{
    public sealed class NfcTarget
    {
        private readonly NfcDevice m_device;
        private readonly IntPtr m_ptr;

        internal IntPtr Handle => this.m_ptr;
        internal NfcDevice Device => this.m_device;

        internal NfcTarget(NfcDevice device, IntPtr ptr)
        {
            this.m_device = device;
            this.m_ptr = ptr;
        }

        public override string ToString()
        {
            int len = NativeMethods.str_nfc_target(out IntPtr ptr, this.m_device.DangerousGetHandle(), true);
            try
            {
                return Marshal.PtrToStringAnsi(ptr, len);
            }
            finally
            {
                NativeMethods.nfc_free(ptr);
            }
        }

        internal T ReadInfo<T>()
        {
            T info = Marshal.PtrToStructure<T>(this.m_ptr);
            return info;
        }
        internal NfcModulation ReadModulation()
        {
            IntPtr ptrModulation = this.m_ptr + nfc_target_info.SIZE;
            NfcModulation modulation = Marshal.PtrToStructure<NfcModulation>(ptrModulation);
            return modulation;
        }
    }
}
