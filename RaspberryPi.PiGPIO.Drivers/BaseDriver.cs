using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

[assembly: InternalsVisibleTo("RaspberryPi.PiGPIO.Drivers.Tests")]
[assembly: InternalsVisibleTo("RaspberryPi.PiGPIO.Drivers.Drawings.ImageSharp")]
[assembly: InternalsVisibleTo("FreetronicsDMDImageSharpDev")]
[assembly: InternalsVisibleTo("FreetronicsDMD")]

namespace RaspberryPi.PiGPIO.Drivers
{
    public abstract class BaseDriver
    {
        protected readonly IPiGPIO m_gpio;

        public IPiGPIO Gpio => this.m_gpio;

        private readonly List<int> m_usedGpio;

        public BaseDriver(IPiGPIO gpio)
        {
            this.m_gpio = gpio ?? throw new ArgumentNullException(nameof(gpio));

            this.m_usedGpio = new List<int>();
        }

        public virtual void Init()
        {
            this.DefineUsedPins();
        }

        protected abstract void DefineUsedPins();

        protected void UseInput(int gpio)
        {
            if (this.m_usedGpio.Contains(gpio))
                throw new GpioAlreadyUsedException(gpio);
            this.m_usedGpio.Add(gpio);
            this.m_gpio.SetMode(gpio, Mode.Input);
        }

        protected void UseOutput(int gpio, bool value)
        {
            if (this.m_usedGpio.Contains(gpio))
                throw new GpioAlreadyUsedException(gpio);
            this.m_usedGpio.Add(gpio);
            this.m_gpio.Write(gpio, value);
            this.m_gpio.SetMode(gpio, Mode.Output);
        }

        //protected void UsePin(int gpio, Mode mode, bool value)
        //{
        //    this.m_usedPins.Add(gpio);
        //    if (mode == Mode.Output)
        //    {
        //        this.m_gpio.SetMode(gpio, Mode.Input);
        //    }
        //    this.m_gpio.Write(gpio, value);
        //    this.m_gpio.SetMode(gpio, mode);
        //}
    }
}
