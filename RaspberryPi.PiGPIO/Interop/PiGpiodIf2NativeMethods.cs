using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace RaspberryPi.PiGPIO.Interop
{
    internal static class PiGpiodIf2NativeMethods
    {
        public const string LIB = "libpigpiod_if2";

        [DllImport(LIB, EntryPoint = "pigpio_start", CallingConvention = CallingConvention.Cdecl)]
        public static extern short Start(string addr, string port);

        [DllImport(LIB, EntryPoint = "pigpio_stop", CallingConvention = CallingConvention.Cdecl)]
        public static extern void Stop(short pi);

        [DllImport(LIB, EntryPoint = "get_hardware_revision", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint HardwareRevision(short pi);

        [DllImport(LIB, EntryPoint = "get_pigpio_version", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint PiGpioVersion(short pi);
    }
}
