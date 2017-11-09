using System;
using System.Collections.Generic;
using System.Text;

namespace OpenGlToMatrix.SimpleGLES
{
    /// <summary>
    /// Used in GL.Hint
    /// </summary>
    public enum HintMode
    {
        /// <summary>
        /// Original was GL_DONT_CARE = 0x1100
        /// </summary>
        DontCare = 4352,
        /// <summary>
        /// Original was GL_Fastest = 0X1101
        /// </summary>
        Fastest,
        /// <summary>
        /// Original was GL_Nicest = 0X1102
        /// </summary>
        Nicest
    }
}
