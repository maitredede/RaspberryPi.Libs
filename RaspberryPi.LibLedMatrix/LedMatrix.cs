using RaspberryPi.LibLedMatrix.Interop;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;

namespace RaspberryPi.LibLedMatrix
{
    public sealed class LedMatrix : NativeObject
    {
        private readonly SemaphoreSlim m_updateLatch;
        private IntPtr m_canvas;
        private int m_canvasWidth;
        private int m_canvasHeight;

        public int CanvasWidth => this.m_canvasWidth;
        public int CanvasHeight => this.m_canvasHeight;

        public LedMatrix(int rows, int chained, int parallel) : base(CreateLedMatrix(rows, chained, parallel), true)
        {
            this.m_updateLatch = new SemaphoreSlim(1);
            this.m_canvas = NativeMethods.led_matrix_create_offscreen_canvas(this.handle);
            NativeMethods.led_canvas_get_size(this.m_canvas, out this.m_canvasWidth, out this.m_canvasHeight);
        }

        public LedMatrix(LedMatrixOptions options) : base(CreateLedMatrix(options), true)
        {
            this.m_updateLatch = new SemaphoreSlim(1);
            this.m_canvas = NativeMethods.led_matrix_create_offscreen_canvas(this.handle);
            NativeMethods.led_canvas_get_size(this.m_canvas, out this.m_canvasWidth, out this.m_canvasHeight);
        }

        private static IntPtr CreateLedMatrix(int rows, int chained, int parallel)
        {
            IntPtr ptr = NativeMethods.led_matrix_create(rows, chained, parallel);
            if (ptr == IntPtr.Zero)
                throw new OutOfMemoryException("Can't create led matrix");
            return ptr;
        }

        private static IntPtr CreateLedMatrix(LedMatrixOptions options)
        {
            GCHandle h = GCHandle.Alloc(options, GCHandleType.Pinned);
            try
            {
                IntPtr argv;
                IntPtr ptr = NativeMethods.led_matrix_create_from_options(h.AddrOfPinnedObject(), IntPtr.Zero, out argv);
                if (ptr == IntPtr.Zero)
                    throw new OutOfMemoryException("Can't create led matrix");
                return ptr;
            }
            finally
            {
                h.Free();
            }
        }

        protected override void FreeHandle()
        {
            NativeMethods.led_matrix_delete(this.handle);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.m_updateLatch.Dispose();
            }
            base.Dispose(disposing);
        }

        public void UpdateCanvas(Action<LedCanvas> method)
        {
            if (method == null)
                throw new ArgumentNullException(nameof(method));
            this.m_updateLatch.Wait();
            try
            {
                LedCanvas canvas = new LedCanvas(this.m_canvas, this.m_canvasWidth, this.m_canvasHeight);

                method(canvas);
                this.m_canvas = NativeMethods.led_matrix_swap_on_vsync(this.handle, this.m_canvas);
            }
            finally
            {
                this.m_updateLatch.Release();
            }
        }

        public bool UpdateOptions(LedMatrixOptions options)
        {
            GCHandle h = GCHandle.Alloc(options, GCHandleType.Pinned);
            try
            {
                return NativeMethods.led_matrix_update_options(this.handle, h.AddrOfPinnedObject());
            }
            finally
            {
                h.Free();
            }
        }
    }
}
