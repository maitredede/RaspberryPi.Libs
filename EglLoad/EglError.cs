using System;
using System.Collections.Generic;
using System.Text;

namespace EglLoad
{
    public enum EglError
    {
        EGL_SUCCESS = 0x3000,
        EGL_NOT_INITIALIZED = 0x3001,
        EGL_BAD_ACCESS = 0x3002,
        EGL_BAD_ALLOC = 0x3003,
        EGL_BAD_ATTRIBUTE = 0x3004,
        EGL_BAD_CONFIG = 0x3005,
        EGL_BAD_CONTEXT = 0x3006,
        EGL_BAD_CURRENT_SURFACE = 0x3007,
        EGL_BAD_DISPLAY = 0x3008,
        EGL_BAD_MATCH = 0x3009,
        EGL_BAD_NATIVE_PIXMAP = 0x300A,
        EGL_BAD_NATIVE_WINDOW = 0x300B,
        EGL_BAD_PARAMETER = 0x300C,
        EGL_BAD_SURFACE = 0x300D,
        EGL_CONTEXT_LOST = 0x300E,
    }
}
