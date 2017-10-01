using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.Text;
using RaspberryPi.RgbLedMatrix.Internals;

namespace RaspberryPi.RgbLedMatrix
{
    public static class ImageSharpLedMatrixExtensions
    {
        public static ImageSharpLedCanvas CreateImageSharpCanvas(this LedMatrix matrix)
        {
            if (matrix == null)
                throw new ArgumentNullException();
            ILedMatrixInternals matrixInternals = (ILedMatrixInternals)matrix;

            throw new NotImplementedException();
        }
    }
}
