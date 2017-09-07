using RaspberryPi.Userland.MMAL;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace RaspberryPi.Userland.Interop
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct MMAL_PARAMETER_HEADER_T
    {
        [MarshalAs(UnmanagedType.U4)]
        public MMALParameterId  id;      /**< Parameter ID. */
        [MarshalAs(UnmanagedType.U4)]
        public uint size;    /**< Size in bytes of the parameter (including the header) */
    }
}
