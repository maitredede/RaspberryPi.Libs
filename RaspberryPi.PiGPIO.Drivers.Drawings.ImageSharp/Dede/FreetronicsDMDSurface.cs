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
            int rowsize = this.m_panTotal << 2;
            byte[][] scanData = new byte[SCANCOUNT][];
            for (int i = 0; i < SCANCOUNT; i++)
            {
                scanData[i] = new byte[rowsize * 4];
            }
            byte[] packedData = new byte[this.m_pixelTotal / 8];

            this.m_semaphore.Wait();
            try
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

                    if (pixData[i] == 0)
                        pixData[uiDMDRAMPointer] |= lookup;   // one bit is pixel off
                    else
                        pixData[uiDMDRAMPointer] &= (byte)~lookup;  // zero bit is pixel on
                }

                for (int scan = 0; scan < SCANCOUNT; scan++)
                {
                    int offset = rowsize * scan;
                    for (int i = 0; i < rowsize; i++)
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

        protected override void SoftSPITransfert(params byte[] bx)
        {
            using (var spi = this.m_gpio.OpenBitBangSpi(2, 3, this.Layout.Data, this.Layout.Clock, 250000, 0))
            {
                spi.Write(bx);
            }
        }

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

        //private void Scan(ref int seq)
        //{
        //    int rowLength = this.m_img.Width;
        //    int rowsLength = this.m_panWide * 4 * this.m_panTall;
        //    byte[] bRow3 = new byte[rowsLength];
        //    byte[] bRow2 = new byte[rowsLength];
        //    byte[] bRow1 = new byte[rowsLength];
        //    byte[] bRow0 = new byte[rowsLength];
        //    for (int r = 0; r < this.m_panTall; r++)
        //    {
        //        BitPixel[] row3;
        //        BitPixel[] row2;
        //        BitPixel[] row1;
        //        BitPixel[] row0;
        //        lock (this.m_img)
        //        {
        //            row3 = this.m_img.GetRowSpan(16 * r + seq + 12).ToArray();
        //            row2 = this.m_img.GetRowSpan(16 * r + seq + 8).ToArray();
        //            row1 = this.m_img.GetRowSpan(16 * r + seq + 4).ToArray();
        //            row0 = this.m_img.GetRowSpan(16 * r + seq).ToArray();
        //        }

        //        for (int x = rowLength - 1; x > 0; x -= 8)
        //        {
        //            int b3 = 0;
        //            int b2 = 0;
        //            int b1 = 0;
        //            int b0 = 0;
        //            for (int i = 7; i >= 0; i--)
        //            {
        //                b3 = (b3 << 1) | (row3[x - i].Value ? 1 : 0);
        //                b2 = (b2 << 1) | (row2[x - i].Value ? 1 : 0);
        //                b1 = (b1 << 1) | (row1[x - i].Value ? 1 : 0);
        //                b0 = (b0 << 1) | (row0[x - i].Value ? 1 : 0);
        //            }
        //            bRow3[r * 4 + x / 8] = (byte)b3;
        //            bRow2[r * 4 + x / 8] = (byte)b2;
        //            bRow1[r * 4 + x / 8] = (byte)b1;
        //            bRow0[r * 4 + x / 8] = (byte)b0;
        //        }
        //    }

        //    for (int i = 0; i < rowsLength; i++)
        //    {
        //        this.SoftSPITransfert(bRow3[i], bRow2[i], bRow1[i], bRow0[i]);
        //    }

        //    //this.Transmit(row3);
        //    //this.Transmit(row2);
        //    //this.Transmit(row1);
        //    //this.Transmit(row0);
        //    //this.Transmit(new[] { row3, row2, row1, row0 });

        //    //for (int x = this.m_img.Width - 1; x > 0; x -= 8)
        //    //for (int x = 0; x < this.m_img.Width; x += 8)
        //    //{
        //    //    int b3 = 0;
        //    //    int b2 = 0;
        //    //    int b1 = 0;
        //    //    int b0 = 0;
        //    //    for (int i = 0; i < 8; i++)
        //    //    {
        //    //        b3 = (b3 << 1) & (row3[x + 8 - i].Value ? 1 : 0);
        //    //        b2 = (b2 << 1) & (row2[x + 8 - i].Value ? 1 : 0);
        //    //        b1 = (b1 << 1) & (row1[x + 8 - i].Value ? 1 : 0);
        //    //        b0 = (b0 << 1) & (row0[x + 8 - i].Value ? 1 : 0);
        //    //    }
        //    //    this.SoftSPITransfert((byte)b3, (byte)b2, (byte)b1, (byte)b0);
        //    //}

        //    this.SoftCommit();
        //}

        void IDMDInternals.ScanFull()
        {
            for (int i = 0; i < 4; i++)
            {
                int scan = i;
                this.Scan(ref scan);
            }
        }
    }
}
