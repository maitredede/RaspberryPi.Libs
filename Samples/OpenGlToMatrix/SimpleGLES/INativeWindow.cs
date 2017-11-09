using System;
using System.Collections.Generic;
using System.Text;

namespace OpenGlToMatrix.SimpleGLES
{
    public interface INativeWindow
    {
        IntPtr Handle { get; }
    }
}
