using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace RaspberryPi.WiringPi.Interop
{
    internal static class NativeMethods
    {
        public const string LIB = "libwiringpi";

        [DllImport(LIB, EntryPoint = "wiringPiVersion", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void version(out int major, out int minor);

        [DllImport(LIB, EntryPoint = "wiringPiSetup", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int setup();

        [DllImport(LIB, EntryPoint = "wiringPiSetupSys", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int setupSys();

        [DllImport(LIB, EntryPoint = "wiringPiSetupGpio", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int setupGpio();

        [DllImport(LIB, EntryPoint = "wiringPiSetupPhys", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int setupPhys();

    }
}
