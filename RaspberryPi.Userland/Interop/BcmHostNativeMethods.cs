using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace RaspberryPi.Userland.Interop
{
    internal static class BcmHostNativeMethods
    {
        public const string LIB = "libbcm_host.so";

        [DllImport(LIB, EntryPoint = "bcm_host_init", CallingConvention = CallingConvention.Cdecl)]
        public static extern void BcmHostInit();

        [DllImport(LIB, EntryPoint = "bcm_host_deinit", CallingConvention = CallingConvention.Cdecl)]
        public static extern void BcmHostDeinit();

        //int32_t graphics_get_display_size( const uint16_t display_number,
        //                                            uint32_t *width,
        //                                            uint32_t* height);
        [DllImport(LIB, EntryPoint = "graphics_get_display_size", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GraphicsGetDisplaySize(ushort displayNumber, out uint width, out uint height);

        [DllImport(LIB, EntryPoint = "vc_gencmd", CallingConvention = CallingConvention.Cdecl)]
        public static extern int VcGencmd(IntPtr response, int responseSize, string command);
    }
}
