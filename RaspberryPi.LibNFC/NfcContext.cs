using RaspberryPi.LibNFC.Interop;
using System;
using System.Runtime.InteropServices;

namespace RaspberryPi.LibNFC
{
    /// <summary>
    /// NFC context
    /// </summary>
    public sealed class NfcContext : RaspberryPi.Interop.NativeObject, INfcContext
    {
        /// <summary>
        /// Create a new NFC context
        /// </summary>
        public NfcContext() : base(Init(), true)
        {
        }

        private static IntPtr Init()
        {
            NativeMethods.init(out IntPtr ctx);
            if (ctx == IntPtr.Zero)
                throw new OutOfMemoryException("Can't initialize libnfc");
            return ctx;
        }

        protected override void FreeHandle()
        {
            NativeMethods.exit(this.handle);
        }

        private static string _version;

        /// <summary>
        /// Version of libnfc
        /// </summary>
        public static string Version
        {
            get
            {
                if (_version == null)
                {
                    IntPtr ptr = NativeMethods.version();
                    _version = Marshal.PtrToStringAnsi(ptr);
                }
                return _version;
            }
        }

        /// <summary>
        /// Open a devive by its connectionstring, on null for default device
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public NfcDevice Open(string connectionString)
        {
            IntPtr ptr = NativeMethods.open(this.handle, connectionString);
            if (ptr == IntPtr.Zero)
                return null;
            return new NfcDevice(ptr);
        }

        /// <summary>
        /// List devices
        /// </summary>
        /// <returns>Array of strings, the connection strings of the devices</returns>
        public string[] ListDevices()
        {
            const int MAX_DEVICE = 8;
            const int LENGTH = MAX_DEVICE * NativeMethods.NFC_BUFSIZE_CONNSTRING;

            IntPtr ptr = Marshal.AllocHGlobal(LENGTH);
            try
            {
                for (int i = 0; i < LENGTH; i++)
                {
                    Marshal.WriteByte(ptr, i, 0);
                }

                int size = NativeMethods.list_devices(this.handle, ptr, MAX_DEVICE);
                string[] arr = new string[size];
                for (int i = 0; i < size; i++)
                {
                    arr[i] = Marshal.PtrToStringAnsi(ptr + i * NativeMethods.NFC_BUFSIZE_CONNSTRING);
                }
                return arr;
            }
            finally
            {
                if (ptr != IntPtr.Zero)
                    Marshal.FreeHGlobal(ptr);
            }
        }
    }
}
