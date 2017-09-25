using ImageSharp;
using ImageSharp.Drawing;
using ImageSharp.Processing;
using RaspberryPi.PiGPIO.Drivers.Dede.PixelFormats;
using RaspberryPi.PiGPIO.Drivers.Freetronics;
using SixLabors.Primitives;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("FreetronicsDMDImageSharpDev")]

namespace RaspberryPi.PiGPIO.Drivers.Dede
{
    public class FreetronicsDMDSurface : DMDBase, IDisposable, IDMDInternals
    {
        private readonly Image<BitPixel> m_img;
        private readonly Thread m_th;
        private bool m_run;

        private readonly int m_panWide;
        private readonly int m_panTall;
        private readonly int m_panTotal;

        private readonly int m_row1;
        private readonly int m_row2;
        private readonly int m_row3;
        private readonly int m_rowsize;
        private int m_scanSeq;

        public static readonly int PIXEL_WIDTH = 32;
        public static readonly int PIXEL_HEIGHT = 16;

        public Image<BitPixel> Surface => this.m_img;
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

            this.m_img = new Image<BitPixel>(this.m_panWide * PIXEL_WIDTH, this.m_panTall * PIXEL_HEIGHT);

            this.m_th = new Thread(this.RefreshDisplay);
            this.m_th.Name = this.GetType().Name;

            this.m_row1 = this.m_panTotal << 4;
            this.m_row2 = this.m_panTotal << 5;
            this.m_row3 = ((this.m_panTotal << 2) * 3) << 2;
            this.m_rowsize = this.m_panTotal << 2;
        }

        public override void Init()
        {
            this.Init(true);
        }

        public void Init(bool runThread)
        {
            base.Init();

            this.m_scanSeq = 0;

            if (runThread)
            {
                this.m_run = true;
                this.m_th.Start();
            }
        }

        public void Dispose()
        {
            if (this.m_th.ThreadState == ThreadState.Running)
            {
                this.m_run = false;
                this.m_th.Join();
            }
            this.m_img.Dispose();
        }

        public override void Clear(bool value)
        {
            this.m_img.Fill(value ? BitPixel.On : BitPixel.Off);
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
            int rowLength = this.m_img.Width;
            int rowsLength = this.m_panWide * 4 * this.m_panTall;
            byte[] bRow3 = new byte[rowsLength];
            byte[] bRow2 = new byte[rowsLength];
            byte[] bRow1 = new byte[rowsLength];
            byte[] bRow0 = new byte[rowsLength];
            for (int r = 0; r < this.m_panTall; r++)
            {
                BitPixel[] row3;
                BitPixel[] row2;
                BitPixel[] row1;
                BitPixel[] row0;
                lock (this.m_img)
                {
                    row3 = this.m_img.GetRowSpan(16 * r + seq + 12).ToArray();
                    row2 = this.m_img.GetRowSpan(16 * r + seq + 8).ToArray();
                    row1 = this.m_img.GetRowSpan(16 * r + seq + 4).ToArray();
                    row0 = this.m_img.GetRowSpan(16 * r + seq).ToArray();
                }

                for (int x = rowLength - 1; x > 0; x -= 8)
                {
                    int b3 = 0;
                    int b2 = 0;
                    int b1 = 0;
                    int b0 = 0;
                    for (int i = 7; i >= 0; i--)
                    {
                        b3 = (b3 << 1) | (row3[x - i].Value ? 1 : 0);
                        b2 = (b2 << 1) | (row2[x - i].Value ? 1 : 0);
                        b1 = (b1 << 1) | (row1[x - i].Value ? 1 : 0);
                        b0 = (b0 << 1) | (row0[x - i].Value ? 1 : 0);
                    }
                    bRow3[r * 4 + x / 8] = (byte)b3;
                    bRow2[r * 4 + x / 8] = (byte)b2;
                    bRow1[r * 4 + x / 8] = (byte)b1;
                    bRow0[r * 4 + x / 8] = (byte)b0;
                }
            }

            for (int i = 0; i < rowsLength; i++)
            {
                this.SoftSPITransfert(bRow3[i], bRow2[i], bRow1[i], bRow0[i]);
            }

            //this.Transmit(row3);
            //this.Transmit(row2);
            //this.Transmit(row1);
            //this.Transmit(row0);
            //this.Transmit(new[] { row3, row2, row1, row0 });

            //for (int x = this.m_img.Width - 1; x > 0; x -= 8)
            //for (int x = 0; x < this.m_img.Width; x += 8)
            //{
            //    int b3 = 0;
            //    int b2 = 0;
            //    int b1 = 0;
            //    int b0 = 0;
            //    for (int i = 0; i < 8; i++)
            //    {
            //        b3 = (b3 << 1) & (row3[x + 8 - i].Value ? 1 : 0);
            //        b2 = (b2 << 1) & (row2[x + 8 - i].Value ? 1 : 0);
            //        b1 = (b1 << 1) & (row1[x + 8 - i].Value ? 1 : 0);
            //        b0 = (b0 << 1) & (row0[x + 8 - i].Value ? 1 : 0);
            //    }
            //    this.SoftSPITransfert((byte)b3, (byte)b2, (byte)b1, (byte)b0);
            //}

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

        void IDMDInternals.SetPixel(int x, int y, bool value)
        {
            this.m_img.DrawLines(value ? BitPixel.On : BitPixel.Off, 1, new PointF[] { new PointF(x, y), new PointF(x, y) }, new GraphicsOptions(false));
        }

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
