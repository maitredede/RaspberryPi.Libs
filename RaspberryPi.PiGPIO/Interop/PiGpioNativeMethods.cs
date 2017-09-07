﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace PiGPIO.Interop
{
    internal static class PiGpioNativeMethods
    {
        public const string LIB = "libpigpio";

        [DllImport(LIB, EntryPoint = "gpioHardwareRevision", CallingConvention = CallingConvention.Cdecl)]
        public static extern ushort HardwareRevision();

        [DllImport(LIB, EntryPoint = "gpioVersion", CallingConvention = CallingConvention.Cdecl)]
        public static extern ushort Version();

        #region Essential
        [DllImport(LIB, EntryPoint = "gpioInitialise", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Initialise();

        [DllImport(LIB, EntryPoint = "gpioTerminate", CallingConvention = CallingConvention.Cdecl)]
        public static extern void Terminate();
        #endregion

        #region Beginner
        [DllImport(LIB, EntryPoint = "gpioSetMode", CallingConvention = CallingConvention.Cdecl)]
        public static extern short SetMode(ushort gpio, ushort mode);

        [DllImport(LIB, EntryPoint = "gpioGetMode", CallingConvention = CallingConvention.Cdecl)]
        public static extern short GetMode(ushort gpio);

        [DllImport(LIB, EntryPoint = "gpioSetPullUpDown", CallingConvention = CallingConvention.Cdecl)]
        public static extern short SetPullUpDown(ushort gpio, ushort pud);

        [DllImport(LIB, EntryPoint = "gpioSetAlertFunc", CallingConvention = CallingConvention.Cdecl)]
        public static extern short SetAlertFunc(ushort user_gpio, [MarshalAs(UnmanagedType.FunctionPtr)] gpioAlertFunc_t f);

        [DllImport(LIB, EntryPoint = "gpioRead", CallingConvention = CallingConvention.Cdecl)]
        public static extern short Read(ushort user_gpio);

        [DllImport(LIB, EntryPoint = "gpioWrite", CallingConvention = CallingConvention.Cdecl)]
        public static extern short Write(ushort user_gpio, ushort level);
        #endregion

        #region Advanced
        [DllImport(LIB, EntryPoint = "gpioNoiseFilter", CallingConvention = CallingConvention.Cdecl)]
        public static extern short NoiseFilter(ushort gpio, ushort steady, ushort active);
        #endregion

        #region I2C
        [DllImport(LIB, EntryPoint = "i2cOpen", CallingConvention = CallingConvention.Cdecl)]
        public static extern short I2COpen(ushort bus, ushort address, ushort flags);

        [DllImport(LIB, EntryPoint = "i2cClose", CallingConvention = CallingConvention.Cdecl)]
        public static extern short I2CClose(ushort handle);

        [DllImport(LIB, EntryPoint = "i2cReadDevice", CallingConvention = CallingConvention.Cdecl)]
        public static extern short i2cReadDevice(ushort handle, IntPtr buf, ushort count);
        #endregion

        #region SPI
        [DllImport(LIB, EntryPoint = "spiOpen", CallingConvention = CallingConvention.Cdecl)]
        public static extern short spiOpen(ushort spiCHan, ushort baud, ushort spiFlags);

        [DllImport(LIB, EntryPoint = "spiClose", CallingConvention = CallingConvention.Cdecl)]
        public static extern short spiClose(ushort handle);

        [DllImport(LIB, EntryPoint = "spiRead", CallingConvention = CallingConvention.Cdecl)]
        public static extern short spiRead(ushort handle, IntPtr buf, ushort count);

        [DllImport(LIB, EntryPoint = "spiWrite", CallingConvention = CallingConvention.Cdecl)]
        public static extern short spiWrite(ushort handle, IntPtr buf, ushort count);

        [DllImport(LIB, EntryPoint = "spiXfer", CallingConvention = CallingConvention.Cdecl)]
        public static extern short spiXfer(ushort handle, IntPtr txBuf, IntPtr rxBuf, ushort count);
        #endregion
    }
}
