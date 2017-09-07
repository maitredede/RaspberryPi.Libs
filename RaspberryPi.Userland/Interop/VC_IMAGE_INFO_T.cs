using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace RaspberryPi.Userland.Interop
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct VC_IMAGE_INFO_T
    {
        VC_IMAGE_YUVINFO_T yuv;
        ushort info;
    }
}
