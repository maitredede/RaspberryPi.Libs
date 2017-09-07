using System;
using System.Collections.Generic;
using System.Text;

namespace RaspberryPi.MMAL
{
    public enum MMAL_PARAMETER_CAMERA_INFO_FLASH_TYPE_T
    {
        MMAL_PARAMETER_CAMERA_INFO_FLASH_TYPE_XENON = 0, /* Make values explicit */
        MMAL_PARAMETER_CAMERA_INFO_FLASH_TYPE_LED = 1, /* to ensure they match */
        MMAL_PARAMETER_CAMERA_INFO_FLASH_TYPE_OTHER = 2, /* values in config ini */
    }
}
