using RaspberryPi.WiringPi.Interop;
using System;

namespace RaspberryPi.WiringPi
{
    public static class WiringPi
    {
        public static Version Version()
        {
            NativeMethods.version(out int major, out int minor);
            return new Version(major, minor);
        }
    }
}
