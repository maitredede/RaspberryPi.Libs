using System;
using System.Collections.Generic;
using System.Text;

namespace RaspberryPi.LibFreefare
{
    public sealed class NTAG21xTag : FreefareTag
    {
        internal NTAG21xTag(IntPtr ptr, bool dispose) : base(ptr, dispose)
        {
        }
    }
}
