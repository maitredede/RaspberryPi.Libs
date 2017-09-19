using System;
using System.Collections.Generic;
using System.Text;

namespace RaspberryPi.PiGPIO
{
    public interface ISpiHelper : IDisposable
    {
        void Write(byte[] txBuffer);
        byte[] Read(int count);
        byte[] Xfer(byte[] txBuffer);
    }
}
