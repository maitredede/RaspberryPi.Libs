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

        internal NfcTarget(NfcDevice device, IntPtr ptr)
        {
            this.m_device = device;
            this.m_ptr = ptr;
        }

        public bool IsPresent()
        {
            return NativeMethods.initiator_target_is_present(this.m_device.Handle, this.m_ptr) == NfcError.Success;
        }

        public override string ToString()
        {
            int len = NativeMethods.str_nfc_target(out IntPtr ptr, this.m_device.Handle, true);
            try
            {
                return Marshal.PtrToStringAnsi(ptr, len);
            }
            finally
            {
                NativeMethods.nfc_free(ptr);
            }
        }
    }
}
