using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace RaspberryPi.RgbLedMatrix.Interop
{
    internal static class NativeMethods
    {
        public const string LIB = "librgbmatrix";

        #region Matrix
        [DllImport(LIB, EntryPoint = "led_matrix_create", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr MatrixCreate(int rows, int chained, int parallel);

        [DllImport(LIB, EntryPoint = "led_matrix_delete", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MatrixDelete(IntPtr handle);
        #endregion
    }
}
