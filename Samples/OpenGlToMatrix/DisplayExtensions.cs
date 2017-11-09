using RaspberryPi.Userland;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace OpenGlToMatrix
{
    public static class DisplayExtensions
    {
        public static Rectangle ToRectangle(this Display display)
        {
            return new Rectangle(0, 0, display.Width, display.Height);
        }
    }
}
