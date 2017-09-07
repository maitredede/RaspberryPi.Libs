using System;
using System.Collections.Generic;
using System.Text;

namespace RaspberryPi.Userland
{
    public class DispmanException : Exception
    {
        internal DispmanException(string message) : base(message)
        {

        }
    }
}
