using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace OpenGlToMatrix.SimpleGLES
{
    public static class GL
    {
        //const string LIB = "libGLESv2";
        public const string LIB_GLES = "libbrcmGLESv2.so";
        //const string LIB = "brcmGLESv2";

        [DllImport(LIB_GLES, EntryPoint = "glGetError", CallingConvention = CallingConvention.Cdecl)]
        public static extern ErrorCode GetError();

        [DllImport(LIB_GLES, EntryPoint = "glClearColor", CallingConvention = CallingConvention.Cdecl)]
        public static extern void ClearColor(float r, float g, float b, float a);

        //[DllImport(LIB, EntryPoint = "glEnable", CallingConvention = CallingConvention.Cdecl)]
        //public static extern void Enable(int cap);

        [DllImport(LIB_GLES, EntryPoint = "glEnable", CallingConvention = CallingConvention.Cdecl)]
        public static extern void Enable(EnableCap cap);

        [DllImport(LIB_GLES, EntryPoint = "glMatrixMode", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MatrixMode(MatrixMode mode);

        [DllImport(LIB_GLES, EntryPoint = "glHint", CallingConvention = CallingConvention.Cdecl)]
        public static extern void Hint(HintTarget target, HintMode mode);

        [DllImport(LIB_GLES, EntryPoint = "glViewport", CallingConvention = CallingConvention.Cdecl)]
        public static extern void Viewport(int x, int y, int width, int height);

        [DllImport(LIB_GLES, EntryPoint = "glLoadIdentity", CallingConvention = CallingConvention.Cdecl)]
        public static extern void LoadIdentity();

        [DllImport(LIB_GLES, EntryPoint = "glFrustumf", CallingConvention = CallingConvention.Cdecl)]
        public static extern void Frustumf(float left, float right, float bottom, float top, float zNear, float zFar);

        [DllImport(LIB_GLES, EntryPoint = "glEnableClientState", CallingConvention = CallingConvention.Cdecl)]
        public static extern void EnableClientState(EnableCap array);

        [DllImport(LIB_GLES, EntryPoint = "glVertexPointer", CallingConvention = CallingConvention.Cdecl)]
        public static extern void VertexPointer(int size, VertexPointerType type, int stride, IntPtr pointer);

        [DllImport(LIB_GLES, EntryPoint = "glTranslatef", CallingConvention = CallingConvention.Cdecl)]
        public static extern void Translatef(float x, float y, float z);

        [DllImport(LIB_GLES, EntryPoint = "glGenTextures", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GenTextures(int n, IntPtr textures);
        [DllImport(LIB_GLES, EntryPoint = "glGenTextures", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void GenTextures(int n, uint* textures);

        [DllImport(LIB_GLES, EntryPoint = "glBindTexture", CallingConvention = CallingConvention.Cdecl)]
        public static extern void BindTexture(TextureTarget target, uint texture);

        [DllImport(LIB_GLES, EntryPoint = "glTexImage2D", CallingConvention = CallingConvention.Cdecl)]
        public static extern void TexImage2D(TextureTarget target, int level, PixelInternalFormat internalFormat, int width, int height, int border, PixelFormat format, PixelType type, IntPtr data);

        [DllImport(LIB_GLES, EntryPoint = "glTexParameterf", CallingConvention = CallingConvention.Cdecl)]
        public static extern void TexParameterf(TextureTarget target, TextureParameterName pname, float param);

        [DllImport(LIB_GLES, EntryPoint = "glTexCoordPointer", CallingConvention = CallingConvention.Cdecl)]
        public static extern void TexCoordPointer(int size, TexCoordPointerType type, int stride, IntPtr pointer);

        [DllImport(LIB_GLES, EntryPoint = "glRotatef", CallingConvention = CallingConvention.Cdecl)]
        public static extern void Rotatef(float angle, float x, float y, float z);

        [DllImport(LIB_GLES, EntryPoint = "glClear", CallingConvention = CallingConvention.Cdecl)]
        public static extern void Clear(ClearBufferMask mask);

        [DllImport(LIB_GLES, EntryPoint = "glDrawArrays", CallingConvention = CallingConvention.Cdecl)]
        public static extern void DrawArrays(PrimitiveType mode, int first, int count);
    }
}
