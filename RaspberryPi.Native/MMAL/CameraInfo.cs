using System;
using System.Collections.Generic;
using System.Text;

namespace RaspberryPi.MMAL
{
    public sealed class CameraInfo
    {
        internal CameraInfo()
        {
        }

        public int Width { get; internal set; }
        public int Height { get; internal set; }
        public string Name { get; internal set; }
    }
}
