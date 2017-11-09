using OpenGlToMatrix.SimpleGLES.Interop;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenGlToMatrix.SimpleGLES
{
    public sealed class EglSurface : IDisposable
    {
        private readonly EglDisplay m_display;
        private readonly IntPtr m_ptr;

        internal IntPtr Ptr => this.m_ptr;

        public EglSurface(EglDisplay display, INativeWindow nativeWindow)
        {
            this.m_display = display;
            this.m_ptr = NativeMethods.eglCreateWindowSurface(this.m_display.Ptr, this.m_display.Config, nativeWindow.Handle, IntPtr.Zero);
            if (this.m_ptr == IntPtr.Zero)
            {
                throw new Exception("Can't create surface: " + NativeMethods.eglGetError());
            }
        }

        public void Dispose()
        {
            bool dest = NativeMethods.eglDestroySurface(this.m_display.Ptr, this.m_ptr);
            if (!dest)
            {
                throw new Exception("Can't destroy surface: " + NativeMethods.eglGetError());
            }
        }

        public void SwapBuffers()
        {
            bool swap = NativeMethods.eglSwapBuffers(this.m_display.Ptr, this.m_ptr);
            if (!swap)
            {
                throw new Exception("Can't swap buffers: " + NativeMethods.eglGetError());
            }
        }
    }
}
