using System;
using System.Collections.Generic;
using System.Text;

namespace RaspberryPi.PiGPIO.Drivers.Dede
{
    public class CoremanHub75LedMatrix : BaseDriver
    {
        private readonly Hub75PinLayout m_pins;

        public Hub75PinLayout Layout => this.m_pins;

        public CoremanHub75LedMatrix(IPiGPIO gpio, Hub75PinLayout pinLayout) : base(gpio)
        {
            this.m_pins = pinLayout ?? throw new ArgumentNullException(nameof(pinLayout));
        }

        protected override void DefineUsedPins()
        {
            this.UseOutput(this.m_pins.A, false);
            this.UseOutput(this.m_pins.B, false);
            this.UseOutput(this.m_pins.C, false);
            this.UseOutput(this.m_pins.D, false);
            this.UseOutput(this.m_pins.E, false);
            this.UseOutput(this.m_pins.R1, false);
            this.UseOutput(this.m_pins.G1, false);
            this.UseOutput(this.m_pins.B1, false);
            this.UseOutput(this.m_pins.R2, false);
            this.UseOutput(this.m_pins.G2, false);
            this.UseOutput(this.m_pins.B2, false);
            this.UseOutput(this.m_pins.OE, false);
            this.UseOutput(this.m_pins.Strobe, false);
        }
    }
}
