using RaspberryPi.Userland.Interop;
using System;
using System.Collections.Generic;
using System.Text;

namespace RaspberryPi.Userland
{
    public sealed class Update
    {
        private bool m_submited;
        private readonly DISPMANX_UPDATE_HANDLE_T m_handle;

        internal DISPMANX_UPDATE_HANDLE_T Handle { get { return this.m_handle; } }

        internal Update(DISPMANX_UPDATE_HANDLE_T handle)
        {
            this.m_handle = handle;
        }

        public void SubmitSync()
        {
            if (this.m_submited)
                throw new InvalidOperationException();
            var ret = DispmanxNativeMethods.UpdateSubmitSync(this.m_handle);
            if (ret != DISPMANX_STATUS_T.SUCCESS)
                throw new DispmanException($"Operation failed : {nameof(DispmanxNativeMethods.UpdateSubmitSync)}");
            this.m_submited = true;
        }
    }
}
