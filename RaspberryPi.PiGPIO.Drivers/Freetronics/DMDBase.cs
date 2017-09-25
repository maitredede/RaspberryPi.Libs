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
    }
}
