using RaspberryPi.Userland.Interop;
using System;
using System.Collections.Generic;
using System.Text;

namespace RaspberryPi.Userland
{
    public sealed class Element
    {
        private readonly DISPMANX_ELEMENT_HANDLE_T m_handle;

        internal DISPMANX_ELEMENT_HANDLE_T Handle => this.m_handle;

        internal Element(DISPMANX_ELEMENT_HANDLE_T handle)
        {
            this.m_handle = handle;
        }
    }
}
