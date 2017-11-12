using System;
using System.Collections.Generic;
using System.Text;

namespace RaspberryPi.Userland.SimpleGL
{
    /// <summary>
	/// Used in GL.CompressedTexImage2D, GL.CopyTexImage2D and 1 other function
	/// </summary>
	public enum PixelInternalFormat
    {
        /// <summary>
        /// Original was GL_Alpha = 0X1906
        /// </summary>
        Alpha = 6406,
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
        /// Original was GL_LuminanceAlpha = 0X190a
        /// </summary>
        LuminanceAlpha
    }
}
