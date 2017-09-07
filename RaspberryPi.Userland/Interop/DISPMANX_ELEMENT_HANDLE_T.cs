using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace RaspberryPi.Userland.Interop
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct DISPMANX_ELEMENT_HANDLE_T
    {
        public IntPtr Handle;
    }
}
