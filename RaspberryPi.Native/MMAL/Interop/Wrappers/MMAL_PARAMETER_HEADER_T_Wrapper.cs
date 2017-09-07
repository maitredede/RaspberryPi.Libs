using RaspberryPi.Interop;
using System;
using System.Collections.Generic;
using System.Text;

namespace RaspberryPi.MMAL.Interop.Wrappers
{
    internal sealed class MMAL_PARAMETER_HEADER_T_Wrapper
    {
        private readonly InteropHandler<MMAL_PARAMETER_HEADER_T> m_handler;

        public MMAL_PARAMETER_HEADER_T_Wrapper(Func<IntPtr> ptrGetter)
        {
            this.m_handler = new InteropHandler<MMAL_PARAMETER_HEADER_T>(ptrGetter);
        }

        public IntPtr GetPtr()
        {
            return this.m_handler.GetPtr();
        }

        public void DumpOffsets()
        {
            this.m_handler.Dump();
        }

        public MMALParameterId id
        {
            get { return (MMALParameterId)this.m_handler.ReadUInt32(nameof(MMAL_PARAMETER_HEADER_T.id)); }
            set { this.m_handler.WriteUInt32((uint)value, nameof(MMAL_PARAMETER_HEADER_T.id)); }
        }

        public uint size
        {
            get { return this.m_handler.ReadUInt32(nameof(MMAL_PARAMETER_HEADER_T.size)); }
            set { this.m_handler.WriteUInt32(value, nameof(MMAL_PARAMETER_HEADER_T.size)); }
        }
    }
}
