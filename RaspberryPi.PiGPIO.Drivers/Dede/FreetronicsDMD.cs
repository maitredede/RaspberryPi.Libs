using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace RaspberryPi.PiGPIO.Drivers.Dede
{
    public class FreetronicsDMD : BaseDriver
    {
        private readonly FreetronicsDMDPinLayout m_layout;
        private readonly int m_displaysWide;
        private readonly int m_displaysHigh;
        private readonly int m_displaysTotal;

        private readonly int row1;
        private readonly int row2;
        private readonly int row3;

        private int m_dmdByte;

        public FreetronicsDMDPinLayout Layout => this.m_layout;

        //display screen (and subscreen) sizing
        public static readonly int DMD_PIXELS_ACROSS = 32;  //pixels across x axis (base 2 size expected)
        public static readonly int DMD_PIXELS_DOWN = 16; //pixels down y axis
        public static readonly int DMD_BITSPERPIXEL = 1;//1 bit per pixel, use more bits to allow for pwm screen brightness control
        public static readonly int DMD_RAM_SIZE_BYTES = ((DMD_PIXELS_ACROSS * DMD_BITSPERPIXEL / 8) * DMD_PIXELS_DOWN); // (32x * 1 / 8) = 4 bytes, * 16y = 64 bytes per screen here.

        public void Clear()
        {
            this.Clear(true);
        }

        public void Clear(bool normal)
        {
            if (normal)
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

        //lookup table for DMD::writePixel to make the pixel indexing routine faster
        private static readonly byte[] bPixelLookupTable =
        {
           0x80,   //0, bit 7
           0x40,   //1, bit 6
           0x20,   //2. bit 5
           0x10,   //3, bit 4
           0x08,   //4, bit 3
           0x04,   //5, bit 2
           0x02,   //6, bit 1
           0x01    //7, bit 0
        };

        private readonly byte[] bDMDScreenRAM;

        public FreetronicsDMD(IPiGPIO gpio, FreetronicsDMDPinLayout layout, int panelsWide, int panelsHigh) : base(gpio)
        {
            this.m_layout = layout ?? throw new ArgumentNullException(nameof(layout));

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

        protected override void DefineUsedPins()
        {
            this.UseOutput(this.m_layout.A, false);
            this.UseOutput(this.m_layout.B, false);
            this.UseOutput(this.m_layout.Clock, false);
            this.UseOutput(this.m_layout.Strobe, false);
            this.UseOutput(this.m_layout.Data, true);
            this.UseOutput(this.m_layout.OE, false);
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
                throw new ArgumentOutOfRangeException(nameof(x));
            if (y < 0 || y >= (DMD_PIXELS_DOWN * this.m_displaysHigh))
                throw new ArgumentOutOfRangeException(nameof(y));

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
        private void SoftSPITransfert(byte b)
        {
            for (int i = 7; i >= 0; i--)
            {
                bool value = (b & (1 << i)) == 0;
                this.m_gpio.Write(this.m_layout.Data, value);
                this.m_gpio.Write(this.m_layout.Clock, true);
                this.m_gpio.Write(this.m_layout.Clock, false);
            }
        }

        public void ScanFull()
        {
            for (int i = 0; i < 4; i++)
            {
                this.Scan();
            }
        }

        public void Scan()
        {
            //SPI transfer pixels to the display hardware shift registers
            int rowsize = this.m_displaysTotal << 2;
            int offset = rowsize * this.m_dmdByte;
            for (int i = 0; i < rowsize; i++)
            {
                this.SoftSPITransfert(bDMDScreenRAM[offset + i + row3]);
                this.SoftSPITransfert(bDMDScreenRAM[offset + i + row2]);
                this.SoftSPITransfert(bDMDScreenRAM[offset + i + row1]);
                this.SoftSPITransfert(bDMDScreenRAM[offset + i]);
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

        private void OE_DMD_ROWS_OFF()
        {
            this.m_gpio.Write(this.m_layout.OE, false);
        }

        private void OE_DMD_ROWS_ON()
        {
            this.m_gpio.Write(this.m_layout.OE, true);
        }

        private void LIGHT_DMD_ROW_01_05_09_13()
        {
            this.m_gpio.Write(this.m_layout.B, false);
            this.m_gpio.Write(this.m_layout.A, false);
        }

        private void LIGHT_DMD_ROW_02_06_10_14()
        {
            this.m_gpio.Write(this.m_layout.B, false);
            this.m_gpio.Write(this.m_layout.A, true);
        }

        private void LIGHT_DMD_ROW_03_07_11_15()
        {
            this.m_gpio.Write(this.m_layout.B, true);
            this.m_gpio.Write(this.m_layout.A, false);
        }

        private void LIGHT_DMD_ROW_04_08_12_16()
        {
            this.m_gpio.Write(this.m_layout.B, true);
            this.m_gpio.Write(this.m_layout.A, true);
        }

        private void LATCH_DMD_SHIFT_REG_TO_OUTPUT()
        {
            this.m_gpio.Write(this.m_layout.Strobe, true);
            this.m_gpio.Write(this.m_layout.Strobe, false);
        }
    }
}
