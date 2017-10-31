using System;
using System.Collections.Generic;
using System.Text;

namespace RaspberryPi.LibNFC
{
    public interface INfcContext
    {
        string[] ListDevices();
        NfcDevice Open(string connectionString);
    }
}
