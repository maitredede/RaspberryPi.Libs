using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

namespace RaspberryPi.Native
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct VC_RECT_T
    {
        public VC_RECT_T(Rectangle rect) : this(rect.X, rect.Y, rect.Width, rect.Height)
        {
        }

        public VC_RECT_T(int x, int y, int width, int height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        public int x, y, width, height;
    }
}
