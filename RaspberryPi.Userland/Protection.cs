using RaspberryPi.Userland.Interop;
using System;
using System.Collections.Generic;
using System.Text;

namespace RaspberryPi.Userland
{
    public sealed class Protection
    {
        private DISPMANX_PROTECTION_T m_handle;

        internal DISPMANX_PROTECTION_T Handle { get { return this.m_handle; } }

        internal Protection(DISPMANX_PROTECTION_T handle)
        {
            this.m_handle = handle;
        }

        public static readonly Protection None = new Protection(new DISPMANX_PROTECTION_T { Handle = IntPtr.Zero });
        public static readonly Protection HDCP = new Protection(new DISPMANX_PROTECTION_T { Handle = new IntPtr(11) });
    }
}
