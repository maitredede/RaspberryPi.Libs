using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace RaspberryPi.Userland.Interop
{
    public enum MMAL_PORT_TYPE_T
    {
        MMAL_PORT_TYPE_UNKNOWN = 0,          /**< Unknown port type */
        MMAL_PORT_TYPE_CONTROL,              /**< Control port */
        MMAL_PORT_TYPE_INPUT,                /**< Input port */
        MMAL_PORT_TYPE_OUTPUT,               /**< Output port */
        MMAL_PORT_TYPE_CLOCK,                /**< Clock port */
        MMAL_PORT_TYPE_INVALID = unchecked((int)0xffffffff)  /**< Dummy value to force 32bit enum */
    }
}
