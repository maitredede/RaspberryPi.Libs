using System;
using System.Collections.Generic;
using System.Text;

namespace RaspberryPi.EscPos.PrintConnectors
{
    public interface IPrintConnector : IDisposable
    {
        int Read(byte[] buffer, int offset, int count);
        void Write(byte[] buffer, int offset, int count);
    }
}
