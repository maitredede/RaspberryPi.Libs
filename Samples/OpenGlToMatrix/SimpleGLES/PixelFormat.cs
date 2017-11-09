using System;
using System.Collections.Generic;
using System.Text;

namespace OpenGlToMatrix.SimpleGLES
{
    /// <summary>
    /// Used in GL.CompressedTexSubImage2D, GL.ReadnPixels and 6 other functions
    /// </summary>
    public enum PixelFormat
    {
        /// <summary>
        /// Original was GL_UNSIGNED_SHORT = 0x1403
        /// </summary>
        UnsignedShort = 5123,
        /// <summary>
        /// Original was GL_UNSIGNED_INT = 0x1405
        /// </summary>
        UnsignedInt = 5125,
        /// <summary>
        /// Original was GL_COLOR_INDEX = 0x1900
        /// </summary>
        ColorIndex = 6400,
        /// <summary>
        /// Original was GL_STENCIL_INDEX = 0x1901
        /// </summary>
        StencilIndex,
        /// <summary>
        /// Original was GL_DEPTH_COMPONENT = 0x1902
        /// </summary>
        DepthComponent,
        /// <summary>
        /// Original was GL_RED = 0x1903
        /// </summary>
        Red,
        /// <summary>
        /// Original was GL_RED_EXT = 0x1903
        /// </summary>
        RedExt = 6403,
        /// <summary>
        /// Original was GL_GREEN = 0x1904
        /// </summary>
        Green,
        /// <summary>
        /// Original was GL_BLUE = 0x1905
        /// </summary>
        Blue,
        /// <summary>
        /// Original was GL_Alpha = 0X1906
        /// </summary>
        Alpha,
        /// <summary>
        /// Original was GL_Rgb = 0X1907
        /// </summary>
        Rgb,
        /// <summary>
        /// Original was GL_Rgba = 0X1908
        /// </summary>
        Rgba,
        /// <summary>
        /// Original was GL_Luminance = 0X1909
        /// </summary>
        Luminance,
        /// <summary>
        /// Original was GL_LUMINANCE_ALPHA = 0x190A
        /// </summary>
        LuminanceAlpha,
        /// <summary>
        /// Original was GL_ABGR_EXT = 0x8000
        /// </summary>
        AbgrExt = 32768,
        /// <summary>
        /// Original was GL_CMYK_EXT = 0x800C
        /// </summary>
        CmykExt = 32780,
        /// <summary>
        /// Original was GL_CMYKA_EXT = 0x800D
        /// </summary>
        CmykaExt,
        /// <summary>
        /// Original was GL_YCRCB_422_SGIX = 0x81BB
        /// </summary>
        Ycrcb422Sgix = 33211,
        /// <summary>
        /// Original was GL_YCRCB_444_SGIX = 0x81BC
        /// </summary>
        Ycrcb444Sgix
    }
}
