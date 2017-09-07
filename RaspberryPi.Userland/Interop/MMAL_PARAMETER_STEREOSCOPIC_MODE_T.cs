using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace RaspberryPi.Userland.Interop
{
    internal struct MMAL_PARAMETER_STEREOSCOPIC_MODE_T
    {
        public MMAL_PARAMETER_HEADER_T hdr;
        [MarshalAs(UnmanagedType.I4)]
        public MMAL_STEREOSCOPIC_MODE_T mode;
        public /*MMAL_BOOL_T*/int decimate;
        public /*MMAL_BOOL_T*/int swap_eyes;
    }
}
