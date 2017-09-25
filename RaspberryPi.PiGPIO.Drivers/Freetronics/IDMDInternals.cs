using System;
using System.Collections.Generic;
using System.Text;


namespace RaspberryPi.PiGPIO.Drivers.Freetronics
{
    internal interface IDMDInternals
    {
        IPiGPIO Gpio { get; }
        void SetPixel(int x, int y, bool value);
        void Scan(int scan);
        void ScanFull();
        int PanelsWide { get; }
        int PanelsTall { get; }
    }
}
