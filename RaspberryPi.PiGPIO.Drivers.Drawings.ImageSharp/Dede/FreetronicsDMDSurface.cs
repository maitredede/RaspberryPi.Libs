using RaspberryPi.PiGPIO.Drivers.Dede.PixelFormats;
using RaspberryPi.PiGPIO.Drivers.Freetronics;
using SixLabors.Primitives;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Advanced;
using System.Runtime.CompilerServices;
using System.IO;

namespace RaspberryPi.PiGPIO.Drivers.Dede
{
    public class FreetronicsDMDSurface : DMDBase, IDisposable, IDMDInternals
    {
        private readonly Image<BitPixel> m_img;
        private readonly SemaphoreSlim m_semaphore;
        private readonly Thread m_th;
        private bool m_run;

        private readonly int m_panWide;
        private readonly int m_panTall;
        private readonly int m_panTotal;
        private readonly int m_rowsize;
        private readonly int m_row1;
        private readonly int m_row2;
        private readonly int m_row3;
        private int m_scanSeq;

        public int PanelsWide => this.m_panWide;
        public int PanelsTall => this.m_panTall;

        public FreetronicsDMDSurface(IPiGPIO gpio, DMDPinLayout layout) : this(gpio, layout, 1, 1)
        {
        }

        public FreetronicsDMDSurface(IPiGPIO gpio, DMDPinLayout layout, int panelsWide, int panelsTall)
            : base(gpio, layout)
        {
            if (panelsWide <= 0)
                throw new ArgumentOutOfRangeException(nameof(panelsWide));
            this.m_panWide = panelsWide;
            if (panelsTall <= 0)
                throw new ArgumentOutOfRangeException(nameof(panelsTall));
            this.m_panTall = panelsTall;
            this.m_panTotal = this.m_panWide * this.m_panTall;
            this.m_rowsize = this.m_panTotal << 2;

            this.m_img = new Image<BitPixel>(this.m_panWide * DMD_PIXELS_ACROSS, this.m_panTall * DMD_PIXELS_DOWN);
            this.m_semaphore = new SemaphoreSlim(1);

            this.m_th = new Thread(this.RefreshDisplay);
            this.m_th.Name = this.GetType().Name;

            this.m_row1 = this.m_panTotal << 4;
            this.m_row2 = this.m_panTotal << 5;
            this.m_row3 = ((this.m_panTotal << 2) * 3) << 2;

            this.m_pixelTotal = this.m_img.Width * this.m_img.Height;

        }

        private readonly int m_pixelTotal;

        public void Dispose()
        {
            if (this.m_th.ThreadState == ThreadState.Running)
            {
                this.m_run = false;
                this.m_th.Join();
            }
            this.m_img.Dispose();
            this.m_semaphore.Dispose();
        }

        public override void Init()
        {
            this.Init(true);
        }

        public void Init(bool runThread)
        {
            base.Init();

            this.Clear(false);
            this.m_scanSeq = 0;

            if (runThread)
            {
                this.m_run = true;
                this.m_th.Start();
            }
        }

        private byte[][] m_scanData;

        private const int SCANCOUNT = 4;

        public void UpdateSurface(Action<IImageProcessingContext<BitPixel>> updates)
        {
            //Allocate new data
            byte[][] scanData = new byte[SCANCOUNT][];
            for (int i = 0; i < SCANCOUNT; i++)
            {
                scanData[i] = new byte[this.m_rowsize * 4];
            }
            byte[] packedData = new byte[this.m_pixelTotal / 8];
            lock (this.m_img)
            {
                //Update the drawings
                this.m_img.Mutate(updates);

                //Let the magic begin
                //this.UnsafePrepareScanData();
                byte[] pixData = this.m_img.SavePixelData();

                for (int i = 0; i < pixData.Length; i++)
                {
                    int y = Math.DivRem(i, this.m_img.Width, out int x);

                    int panel = (x / DMD_PIXELS_ACROSS) + (this.m_panWide * (y / DMD_PIXELS_DOWN));
                    x = (x % DMD_PIXELS_ACROSS) + (panel << 5);
                    y = y % DMD_PIXELS_DOWN;
                    //set pointer to DMD RAM byte to be modified
                    int uiDMDRAMPointer = x / 8 + y * (this.m_panWide << 2);

                    byte lookup = bPixelLookupTable[x & 0x07];

                    if (pixData[i] != 0)
                        pixData[uiDMDRAMPointer] |= lookup;   // one bit is pixel off
                    else
                        pixData[uiDMDRAMPointer] &= (byte)~lookup;  // zero bit is pixel on
                }

                for (int scan = 0; scan < SCANCOUNT; scan++)
                {
                    int offset = this.m_rowsize * scan;
                    for (int i = 0; i < this.m_rowsize; i++)
                    {
                        int offPos = offset + i;
                        byte b3 = pixData[offPos + this.m_row3];
                        byte b2 = pixData[offPos + this.m_row2];
                        byte b1 = pixData[offPos + this.m_row1];
                        byte b0 = pixData[offPos + 0];

                        //this.SoftSPITransfert(b3, b2, b1, b0);
                        scanData[scan][i * 4 + 0] = b3;
                        scanData[scan][i * 4 + 1] = b2;
                        scanData[scan][i * 4 + 2] = b1;
                        scanData[scan][i * 4 + 3] = b0;
                    }
                }
            }

            this.m_semaphore.Wait();
            try
            {
                this.m_scanData = scanData;
            }
            finally
            {
                this.m_semaphore.Release();
            }
        }

