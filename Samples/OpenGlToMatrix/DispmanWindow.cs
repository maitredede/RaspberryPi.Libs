using System;
using System.Collections.Generic;
using System.Text;
using RaspberryPi.Userland;
using OpenGlToMatrix.SimpleGLES;
using System.Runtime.InteropServices;

namespace OpenGlToMatrix
{
    internal sealed class DispmanWindow : INativeWindow, IDisposable
    {
        private readonly Element m_element;
        private readonly int m_width;
        private readonly int m_height;

        private readonly EGL_DISPMANX_WINDOW_T m_struct;
        private readonly GCHandle m_handle;

        public IntPtr Handle => this.m_handle.AddrOfPinnedObject();

        public DispmanWindow(Element element, int width, int height)
        {
            this.m_element = element;
            this.m_width = width;
            this.m_height = height;

            this.m_struct = new EGL_DISPMANX_WINDOW_T(element, width, height);
            this.m_handle = GCHandle.Alloc(this.m_struct, GCHandleType.Pinned);
        }

        public void Dispose()
        {
            this.m_handle.Free();
        }
    }
}
