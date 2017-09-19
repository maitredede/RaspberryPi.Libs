using System;
using System.Collections.Generic;
using System.Text;

namespace RaspberryPi.PiGPIO
{
    /// <summary>
    /// Callback informations
    /// </summary>
    public sealed class CallbackInfo
    {
        private readonly uint m_port;
        private readonly uint m_bits;
        private readonly Edge m_edge;
        private readonly GpioCallback m_callback;

        internal CallbackInfo(uint port, uint bits, Edge edge, GpioCallback callback)
        {
            this.m_port = port;
            this.m_bits = bits;
            this.m_edge = edge;
            this.m_callback = callback;
        }

        internal uint Bits { get { return this.m_bits; } }
        /// <summary>
        /// GPIO port
        /// </summary>
        public int Port { get { return unchecked((int)this.m_port); } }
        /// <summary>
        /// Edge
        /// </summary>
        public Edge Edge { get { return this.m_edge; } }
        /// <summary>
        /// Callback method
        /// </summary>
        public GpioCallback Callback { get { return this.m_callback; } }
    }
}
