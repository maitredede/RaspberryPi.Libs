using System;
using RaspberryPi.EscPos;
using RaspberryPi.EscPos.PrintConnectors;
using LibUsbDotNet;
using RaspberryPi.Interop;

namespace EscPosTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            IntPtr ptr = NativeMethods.LoadLib("libusb-1.0.so");
            // if(ptr == IntPtr.Zero)
            //     ptr = NativeMethods.LoadLib("libusb.so");
            if(ptr==IntPtr.Zero)
                ptr=NativeMethods.LoadLib("/home/damien/perso/libusb/libusb/.libs/libusb-1.0.so");

            Console.WriteLine("LibUsb at {0}", ptr);
            UsbPrintConnector usbConnector = UsbPrintConnector.Open(0x04b8, 0x0202);
            if (usbConnector == null)
            {
                Console.WriteLine("Printer not found");
                return;
            }
            Console.WriteLine("ok");

            using (usbConnector)
            using (EscPosPrinter printer = new EscPosPrinter(usbConnector, false))
            {
                IRawPrinter raw = (IRawPrinter)printer;
                raw.LF();
            }
        }
    }
}
