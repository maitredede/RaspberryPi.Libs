using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace RaspberryPi.MMAL.Interop
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct MMAL_PARAMETER_CAMERA_INFO_T
    {
        public MMAL_PARAMETER_HEADER_T hdr;
        [MarshalAs(UnmanagedType.U4)]
        public uint num_cameras;
        [MarshalAs(UnmanagedType.U4)]
        public uint num_flashes;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = NativeMethods.MMAL_PARAMETER_CAMERA_INFO_MAX_CAMERAS)]
        public MMAL_PARAMETER_CAMERA_INFO_CAMERA_T[] cameras;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = NativeMethods.MMAL_PARAMETER_CAMERA_INFO_MAX_FLASHES)]
        public MMAL_PARAMETER_CAMERA_INFO_FLASH_T[] flashes;
    }
}
