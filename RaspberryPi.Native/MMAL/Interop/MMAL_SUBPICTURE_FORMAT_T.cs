using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace RaspberryPi.MMAL.Interop
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct MMAL_SUBPICTURE_FORMAT_T
    {
        public uint x_offset;        /**< Width offset to the start of the subpicture */
        public uint y_offset;        /**< Height offset to the start of the subpicture */
    }
}
