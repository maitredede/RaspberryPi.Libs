using System;
using System.Collections.Generic;
using System.Text;

namespace RaspberryPi.PiGPIO.Drivers.Dede
{
    public class CoremanHub75LedMatrix
    {
        private readonly IPiGPIO m_gpio;
        private readonly Hub75PinLayout m_pins;

        public Hub75PinLayout Layout => this.m_pins;

        public CoremanHub75LedMatrix(IPiGPIO gpio, Hub75PinLayout pinLayout)
        {
            this.m_gpio = gpio ?? throw new ArgumentNullException(nameof(gpio));
            this.m_pins = pinLayout ?? throw new ArgumentNullException(nameof(pinLayout));
        }

        public void Init()
        {
            this.InitPin(this.m_pins.A);
        }

        private void InitPin(int pin)
        {
            this.m_gpio.SetMode(pin, Mode.Output);
            this.m_gpio.Write(pin, false);
        }
    }
}
