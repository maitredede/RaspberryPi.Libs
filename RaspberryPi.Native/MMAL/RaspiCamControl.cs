using System;
using System.Collections.Generic;
using System.Text;

namespace RaspberryPi.MMAL
{
    public sealed class RaspiCamControl
    {
        private readonly CameraComponent m_cam;

        internal RaspiCamControl(CameraComponent cam)
        {
            this.m_cam = cam;
        }

    }
}
