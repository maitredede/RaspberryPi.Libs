using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace RaspberryPi.Native
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct DISPMANX_PROTECTION_T
    {
        public IntPtr Handle;
    }
}
