using System;
using System.Collections.Generic;
using System.Text;
using SixLabors.ImageSharp;

namespace LedMatrixPoc
{
    internal struct Frame
    {
        public Image<Rgba32> Image { get; internal set; }
        public int Delay { get; internal set; }
    }
}
