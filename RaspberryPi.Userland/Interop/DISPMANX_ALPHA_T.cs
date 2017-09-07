using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace RaspberryPi.Userland.Interop
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct DISPMANX_ALPHA_T
    {
        public DISPMANX_FLAGS_ALPHA_T flags;
        public uint opacity;
        public IntPtr mask;

        public VC_IMAGE_T GetMask()
        {
            return Marshal.PtrToStructure<VC_IMAGE_T>(this.mask);
        }
    }
}
