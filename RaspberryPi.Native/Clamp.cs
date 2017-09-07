using RaspberryPi.Native;
using System;
using System.Collections.Generic;
using System.Text;

namespace RaspberryPi
{
    public sealed class Clamp
    {
        private readonly DISPMANX_CLAMP_T m_clamp;

        internal DISPMANX_CLAMP_T Native { get { return this.m_clamp; } }

        public Clamp()
        {
            this.m_clamp = new DISPMANX_CLAMP_T();
        }
    }
}
