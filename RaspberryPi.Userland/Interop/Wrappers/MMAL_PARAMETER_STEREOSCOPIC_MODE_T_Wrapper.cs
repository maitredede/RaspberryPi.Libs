using RaspberryPi.Userland.Interop;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace RaspberryPi.Userland.Interop.Wrappers
{
    internal sealed class MMAL_PARAMETER_STEREOSCOPIC_MODE_T_Wrapper : BaseParameter<MMAL_PARAMETER_STEREOSCOPIC_MODE_T>
    {
        public MMAL_STEREOSCOPIC_MODE_T mode
        {
            get { return (MMAL_STEREOSCOPIC_MODE_T)this.Handler.ReadInt32(); }
            set { this.Handler.WriteInt32((int)value); }
        }

        public bool decimate
        {
            get { return this.Handler.ReadInt32() != 0; }
            set { this.Handler.WriteInt32(value ? 1 : 0); }
        }

        public bool swap_eyes
        {
            get { return this.Handler.ReadInt32() != 0; }
            set { this.Handler.WriteInt32(value ? 1 : 0); }
        }
    }
}
