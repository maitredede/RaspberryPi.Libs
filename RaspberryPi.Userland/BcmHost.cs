using RaspberryPi.Userland.MMAL;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using static RaspberryPi.Userland.Interop.BcmHostNativeMethods;

namespace RaspberryPi.Userland
{
    public sealed class BcmHost : DisposableSingleton<BcmHost>
    {
        private readonly DisplayManager m_dispman;
        private readonly MMALManager m_mmal;

        public DisplayManager Dispman => this.m_dispman;
        public MMALManager MMAL => this.m_mmal;

        public BcmHost()
        {
            BcmHostInit();

            this.m_dispman = new DisplayManager();
            this.m_mmal = new MMALManager();
        }

        public static string GenCmd(string command)
        {
            int length = 4096;
            byte[] response = new byte[length];
            GCHandle respHandle = GCHandle.Alloc(response, GCHandleType.Pinned);
            try
            {
                IntPtr ptr = respHandle.AddrOfPinnedObject();
                int result = VcGencmd(ptr, length, command);
                if (result != 0)
                    throw new InvalidOperationException("Failed to call vc_gencmd");
            }
            finally
            {
                respHandle.Free();
            }
            return Encoding.UTF8.GetString(response).TrimEnd('\0');
        }

        protected override void Dispose(bool disposing)
        {
            BcmHostDeinit();

            base.Dispose(disposing);
        }
    }
}
