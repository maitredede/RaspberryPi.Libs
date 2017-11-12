using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace RaspberryPi.LibLedMatrix.Interop
{
    internal static class NativeMethods
    {
        public const string LIB = "librgbmatrix";

        [DllImport(LIB, EntryPoint = "led_matrix_create", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr led_matrix_create(int rows, int chained, int parallel);

        [DllImport(LIB, EntryPoint = "led_matrix_create_from_options", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr led_matrix_create_from_options(IntPtr options, IntPtr argc, out IntPtr argv);

        [DllImport(LIB, EntryPoint = "led_matrix_delete", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void led_matrix_delete(IntPtr matrix);

        [DllImport(LIB, EntryPoint = "led_matrix_update_options", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool led_matrix_update_options(IntPtr matrix, IntPtr options);

        [DllImport(LIB, EntryPoint = "led_matrix_create_offscreen_canvas", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr led_matrix_create_offscreen_canvas(IntPtr matrix);

        [DllImport(LIB, EntryPoint = "led_matrix_swap_on_vsync", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr led_matrix_swap_on_vsync(IntPtr matrix, IntPtr canvas);

        [DllImport(LIB, EntryPoint = "led_matrix_get_canvas", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr led_matrix_get_canvas(IntPtr matrix);

        [DllImport(LIB, EntryPoint = "led_canvas_get_size", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void led_canvas_get_size(IntPtr canvas, out int width, out int height);

        [DllImport(LIB, EntryPoint = "led_canvas_clear", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void led_canvas_clear(IntPtr canvas);

        [DllImport(LIB, EntryPoint = "led_canvas_set_pixel", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void led_canvas_set_pixel(IntPtr canvas, int x, int y, byte r, byte g, byte b);

        [DllImport(LIB, EntryPoint = "led_canvas_fill", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void led_canvas_fill(IntPtr canvas, byte r, byte g, byte b);
    }
}
