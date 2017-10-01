using RaspberryPi.RgbLedMatrix.Internals;
using System;
using System.Collections.Generic;
using System.Text;

namespace RaspberryPi.RgbLedMatrix
{
    public sealed class LedCanvas : ILedCanvasInternals
    {
        private readonly LedMatrix m_matrix;
        private readonly IntPtr m_handle;

        internal LedCanvas(LedMatrix matrix, IntPtr handle)
        {
            this.m_matrix = matrix;
            this.m_handle = handle;
        }
    }
}
