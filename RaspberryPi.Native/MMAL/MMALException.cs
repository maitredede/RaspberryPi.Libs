using RaspberryPi.MMAL.Interop;
using System;
using System.Collections.Generic;
using System.Text;

namespace RaspberryPi.MMAL
{
    public sealed class MMALException : Exception
    {
        private readonly MMAL_STATUS_T m_status;

        internal MMALException(MMAL_STATUS_T status) : base(status.ToString())
        {
            this.m_status = status;
        }
    }
}
