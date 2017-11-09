using System;
using System.Collections.Generic;
using System.Text;

namespace OpenGlToMatrix.SimpleGLES
{
    /// <summary>
	/// Used in GL.Clear
	/// </summary>
	[Flags]
    public enum ClearBufferMask
    {
        /// <summary>
        /// Original was GL_DEPTH_BUFFER_BIT = 0x00000100
        /// </summary>
        DepthBufferBit = 256,
        /// <summary>
        /// Original was GL_ACCUM_BUFFER_BIT = 0x00000200
        /// </summary>
        AccumBufferBit = 512,
        /// <summary>
        /// Original was GL_STENCIL_BUFFER_BIT = 0x00000400
        /// </summary>
        StencilBufferBit = 1024,
        /// <summary>
        /// Original was GL_COLOR_BUFFER_BIT = 0x00004000
        /// </summary>
        ColorBufferBit = 16384,
        /// <summary>
        /// Original was GL_COVERAGE_BUFFER_BIT_NV = 0x00008000
        /// </summary>
        CoverageBufferBitNv = 32768
    }
}
