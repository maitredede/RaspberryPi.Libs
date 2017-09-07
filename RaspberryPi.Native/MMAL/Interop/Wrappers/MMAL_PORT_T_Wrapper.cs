using RaspberryPi.Interop;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace RaspberryPi.MMAL.Interop.Wrappers
{
    internal sealed class MMAL_PORT_T_Wrapper
    {
        public static readonly int Size = Marshal.SizeOf<MMAL_PORT_T>();

        private readonly InteropHandler<MMAL_PORT_T> m_handler;

        internal IntPtr Ptr() => this.m_handler.GetPtr();

        internal MMAL_PORT_T_Wrapper(Func<IntPtr> ptrGetter)
        {
            this.m_handler = new InteropHandler<MMAL_PORT_T>(ptrGetter);
        }

        public string name { get { return this.m_handler.ReadString(); } }
    }
}
