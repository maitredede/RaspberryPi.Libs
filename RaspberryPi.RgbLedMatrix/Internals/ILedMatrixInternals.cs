using System;
using System.Collections.Generic;
using System.Text;

namespace RaspberryPi.RgbLedMatrix.Internals
{
    public interface ILedMatrixInternals
    {
        IntPtr Handle { get; }
    }
}
