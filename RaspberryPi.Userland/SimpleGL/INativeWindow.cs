using System;
using System.Collections.Generic;
using System.Text;

namespace RaspberryPi.Userland.SimpleGL
{
    public interface INativeWindow
    {
        IntPtr Handle { get; }
    }
}
