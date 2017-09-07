using System;
using System.Collections.Generic;
using System.Text;

namespace PiGPIO.Animations
{
    public interface IAnimation
    {
        float Compute(float percent);
    }
}
