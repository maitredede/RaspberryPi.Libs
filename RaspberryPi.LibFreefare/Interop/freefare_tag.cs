using RaspberryPi.LibNFC.Interop;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace RaspberryPi.LibFreefare.Interop
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct freefare_tag
    {
        public IntPtr device;
        public nfc_target target;
        public TagType type;
        public bool active;
        public IntPtr tag;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct ntag21x_tag
    {
        public freefare_tag __tag;
        public int subtype;
        public byte vendor_id;
        public byte product_type;
        public byte product_subtype;
        public byte major_product_version;
        public byte minor_product_version;
        public byte storage_size;
        public byte protocol_type;
    }
}
