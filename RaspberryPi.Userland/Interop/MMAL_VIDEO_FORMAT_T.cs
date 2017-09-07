using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using MMAL_FOURCC_T = System.UInt32;

namespace RaspberryPi.Userland.Interop
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MMAL_VIDEO_FORMAT_T
    {
       public uint width;        /**< Width of frame in pixels */
        public uint height;       /**< Height of frame in rows of pixels */
        public MMAL_RECT_T crop;         /**< Visible region of the frame */
        public MMAL_RATIONAL_T frame_rate;   /**< Frame rate */
        public MMAL_RATIONAL_T par;          /**< Pixel aspect ratio */

        public MMAL_FOURCC_T color_space;  /**< FourCC specifying the color space of the
                                   * video stream. See the \ref MmalColorSpace
                                   * "pre-defined color spaces" for some examples.
                                   */
    }
}
