using System;
using System.Runtime.InteropServices;
using RaspberryPi.LibUsb.Interop;

namespace RaspberryPi.LibUsb
{
    public class LibUsbContext : RaspberryPi.Interop.NativeObject
    {
         /// <summary>
        /// Create a new NFC context
        /// </summary>
        public LibUsbContext() : base(Init(), true)
        {
        }

        protected override void FreeHandle()
        {
            NativeMethods.exit(this.handle);
        }


        private static IntPtr Init()
        {
            IntPtr ctx = IntPtr.Zero;
            LibUsbError error = NativeMethods.init(ref ctx);
            if (error != LibUsbError.Success)
                throw new LibUsbException("Can't initialize libusb", error);
            return ctx;
        }
    }
}