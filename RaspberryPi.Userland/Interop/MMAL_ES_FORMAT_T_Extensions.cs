using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace RaspberryPi.Userland.Interop
{
    internal static class MMAL_ES_FORMAT_T_Extensions
    {
        public static MMAL_VIDEO_FORMAT_T getEsAsVideoFormat(this MMAL_ES_FORMAT_T format)
        {
            //if (es == IntPtr.Zero)
            //    return null;
            return Marshal.PtrToStructure<MMAL_VIDEO_FORMAT_T>(format.es);
        }

        public static MMAL_AUDIO_FORMAT_T getEsAsAudioFormat(this MMAL_ES_FORMAT_T format)
        {
            //if (es == IntPtr.Zero)
            //    return null;
            return Marshal.PtrToStructure<MMAL_AUDIO_FORMAT_T>(format.es);
        }

        public static MMAL_SUBPICTURE_FORMAT_T getEsAsSubPictureFormat(this MMAL_ES_FORMAT_T format)
        {
            //if (es == IntPtr.Zero)
            //    return null;
            return Marshal.PtrToStructure<MMAL_SUBPICTURE_FORMAT_T>(format.es);
        }
    }
}
