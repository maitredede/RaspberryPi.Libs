using System;
using System.Collections.Generic;
using System.Text;

namespace RaspberryPi.LibFreefare
{
    public class FreefareException : Exception
    {
        internal FreefareException(string message) : base(message)
        {
        }
    }
}
