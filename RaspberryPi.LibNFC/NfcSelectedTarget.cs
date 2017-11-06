using RaspberryPi.LibNFC.Interop;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace RaspberryPi.LibNFC
{
    public sealed class NfcSelectedTarget : NativeObject
    {
        private readonly NfcDevice m_device;
        private readonly NfcTarget m_target;

        internal NfcSelectedTarget(IntPtr targetPtr, NfcDevice device) : base(targetPtr, true)
        {
            this.m_device = device;
            this.m_target = new NfcTarget(this.m_device, targetPtr);
        }

        protected override void FreeHandle()
        {
            NfcError code = NativeMethods.initiator_deselect_target(this.m_device.DangerousGetHandle());
            if (code != NfcError.Success)
            {
                NfcException.Raise(code);
            }
            Marshal.FreeHGlobal(this.handle);
        }

        internal NfcTarget Target => this.m_target;

        public bool IsPresent()
        {
            NfcError code = NativeMethods.initiator_target_is_present(this.m_device.DangerousGetHandle(), this.handle);
            if (code == NfcError.Success)
            {
                return true;
            }
            else
            {
                Console.WriteLine("IsPresent returned {0}", code);
                return false;
            }
        }
    }
}
