using System;
using System.Runtime.InteropServices;

namespace RaspberryPi.LibUsb
{
    public class LibUsbException : Exception
    {
        public LibUsbError Error {get;}

        public LibUsbException(string message, LibUsbError error) 
        : base(message) {
            this.Error = error;
         }
        public LibUsbException(string message, System.Exception inner) 
        : base(message, inner) { 
        }
    }
}