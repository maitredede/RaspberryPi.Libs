using System;
using System.Collections.Generic;
using System.Text;

namespace RaspberryPi.Userland.Interop.Wrappers
{
    internal sealed class MMAL_PARAMETER_INT32_T_Wrapper : BaseParameter<MMAL_PARAMETER_INT32_T>
    {
        public int value
        {
            get { return this.Handler.ReadInt32(); }
            set { this.Handler.WriteInt32(value); }
        }
    }
}
