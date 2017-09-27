using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace RaspberryPi.PiGPIO.Drivers.Freetronics
{
    public abstract class DMDBase : BaseDriver
    {
        private readonly DMDPinLayout m_layout;

        public DMDBase(IPiGPIO gpio, DMDPinLayout layout) : base(gpio)
        {
            this.m_layout = layout ?? throw new ArgumentNullException(nameof(layout));
        }

        public DMDPinLayout Layout => this.m_layout;

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
        protected virtual void SoftSPITransfert(params byte[] bx)
        {
            for (int x = 0; x < bx.Length; x++)
            {
                byte b = bx[x];
                for (int i = 7; i >= 0; i--)
                {
                    bool value = (b & (1 << i)) == 0;
                    this.TransfertDataBit(value);
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected virtual void TransfertDataBit(bool value)
        {
            this.m_gpio.Write(this.Layout.Data, value);
            this.m_gpio.Write(this.Layout.Clock, true);
            this.m_gpio.Write(this.Layout.Clock, false);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void OE_DMD_ROWS_OFF()
        {
            this.m_gpio.Write(this.Layout.OE, false);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void OE_DMD_ROWS_ON()
        {
            this.m_gpio.Write(this.Layout.OE, true);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void LIGHT_DMD_ROW_01_05_09_13()
        {
            this.m_gpio.Write(this.Layout.B, false);
            this.m_gpio.Write(this.Layout.A, false);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void LIGHT_DMD_ROW_02_06_10_14()
        {
            this.m_gpio.Write(this.Layout.B, false);
            this.m_gpio.Write(this.Layout.A, true);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void LIGHT_DMD_ROW_03_07_11_15()
        {
            this.m_gpio.Write(this.Layout.B, true);
            this.m_gpio.Write(this.Layout.A, false);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void LIGHT_DMD_ROW_04_08_12_16()
        {
            this.m_gpio.Write(this.Layout.B, true);
            this.m_gpio.Write(this.Layout.A, true);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void LATCH_DMD_SHIFT_REG_TO_OUTPUT()
        {
            this.m_gpio.Write(this.Layout.Strobe, true);
            this.m_gpio.Write(this.Layout.Strobe, false);
        }

        public abstract void Clear(bool value);
        public void Clear()
        {
            this.Clear(false);
        }

        //lookup table for DMD::writePixel to make the pixel indexing routine faster
        protected static readonly byte[] bPixelLookupTable =
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

        //display screen (and subscreen) sizing
        public static readonly int DMD_PIXELS_ACROSS = 32;  //pixels across x axis (base 2 size expected)
        public static readonly int DMD_PIXELS_DOWN = 16; //pixels down y axis
        public static readonly int DMD_BITSPERPIXEL = 1;//1 bit per pixel, use more bits to allow for pwm screen brightness control
        public static readonly int DMD_RAM_SIZE_BYTES = ((DMD_PIXELS_ACROSS * DMD_BITSPERPIXEL / 8) * DMD_PIXELS_DOWN); // (32x * 1 / 8) = 4 bytes, * 16y = 64 bytes per screen here.
    }
}
