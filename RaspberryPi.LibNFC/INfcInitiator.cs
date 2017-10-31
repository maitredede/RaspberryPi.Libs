using System;
using System.Collections.Generic;
using System.Text;

namespace RaspberryPi.LibNFC
{
    public interface INfcInitiator
    {
        NfcTarget Poll(NfcModulation[] modulations, byte pollNr, byte period);
    }
}
