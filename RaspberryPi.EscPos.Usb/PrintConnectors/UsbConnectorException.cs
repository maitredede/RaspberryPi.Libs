using System;
using System.Collections.Generic;
using System.Text;

namespace RaspberryPi.EscPos.PrintConnectors
{
    public class UsbConnectorException : Exception
    {
        public UsbConnectorException(string message) : base(message)
        {

        }
    }
}
