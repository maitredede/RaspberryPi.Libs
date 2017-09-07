using System;
using System.Collections.Generic;
using System.Text;
using RaspberryPi.Userland.Interop;

namespace RaspberryPi.Userland
{
    public sealed class Alpha
    {
        private readonly VC_DISPMANX_ALPHA_T m_native;
        private Resource m_mask;

        public Alpha()
        {
            this.m_native = new VC_DISPMANX_ALPHA_T();
        }

        internal VC_DISPMANX_ALPHA_T Native => this.m_native;

        public DISPMANX_FLAGS_ALPHA_T Flags { get { return this.m_native.flags; } set { this.m_native.flags = value; } }
        public int Opacity { get { return (int)this.m_native.opacity; } set { this.m_native.opacity = (uint)value; } }
        public Resource Mask
        {
            get { return this.m_mask; }
            set
            {
                this.m_mask = value;
                this.m_native.mask.Handle = this.m_mask?.Handle.Handle ?? IntPtr.Zero;
            }
        }

    }
}
