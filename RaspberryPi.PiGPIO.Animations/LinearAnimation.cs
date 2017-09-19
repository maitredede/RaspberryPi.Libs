using System;
using System.Collections.Generic;
using System.Text;

namespace RaspberryPi.PiGPIO.Animations
{
    public sealed class LinearAnimation : IAnimation
    {
        public float Compute(float percent)
        {
            return percent;
        }
    }
}
