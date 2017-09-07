using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace RaspberryPi.Native
{
    [StructLayout(LayoutKind.Sequential)]
    public struct DISPMANX_WINDOW_T
    {
        public IntPtr element;
        public int width, height;
    }
}
