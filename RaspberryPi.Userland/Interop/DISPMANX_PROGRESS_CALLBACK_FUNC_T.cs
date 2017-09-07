using System;
using System.Collections.Generic;
using System.Text;

namespace RaspberryPi.Userland.Interop
{
    internal delegate void DISPMANX_PROGRESS_CALLBACK_FUNC_T(DISPMANX_UPDATE_HANDLE_T u, uint line, IntPtr arg);
}
