using RaspberryPi.LibNFC.Interop;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace RaspberryPi.LibNFC
{
    public sealed class NfcTargetList : NativeCollection<NfcTarget>
    {
        private readonly int m_count;
        private readonly NfcDevice m_device;

        internal NfcTargetList(IntPtr ptr, int count, NfcDevice device) : base(ptr, true)
        {
            this.m_count = count;
            this.m_device = device;
        }

        protected override void FreeHandle()
        {
            Marshal.FreeHGlobal(this.handle);
        }

        protected override NfcTarget[] BuildArrayFromHandle(IntPtr ptr)
        {
            if (ptr != IntPtr.Zero && this.m_count > 0)
            {
                List<NfcTarget> targets = new List<NfcTarget>();
                for (int i = 0; i < this.m_count; i++)
                {
                    NfcTarget target = new NfcTarget(this.m_device, ptr + i * IntPtr.Size);
                    targets.Add(target);
                }
                return targets.ToArray();
            }
            else
            {
                return new NfcTarget[0];
            }
        }
    }
}
