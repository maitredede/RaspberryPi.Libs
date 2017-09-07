using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

namespace RaspberryPi.Userland.Interop
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct VC_RECT_T
    {
        public VC_RECT_T(int x, int y, int width, int height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        public int x, y, width, height;

        public static implicit operator Rectangle(VC_RECT_T rect)
        {
            return new Rectangle(rect.x, rect.y, rect.width, rect.height);
        }

        public static implicit operator VC_RECT_T(Rectangle rect)
        {
            return new Rectangle(rect.X, rect.Y, rect.Width, rect.Height);
        }
    }
}
