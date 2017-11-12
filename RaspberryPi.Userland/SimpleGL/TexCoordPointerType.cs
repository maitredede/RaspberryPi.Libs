using System;
using System.Collections.Generic;
using System.Text;

namespace RaspberryPi.Userland.SimpleGL
{
    /// <summary>
	/// Used in GL.TexCoordPointer
	/// </summary>
	public enum TexCoordPointerType
    {
        /// <summary>
        /// Original was GL_SHORT = 0x1402
        /// </summary>
        Short = 5122,
        /// <summary>
        /// Original was GL_INT = 0x1404
        /// </summary>
        Int = 5124,
        /// <summary>
        /// Original was GL_FLOAT = 0x1406
        /// </summary>
        Float = 5126,
        /// <summary>
        /// Original was GL_DOUBLE = 0x140A
        /// </summary>
        Double = 5130
    }
}
