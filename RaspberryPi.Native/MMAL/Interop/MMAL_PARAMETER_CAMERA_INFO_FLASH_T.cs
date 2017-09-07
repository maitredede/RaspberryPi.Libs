using RaspberryPi.MMAL;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace RaspberryPi.MMAL.Interop
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct MMAL_PARAMETER_CAMERA_INFO_FLASH_T
    {
        [MarshalAs(UnmanagedType.I4)]
        //public MMAL_PARAMETER_CAMERA_INFO_FLASH_TYPE_T flash_type;
        public int flash_type;
    }
}
