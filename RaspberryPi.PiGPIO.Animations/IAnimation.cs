using System;
using System.Collections.Generic;
using System.Text;

namespace RaspberryPi.PiGPIO.Animations
{
    public interface IAnimation
    {
        float Compute(float percent);
    }
}
