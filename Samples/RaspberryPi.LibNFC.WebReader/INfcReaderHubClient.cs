using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RaspberryPi.LibNFC.WebReader
{
    public interface INfcReaderHubClient
    {
        void setVersion(string version);
        void cardOn();
        void cardOff();
        void cardData(object data);
    }
}
