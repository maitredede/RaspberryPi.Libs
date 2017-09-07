using RaspberryPi.MMAL.Interop;
using System;
using System.Collections.Generic;
using System.Text;

namespace RaspberryPi.MMAL
{
    public struct StereoScopicMode
    {
        public MMAL_STEREOSCOPIC_MODE_T Mode { get; set; }
        public bool Decimate { get; set; }
        public bool SwapEyes { get; set; }

        public static StereoScopicMode Default = new StereoScopicMode { Mode = MMAL_STEREOSCOPIC_MODE_T.MMAL_STEREOSCOPIC_MODE_NONE, Decimate = false, SwapEyes = false };
    }
}
