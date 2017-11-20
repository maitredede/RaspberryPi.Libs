using System;
using RaspberryPi.EscPos;
using RaspberryPi.EscPos.PrintConnectors;
using LibUsbDotNet;

namespace EscPosTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            UsbPrintConnector usbConnector = UsbPrintConnector.Open(0x04b8, 0x0202);
            if (usbConnector == null)
            {
                Console.WriteLine("Printer not found");
                return;
            }
            using (usbConnector)
            using (EscPosPrinter printer = new EscPosPrinter(usbConnector, false))
            {
                IRawPrinter raw = (IRawPrinter)printer;
                raw.LF();
            }
        }
    }
}
