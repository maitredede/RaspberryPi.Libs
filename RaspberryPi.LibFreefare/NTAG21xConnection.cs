using RaspberryPi.LibFreefare.Interop;
using System;
using System.Collections.Generic;
using System.Text;

namespace RaspberryPi.LibFreefare
{
    public sealed class NTAG21xConnection : IDisposable
    {
        private readonly NTAG21xTag m_tag;

        internal NTAG21xConnection(NTAG21xTag tag)
        {
            this.m_tag = tag;
        }

        public void Dispose()
        {
            int code = NativeMethods.ntag21x_disconnect(this.m_tag.Handle);
            if (code < 0)
                throw new FreefareException("Error when disconnecting from tag: " + this.m_tag.LastError());
        }
    }
}
