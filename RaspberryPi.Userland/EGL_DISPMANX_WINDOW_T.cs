using RaspberryPi.Userland.Interop;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace RaspberryPi.Userland
{
    [StructLayout(LayoutKind.Sequential)]
    public struct EGL_DISPMANX_WINDOW_T
    {
        public EGL_DISPMANX_WINDOW_T(Element element, int width, int height)
        {
            this.element = element.Handle;
            this.width = width;
            this.height = height;
        }

        private DISPMANX_ELEMENT_HANDLE_T element;
        private int width;
        private int height;

        public int Width => this.width;
        public int Height => this.height;
    }
}
