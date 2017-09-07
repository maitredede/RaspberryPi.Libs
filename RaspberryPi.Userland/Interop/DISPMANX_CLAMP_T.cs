using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace RaspberryPi.Userland.Interop
{
    [StructLayout(LayoutKind.Sequential)]
    internal sealed class DISPMANX_CLAMP_T
    {
        public DISPMANX_FLAGS_CLAMP_T mode;
        public DISPMANX_FLAGS_KEYMASK_T key_mask;
        public DISPMANX_CLAMP_KEYS_T key_value;
        public uint replace_value;
    }
}
