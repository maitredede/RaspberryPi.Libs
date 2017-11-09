using Microsoft.Extensions.Logging;
using RaspberryPi.LibLedMatrix;
using SixLabors.ImageSharp;
using SixLabors.Primitives;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WebLedMatrixImageViewer
{
    public class LedMatrixManager : IDisposable
    {
        private readonly ILogger<LedMatrixManager> m_logger;
        private readonly SemaphoreSlim m_sem;
        private readonly LedMatrixOptions m_options;

        private LedMatrix m_matrix;
        private Thread m_threadDisplay;
        private bool m_run;
        private byte[] m_imageData;
        private string m_imageType;

        public int Brightness => this.m_options.brightness;

        public LedMatrixManager(ILogger<LedMatrixManager> logger)
        {
            this.m_logger = logger;
            this.m_sem = new SemaphoreSlim(1);
            this.m_options = new LedMatrixOptions()
            {
                brightness = 100,
                chain_length = 2,
                rows = 64,
                parallel = 1,
                hardware_mapping = "RBG",
            };
        }

        public void Dispose()
        {
            this.CloseMatrix();
            this.m_sem.Dispose();
        }

        public async Task<(String Type, byte[] Data)> GetCurrentImage()
        {
            await this.m_sem.WaitAsync();
            try
            {
                return (this.m_imageType, this.m_imageData);
            }
            finally
            {
                this.m_sem.Release();
            }
        }

        private void CloseMatrix()
        {
            if (this.m_threadDisplay != null)
            {
                this.m_run = false;
                this.m_threadDisplay.Abort();
                this.m_threadDisplay.Join();
                this.m_threadDisplay = null;
            }
            this.m_matrix?.Dispose();
        }

        private void ReInitializeMatrix()
        {
            this.CloseMatrix();
            this.m_matrix = new LedMatrix(this.m_options);
            this.m_threadDisplay = new Thread(this.Display);
            this.m_threadDisplay.Name = "matrix_display";
            this.m_threadDisplay.Start();
        }

        private void Display(object state)
        {
            try
            {
                this.m_run = true;
                this.DisplayImage();
            }
            catch (ThreadAbortException)
            {
                this.m_run = false;
            }
        }

        private void DisplayImage()
        {
            Stopwatch swatch = Stopwatch.StartNew();
            Size size = new Size(this.m_matrix.CanvasWidth, this.m_matrix.CanvasHeight);
            try
            {
                using (Image<Rgba32> img = Image.Load<Rgba32>(this.m_imageData))
                {
                    img.Mutate(ctx =>
                    {
                        ctx.Resize(size);
                    });
                    int framesCount = img.Frames.Count;
                    if (framesCount == 1)
                    {
                        this.m_matrix.UpdateCanvas(canvas =>
                        {
                            for (int x = 0; x < size.Width; x++)
                            {
                                for (int y = 0; y < size.Height; y++)
                                {
                                    Rgba32 pix = img.Frames.RootFrame[x, y];
                                    canvas.SetPixel(x, y, pix.R, pix.G, pix.B);
                                }
                            }
                        });
                        return;
                    }
                    while (this.m_run)
                    {
                        for (int i = 0; i < framesCount; i++)
                        {
                            long cur = swatch.ElapsedMilliseconds;
                            var frame = img.Frames[i];
                            this.m_matrix.UpdateCanvas(canvas =>
                            {
                                for (int x = 0; x < size.Width; x++)
                                {
                                    for (int y = 0; y < size.Height; y++)
                                    {
                                        Rgba32 pix = frame[x, y];
                                        canvas.SetPixel(x, y, pix.R, pix.G, pix.B);
                                    }
                                }
                            });
                            long elapsed = swatch.ElapsedMilliseconds - cur;
                            long wait = frame.MetaData.FrameDelay * 10 - elapsed;
                            if (wait > 0)
                            {
                                Thread.Sleep((int)wait);
                            }
                        }
                    }
                }
            }
            finally
            {
                swatch.Stop();
            }
        }

        public async Task SetBrightness(int value)
        {
            if (value < 1 || value > 100)
                throw new ArgumentOutOfRangeException(nameof(value));
            await this.m_sem.WaitAsync();
            try
            {
                if (value != this.m_options.brightness)
                {
                    this.CloseMatrix();
                    this.m_options.brightness = value;
                    this.ReInitializeMatrix();
                }
            }
            finally
            {
                this.m_sem.Release();
            }
        }

        public async Task SetImage(byte[] data, string type)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            if (string.IsNullOrEmpty(type))
                throw new ArgumentNullException(nameof(type));
            using (Image<Rgba32> img = Image.Load(data))
            {
                await this.m_sem.WaitAsync();
                try
                {
                    this.m_imageData = data;
                    this.m_imageType = type;
                    this.ReInitializeMatrix();
                }
                finally
                {
                    this.m_sem.Release();
                }
            }
        }
    }
}
