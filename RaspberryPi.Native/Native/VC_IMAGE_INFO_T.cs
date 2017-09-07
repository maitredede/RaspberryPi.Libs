using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace RaspberryPi.Native
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct VC_IMAGE_INFO_T
    {
        VC_IMAGE_YUVINFO_T yuv;
        ushort info;
    }
}
