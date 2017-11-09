using OpenGlToMatrix.SimpleGLES.Interop;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace OpenGlToMatrix.SimpleGLES
{
    public sealed class EglDisplay : IDisposable
    {
        private readonly IntPtr m_ptr;
        private readonly IntPtr m_config;

        internal IntPtr Ptr => this.m_ptr;
        internal IntPtr Config => this.m_config;

        public EglDisplay()
        {
            this.m_ptr = NativeMethods.eglGetDisplay(0);
            if (this.m_ptr == IntPtr.Zero)
            {
                throw new Exception("Can't get display: " + NativeMethods.eglGetError());
            }

            bool init = NativeMethods.eglInitialize(this.m_ptr, out int major, out int minor);
            if (!init)
            {
                throw new Exception("Can't initialize display: " + NativeMethods.eglGetError());
            }

            int[] attribute_list =
            {
                EGL_RED_SIZE, 8,
                EGL_GREEN_SIZE, 8,
                EGL_BLUE_SIZE, 8,
                EGL_ALPHA_SIZE, 8,
                EGL_SURFACE_TYPE, EGL_WINDOW_BIT,
                EGL_NONE
            };
            int sizeConfig = 10;
            IntPtr ptrConfig = Marshal.AllocHGlobal(IntPtr.Size * sizeConfig);
            try
            {
                bool config = NativeMethods.eglChooseConfig(this.m_ptr, attribute_list, ptrConfig, sizeConfig, out int numConfig);
                if (!config)
                {
                    throw new Exception("Can't choose config: " + NativeMethods.eglGetError());
                }
                if (numConfig == 0)
                {
                    throw new Exception("No configuration returned");
                }
                this.m_config = Marshal.ReadIntPtr(ptrConfig);
            }
            finally
            {
                Marshal.FreeHGlobal(ptrConfig);
            }
        }

        public void Dispose()
        {
            bool term = NativeMethods.eglTerminate(this.m_ptr);
            if (!term)
            {
                throw new Exception("Can't terminate: " + NativeMethods.eglGetError());
            }
        }

        const int EGL_ALPHA_SIZE = 0x3021;
        const int EGL_BLUE_SIZE = 0x3022;
        const int EGL_GREEN_SIZE = 0x3023;
        const int EGL_RED_SIZE = 0x3024;
        const int EGL_SURFACE_TYPE = 0x3033;
        const int EGL_WINDOW_BIT = 0x0004;
        const int EGL_NONE = 0x3038;

        public void MakeCurrent(EglContext context, EglSurface surface)
        {
            bool make = NativeMethods.eglMakeCurrent(this.m_ptr, surface.Ptr, surface.Ptr, context.Ptr);
            if (!make)
            {
                throw new Exception("Can't make current: " + NativeMethods.eglGetError());
            }
        }
    }
}
