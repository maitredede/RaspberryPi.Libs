using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace RaspberryPi.MMAL.Interop
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct MMAL_PARAMETER_INT32_T
    {
        public MMAL_PARAMETER_HEADER_T hdr;
        [MarshalAs(UnmanagedType.I4)]
        public int value;
    }
}
