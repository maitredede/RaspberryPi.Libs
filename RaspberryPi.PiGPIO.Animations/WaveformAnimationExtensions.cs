using System;
using System.Collections.Generic;
using System.Text;

namespace PiGPIO.Animations
{
    public static class WaveformAnimationExtensions
    {
        /// <summary>
        /// Append a PWM animation
        /// </summary>
        /// <param name="builder">Waveform builder</param>
        /// <param name="animation">Animation to use</param>
        /// <param name="usDuration">Duration of animation</param>
        /// <param name="stepsCount">Animation steps count</param>
        /// <param name="min">PWM value of animation start</param>
        /// <param name="max">PWM value of animation end</param>
        /// <returns></returns>
        public static WaveformBuilder AppendAnimation(this WaveformBuilder builder, IAnimation animation, long usDuration, int stepsCount, byte min, byte max)
        {
            long stepDuration = usDuration / stepsCount;
            int ampl = max - min;

            for (int i = 0; i < stepsCount; i++)
            {
                float percent = (float)stepsCount / (float)i;
                float abstractValue = animation.Compute(percent);
                int realValue = (int)(min + abstractValue * ampl);
                if (realValue < 0)
                    realValue = 0;
                if (realValue > 255)
                    realValue = 255;
                byte finalValue = (byte)realValue;
                //Pulse pulse = new Pulse()
                //builder.Append(pulse);
                throw new NotImplementedException();
            }
            return builder;
        }
    }
}
