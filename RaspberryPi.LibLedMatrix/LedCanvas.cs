using RaspberryPi.LibLedMatrix.Interop;
using System;
using System.Collections.Generic;
using System.Text;

namespace RaspberryPi.LibLedMatrix
{
    public sealed class LedCanvas
    {
        private readonly IntPtr m_handle;

        private readonly int m_width;
        private readonly int m_height;

        public int Width => this.m_width;
        public int Height => this.m_height;

        internal LedCanvas(IntPtr handle, int width, int height)
        {
            this.m_handle = handle;
            this.m_width = width;
            this.m_height = height;
        }

        public void SetPixel(int x, int y, byte r, byte g, byte b)
        {
            NativeMethods.led_canvas_set_pixel(this.m_handle, x, y, r, g, b);
        }
    }
}
