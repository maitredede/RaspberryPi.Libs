using System;
using System.Collections.Generic;
using System.Text;
using DISPLAY_INPUT_FORMAT_T = RaspberryPi.Userland.Interop.VCOS_DISPLAY_INPUT_FORMAT_T;

namespace RaspberryPi.Userland.Interop
{
    internal struct DISPMANX_MODEINFO_T
    {
        public int width;
        public int height;
        public DISPMANX_TRANSFORM_T transform;
        public DISPLAY_INPUT_FORMAT_T input_format;
        public uint display_num;
    }
}
