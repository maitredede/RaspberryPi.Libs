using RaspberryPi.LibFreefare.Interop;
using RaspberryPi.LibNFC;
using System;
using System.Collections.Generic;
using System.Text;

namespace RaspberryPi.LibFreefare
{
    public sealed class NTAG21xTag : FreefareTag
    {
        private NTagSubtype? m_subtype;

        internal NTAG21xTag(IntPtr ptr, bool dispose, NfcDevice device, NfcTarget target) : base(ptr, dispose, device, target)
        {
            int code = NativeMethods.ntag21x_get_info(ptr);
            if (code < 0)
            {
                throw new FreefareException(string.Format("Can't read tag info. ret={0} lastError={1}", code, this.LastError()));
            }
        }

        public NTagSubtype SubType
        {
            get
            {
                if (!this.m_subtype.HasValue)
                {
                    this.m_subtype = NativeMethods.ntag21x_get_subtype(this.Handle);
                }
                return this.m_subtype.Value;
            }
        }

        public NTAG21xConnection Connect()
        {
            int ret = NativeMethods.ntag21x_connect(this.Handle);
            if (ret < 0)
                throw new FreefareException("Error connecting to tag: " + this.LastError());
            return new NTAG21xConnection(this);
        }
    }
}
