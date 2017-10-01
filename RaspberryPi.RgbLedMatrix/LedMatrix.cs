using RaspberryPi.RgbLedMatrix.Internals;
using RaspberryPi.RgbLedMatrix.Interop;
using System;

namespace RaspberryPi.RgbLedMatrix
{
    public sealed class LedMatrix : IDisposable, ILedMatrixInternals
    {
        private readonly IntPtr m_handle;
        private LedCanvas m_canvas;

        IntPtr ILedMatrixInternals.Handle => this.m_handle;

        public LedMatrix(int rows, int chained, int parallel)
        {
            this.m_handle = NativeMethods.MatrixCreate(rows, chained, parallel);
            if (this.m_handle == IntPtr.Zero)
                throw new InvalidOperationException("Can't create matrix");
        }

        ~LedMatrix()
        {
            this.Dispose(false);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        public LedCanvas Canvas
        {
            get
            {
                if (this.m_canvas == null)
                {
                    //this.m_canvas = new LedCanvas(this);
                    throw new NotImplementedException();
                }
                return this.m_canvas;
            }
        }

        private void Dispose(bool disposing)
        {
            NativeMethods.MatrixDelete(this.m_handle);
        }
    }
}
