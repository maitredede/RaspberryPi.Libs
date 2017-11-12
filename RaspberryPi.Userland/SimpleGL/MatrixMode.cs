using System;
using System.Collections.Generic;
using System.Text;

namespace RaspberryPi.Userland.SimpleGL
{
    /// <summary>
    /// Not used directly.
    /// </summary>
    public enum MatrixMode
    {
        /// <summary>
        /// Original was GL_MODELVIEW = 0x1700
        /// </summary>
        Modelview = 5888,
        /// <summary>
        /// Original was GL_MODELVIEW0_EXT = 0x1700
        /// </summary>
        Modelview0Ext = 5888,
        /// <summary>
        /// Original was GL_PROJECTION = 0x1701
        /// </summary>
        Projection,
        /// <summary>
        /// Original was GL_TEXTURE = 0x1702
        /// </summary>
        Texture
    }
}
