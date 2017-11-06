using RaspberryPi.LibFreefare.Interop;
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Runtime.InteropServices;
using RaspberryPi.LibNFC;
using RaspberryPi.LibNFC.Interop;

namespace RaspberryPi.LibFreefare
{
    /// <summary>
    /// List of Freefare tags
    /// </summary>
    public sealed class FreefareTagList : NativeCollection<FreefareTag>
    {
        private readonly NfcDevice m_device;
        private readonly IntPtr m_ptr;

        internal FreefareTagList(IntPtr ptr, NfcDevice device) : base(ptr, true)
        {
            this.m_device = device;
        }

        protected override FreefareTag[] BuildArrayFromHandle(IntPtr ptr)
        {
            List<FreefareTag> lst = new List<FreefareTag>();
            int i = 0;
            IntPtr ptrTag;
            while ((ptrTag = Marshal.ReadIntPtr(this.m_ptr, IntPtr.Size * i++)) != IntPtr.Zero)
            {
                Console.WriteLine("i={0} ptr={1}", i, ptrTag);
                FreefareTag tag = FreefareTag.Build(ptrTag, false, this.m_device, null);
                lst.Add(tag);
            }
            return lst.ToArray();
        }

        protected override void FreeHandle()
        {
            Interop.NativeMethods.freefare_free_tags(this.m_ptr);
        }
    }
}
