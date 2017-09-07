using System;
using System.Collections.Generic;
using System.Text;

namespace PiGPIO
{
    public sealed class Waveform : IDisposable
    {
        private IPiGPIO m_pigpio;
        private int m_id;

        internal Waveform(IPiGPIO pigpio, int id)
        {
            this.m_pigpio = pigpio;
            this.m_id = id;
        }

        public void Dispose()
        {
            this.m_pigpio.WaveformDelete(this.m_id);
        }
    }
}
