using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace RaspberryPi.Interop{
    public static partial class NativeMethods{
        public const string LIB_DL = "libdl";
        
        [DllImport(LIB_DL, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr dlopen(string filename, RTLD flags);
        [DllImport(LIB_DL, CallingConvention = CallingConvention.Cdecl)]
        public static extern string dlerror();

        public static IntPtr LoadLib(string name)
        {
            IntPtr ptr = dlopen(name, RTLD.RTLD_NOW | RTLD.RTLD_GLOBAL);
            if (ptr == IntPtr.Zero)
            {
                string msg = dlerror() ?? "nil";
                Console.Error.WriteLine("dlopen '{0}' failed: {1}", name, msg);
            }
            return ptr;
        }
    }
}