        public override void Clear(bool value)
        {
            this.UpdateSurface((updates) =>
            {
                updates.Fill(value ? BitPixel.On : BitPixel.Off);
            });
        }

        void IDMDInternals.SetPixel(int x, int y, bool value)
        {
            this.UpdateSurface((updates) =>
            {
                updates.DrawLines(value ? BitPixel.On : BitPixel.Off, 1, new PointF[] { new PointF(x, y), new PointF(x, y) }, new GraphicsOptions(false));
            });
        }

        private void RefreshDisplay()
        {
            while (this.m_run)
            {
                this.ScanOnce();
            }
        }

        private void ScanOnce()
        {
            this.Scan(ref this.m_scanSeq);
        }

        void IDMDInternals.Scan(int scan)
        {
            this.Scan(ref scan);
        }

        private void Transmit(Span<BitPixel>[] rows)
        {
            int len = rows[0].Length;
            for (int i = rows[0].Length - 1; i > 0; i -= 8)
            {
                for (int x = 0; x < rows.Length; x++)
                {
                    Span<BitPixel> row = rows[x];

                    int b = 0;
                    for (int j = 7; j >= 0; j--)
                    {
                        b = (b << 1) | (row[i - j].Value ? 1 : 0);
                    }
                    this.SoftSPITransfert((byte)b);
                }
            }
        }

        private void Scan(ref int seq)
        {
            byte[][] scanData;
            this.m_semaphore.Wait();
            try
            {
                scanData = this.m_scanData;
            }
            finally
            {
                this.m_semaphore.Release();
            }

            byte[] currentScan = scanData[seq];
            this.SoftSPITransfert(currentScan);
            this.SoftCommit(ref seq);
        }

        //protected override void SoftSPITransfert(params byte[] bx)
        //{
        //    using (var spi = this.m_gpio.OpenBitBangSpi(2, 3, this.Layout.Data, this.Layout.Clock, 250000, 0))
        //    {
        //        spi.Write(bx);
        //    }
        //}

        private void SoftCommit(ref int seq)
        {
            OE_DMD_ROWS_OFF();
            LATCH_DMD_SHIFT_REG_TO_OUTPUT();
            switch (seq)
            {
                case 0:         // row 1, 5, 9, 13 were clocked out
                    LIGHT_DMD_ROW_01_05_09_13();
                    seq = 1;
                    break;
                case 1:         // row 2, 6, 10, 14 were clocked out
                    LIGHT_DMD_ROW_02_06_10_14();
                    seq = 2;
                    break;
                case 2:         // row 3, 7, 11, 15 were clocked out
                    LIGHT_DMD_ROW_03_07_11_15();
                    seq = 3;
                    break;
                case 3:         // row 4, 8, 12, 16 were clocked out
                    LIGHT_DMD_ROW_04_08_12_16();
                    seq = 0;
                    break;
                default:
                    seq = 0;
                    break;
            }
            OE_DMD_ROWS_ON();
        }

        void IDMDInternals.ScanFull()
        {
            for (int i = 0; i < 4; i++)
            {
                int scan = i;
                this.Scan(ref scan);
            }
        }

        public void SavePNG(string filename)
        {
            lock (this.m_img)
            {
                using (FileStream fs = File.Create(filename))
                {
                    this.m_img.SaveAsPng(fs);
                }
            }
        }
    }
}
