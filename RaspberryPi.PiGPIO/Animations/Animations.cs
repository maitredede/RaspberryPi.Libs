using System;
using System.Collections.Generic;
using System.Text;

namespace PiGPIO.Animations
{
    public static class Animations
    {
        private static LinearAnimation s_linear;

        public static LinearAnimation Linear
        {
            get
            {
                if (s_linear == null)
                    s_linear = new LinearAnimation();
                return s_linear;
            }
        }
    }
}
