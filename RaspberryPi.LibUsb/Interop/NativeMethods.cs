using System;
using System.Runtime.InteropServices;

namespace RaspberryPi.LibUsb.Interop
{
    public static class NativeMethods
    {
        public const string LIB = "libusb-1.0";

        /// <summary>
        /// Initialize libusb. This function must be called before calling any other libusb function.
        /// If you do not provide an output location for a context pointer, a default context will be created. If there was already a default context, it will be reused (and nothing will be initialized/reinitialized).
        /// </summary>
        /// <param name="pContext">Optional output location for context pointer. Only valid on return code 0.</param>
        /// <returns>0 on success, or a LIBUSB_ERROR code on failure</returns>
        [DllImport(LIB, CallingConvention = CallingConvention.Cdecl, EntryPoint = "libusb_init")]
        public static extern LibUsbError init(ref IntPtr pContext);

        /// <summary>
        /// Deinitialize libusb. Should be called after closing all open devices and before your application terminates.
        /// </summary>
        /// <param name="pContext">the context to deinitialize, or NULL for the default context</param>
        [DllImport(LIB, CallingConvention = CallingConvention.Cdecl, EntryPoint = "libusb_exit")]
        internal static extern void exit(IntPtr pContext);
    }
}