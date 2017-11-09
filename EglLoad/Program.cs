using RaspberryPi.Userland;
using System;
using System.Runtime.InteropServices;

namespace EglLoad
{
    class Program
    {
        public const string LIB_EGL = "libbrcmEGL";

        [DllImport(LIB_EGL, EntryPoint = "eglGetDisplay", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr eglGetDisplay(int display);

        [DllImport(LIB_EGL, EntryPoint = "eglGetError", CallingConvention = CallingConvention.Cdecl)]
        public static extern int eglGetError();

        [DllImport(LIB_EGL, EntryPoint = "eglInitialize", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool eglInitialize(IntPtr display, out int major, out int minor);

        [DllImport(LIB_EGL, EntryPoint = "eglTerminate", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool eglTerminate(IntPtr display);

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            using (BcmHost host = new BcmHost())
            {
                Console.WriteLine("host ok");


                IntPtr display = eglGetDisplay(0);

                Console.WriteLine("Display ptr {0}", display);
            }
        }
    }
}
