using OpenGlToMatrix.SimpleGLES.Interop;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenGlToMatrix.SimpleGLES
{
    public sealed class EglContext : IDisposable
    {
        private readonly IntPtr m_ptr;
        private readonly EglDisplay m_display;

        internal IntPtr Ptr => this.m_ptr;

        public EglContext(EglDisplay display)
        {
            this.m_display = display;

            this.m_ptr = NativeMethods.eglCreateContext(this.m_display.Ptr, display.Config, IntPtr.Zero, IntPtr.Zero);
            if (this.m_ptr == IntPtr.Zero)
            {
                throw new Exception("Can't create context: " + NativeMethods.eglGetError());
            }
        }

        public void Dispose()
        {
            bool term = NativeMethods.eglDestroyContext(this.m_display.Ptr, this.m_ptr);
            if (!term)
            {
                throw new Exception("Can't destroy context: " + NativeMethods.eglGetError());
            }
        }

        public void MakeCurrent(EglSurface surface)
        {
            this.m_display.MakeCurrent(this, surface);
        }
    }
}
