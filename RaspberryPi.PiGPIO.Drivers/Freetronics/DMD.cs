using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace RaspberryPi.PiGPIO.Drivers.Freetronics
{
    public class DMD : DMDBase, IDMDInternals
    {
        private readonly int m_displaysWide;
        private readonly int m_displaysHigh;
        private readonly int m_displaysTotal;

        private readonly int row1;
        private readonly int row2;
        private readonly int row3;

        private int m_dmdByte;

        public int PanelsWide => this.m_displaysWide;
        public int PanelsTall => this.m_displaysHigh;

        public override void Clear(bool value)
        {
            if (!value)
            {
                Array.Clear(this.bDMDScreenRAM, 0, this.bDMDScreenRAM.Length);
            }
            else
            {
                for (int i = 0; i < this.bDMDScreenRAM.Length; i++)
                {
                    this.bDMDScreenRAM[i] = byte.MaxValue;
                }
            }
        }

        private readonly byte[] bDMDScreenRAM;

        public DMD(IPiGPIO gpio, DMDPinLayout layout, int panelsWide, int panelsHigh) : base(gpio, layout)
        {
            if (panelsWide < 1)
                throw new ArgumentOutOfRangeException(nameof(panelsWide));
            if (panelsHigh < 1)
                throw new ArgumentOutOfRangeException(nameof(panelsHigh));
            this.m_displaysWide = panelsWide;
            this.m_displaysHigh = panelsHigh;
            this.m_displaysTotal = this.m_displaysWide * this.m_displaysHigh;

            this.row1 = this.m_displaysTotal << 4;
            this.row2 = this.m_displaysTotal << 5;
            this.row3 = ((this.m_displaysTotal << 2) * 3) << 2;

            this.bDMDScreenRAM = new byte[this.m_displaysTotal * DMD_RAM_SIZE_BYTES];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetPixel(int x, int y, bool value)
        {
            this.SetPixel(x, y, DMDGraphicsMode.Normal, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetPixel(int x, int y, DMDGraphicsMode graphicsMode, bool pixel)
        {
            if (x < 0 || x >= (DMD_PIXELS_ACROSS * this.m_displaysWide))
                return; // throw new ArgumentOutOfRangeException(nameof(x));
            if (y < 0 || y >= (DMD_PIXELS_DOWN * this.m_displaysHigh))
                return; // throw new ArgumentOutOfRangeException(nameof(y));

            int panel = (x / DMD_PIXELS_ACROSS) + (this.m_displaysWide * (y / DMD_PIXELS_DOWN));
            x = (x % DMD_PIXELS_ACROSS) + (panel << 5);
            y = y % DMD_PIXELS_DOWN;
            //set pointer to DMD RAM byte to be modified
            int uiDMDRAMPointer = x / 8 + y * (this.m_displaysTotal << 2);

            byte lookup = bPixelLookupTable[x & 0x07];

            switch (graphicsMode)
            {
                case DMDGraphicsMode.Normal:
                    if (pixel)
                        this.bDMDScreenRAM[uiDMDRAMPointer] |= lookup;   // one bit is pixel off
                    else
                        this.bDMDScreenRAM[uiDMDRAMPointer] &= (byte)~lookup;  // zero bit is pixel on
                    break;
                case DMDGraphicsMode.Inverse:
                    if (pixel)
                        this.bDMDScreenRAM[uiDMDRAMPointer] &= (byte)~lookup;  // zero bit is pixel on
                    else
                        this.bDMDScreenRAM[uiDMDRAMPointer] |= lookup;   // one bit is pixel off
                    break;
                //case DMDGraphicsMode.Toggle:
                //    if (pixel == true)
                //    {
                //        if ((this.bDMDScreenRAM[uiDMDRAMPointer] & lookup) == 0)
                //            this.bDMDScreenRAM[uiDMDRAMPointer] |= lookup;   // one bit is pixel off
                //        else
                //            this.bDMDScreenRAM[uiDMDRAMPointer] &= (byte)~lookup;  // one bit is pixel off
                //    }
                //    break;
                //case DMDGraphicsMode.Or:
                //    //only set pixels on
                //    if (pixel == true)
                //        this.bDMDScreenRAM[uiDMDRAMPointer] &= (byte)~lookup;  // zero bit is pixel on
                //    break;
                //case DMDGraphicsMode.Nor:
                //    //only clear on pixels
                //    if ((pixel == true) &&
                //        ((this.bDMDScreenRAM[uiDMDRAMPointer] & lookup) == 0))
                //        this.bDMDScreenRAM[uiDMDRAMPointer] |= lookup;   // one bit is pixel off
                //    break;
                default:
                    throw new NotImplementedException("GraphicsMode." + graphicsMode.ToString());
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ScanFull()
        {
            for (int i = 0; i < 4; i++)
            {
                this.Scan();
            }
        }

        void IDMDInternals.Scan(int scan)
        {
            this.m_dmdByte = (byte)scan;
            this.Scan();
        }

        public void Scan()
        {
            //SPI transfer pixels to the display hardware shift registers
            int rowsize = this.m_displaysTotal << 2;
            int offset = rowsize * this.m_dmdByte;
            for (int i = 0; i < rowsize; i++)
            {
                int offPos = offset + i;
                byte b3 = this.bDMDScreenRAM[offPos + this.row3];
                byte b2 = this.bDMDScreenRAM[offPos + this.row2];
                byte b1 = this.bDMDScreenRAM[offPos + this.row1];
                byte b0 = this.bDMDScreenRAM[offPos + 0];

                this.SoftSPITransfert(b3, b2, b1, b0);
            }

            OE_DMD_ROWS_OFF();
            LATCH_DMD_SHIFT_REG_TO_OUTPUT();
            switch (this.m_dmdByte)
            {
                case 0:         // row 1, 5, 9, 13 were clocked out
                    LIGHT_DMD_ROW_01_05_09_13();
                    this.m_dmdByte = 1;
                    break;
                case 1:         // row 2, 6, 10, 14 were clocked out
                    LIGHT_DMD_ROW_02_06_10_14();
                    this.m_dmdByte = 2;
                    break;
                case 2:         // row 3, 7, 11, 15 were clocked out
                    LIGHT_DMD_ROW_03_07_11_15();
                    this.m_dmdByte = 3;
                    break;
                case 3:         // row 4, 8, 12, 16 were clocked out
                    LIGHT_DMD_ROW_04_08_12_16();
                    this.m_dmdByte = 0;
                    break;
                default:
                    this.m_dmdByte = 0;
                    break;
            }
            OE_DMD_ROWS_ON();
        }
    }
}
