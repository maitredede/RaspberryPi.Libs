using System;
using System.Collections.Generic;
using System.Text;

namespace RaspberryPi.LibFreefare
{
    internal sealed class GenericTag : FreefareTag
    {
        internal GenericTag(IntPtr ptr, bool dispose) : base(ptr, dispose)
        {
        }
    }
}
