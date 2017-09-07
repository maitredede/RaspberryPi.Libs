using System;
using System.Collections.Generic;
using System.Text;

namespace RaspberryPi
{
    [Flags]
    public enum DISPMANX_TRANSFORM_T
    {
        /* Bottom 2 bits sets the orientation */
        DISPMANX_NO_ROTATE = 0,
        DISPMANX_ROTATE_90 = 1,
        DISPMANX_ROTATE_180 = 2,
        DISPMANX_ROTATE_270 = 3,

        DISPMANX_FLIP_HRIZ = 1 << 16,
        DISPMANX_FLIP_VERT = 1 << 17,

        /* invert left/right images */
        DISPMANX_STEREOSCOPIC_INVERT = 1 << 19,
        /* extra flags for controlling 3d duplication behaviour */
        DISPMANX_STEREOSCOPIC_NONE = 0 << 20,
        DISPMANX_STEREOSCOPIC_MONO = 1 << 20,
        DISPMANX_STEREOSCOPIC_SBS = 2 << 20,
        DISPMANX_STEREOSCOPIC_TB = 3 << 20,
        DISPMANX_STEREOSCOPIC_MASK = 15 << 20,

        /* extra flags for controlling snapshot behaviour */
        DISPMANX_SNAPSHOT_NO_YUV = 1 << 24,
        DISPMANX_SNAPSHOT_NO_RGB = 1 << 25,
        DISPMANX_SNAPSHOT_FILL = 1 << 26,
        DISPMANX_SNAPSHOT_SWAP_RED_BLUE = 1 << 27,
        DISPMANX_SNAPSHOT_PACK = 1 << 28
    }
}
