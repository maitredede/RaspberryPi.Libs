using System;
using System.Collections.Generic;
using System.Text;

namespace RaspberryPi.MMAL.Interop
{
    public enum MMAL_ES_TYPE_T
    {
        MMAL_ES_TYPE_UNKNOWN,     /**< Unknown elementary stream type */
        MMAL_ES_TYPE_CONTROL,     /**< Elementary stream of control commands */
        MMAL_ES_TYPE_AUDIO,       /**< Audio elementary stream */
        MMAL_ES_TYPE_VIDEO,       /**< Video elementary stream */
        MMAL_ES_TYPE_SUBPICTURE   /**< Sub-picture elementary stream (e.g. subtitles, overlays) */
    }
}
