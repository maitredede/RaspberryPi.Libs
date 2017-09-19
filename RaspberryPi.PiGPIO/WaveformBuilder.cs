using System;
using System.Collections.Generic;
using System.Text;

namespace RaspberryPi.PiGPIO
{
    public sealed class WaveformBuilder
    {
        private readonly IPiGPIO m_gpio;
        private readonly List<Pulse> m_lst;

        internal WaveformBuilder(IPiGPIO gpio)
        {
            this.m_gpio = gpio;
            this.m_lst = new List<Pulse>();
        }

        public WaveformBuilder Append(Pulse pulse)
        {
            this.m_lst.Add(pulse);
            return this;
        }

        public WaveformBuilder Append(params Pulse[] pulse)
        {
            this.m_lst.AddRange(pulse);
            return this;
        }

        public Waveform Build()
        {
            this.m_gpio.WaveFormNew();

            const int sliceSize = 10;

            for (int slice = 0; slice < this.m_lst.Count; slice += sliceSize)
            {
                int remain = Math.Min(sliceSize, this.m_lst.Count - slice);
                if (remain == 0)
                    break;
                Pulse[] data = new Pulse[remain];
                for (int i = 0; i < remain; i++)
                {
                    data[i] = this.m_lst[slice + i];
                }
                this.m_gpio.WaveformAppend(data);
            }
            int id = this.m_gpio.WaveformCreate();
            return new Waveform(this.m_gpio, id);
        }
    }
}
