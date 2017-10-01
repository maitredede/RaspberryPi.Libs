using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace RaspberryPi.RgbLedMatrix
{
    public sealed class ImageSharpLedCanvas : IDisposable
    {
        private readonly LedMatrix m_matrix;
        private readonly Image<Rgba32> m_img;

        internal ImageSharpLedCanvas(LedMatrix matrix, Image<Rgba32> image)
        {
            this.m_matrix = matrix;
            this.m_img = image;
        }

        public void Dispose()
        {
        }
    }
}
