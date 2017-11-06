using RaspberryPi.LibFreefare;
using RaspberryPi.LibFreefare.Interop;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace RaspberryPi.LibNFC
{
    /// <summary>
    /// Extension methods for Freefare lib
    /// </summary>
    public static class FreefareExtensions
    {
        /// <summary>
        /// Get detected tags
        /// </summary>
        /// <param name="device">The device to get detected tags</param>
        /// <returns>List of detected devices</returns>
        public static FreefareTagList FreefareGetTags(this NfcDevice device)
        {
            IntPtr arrPtr = NativeMethods.freefare_get_tags(device.DangerousGetHandle());
            if (arrPtr == IntPtr.Zero)
                throw new FreefareException("Error listing tags");
            return new FreefareTagList(arrPtr, device);
        }

        public static bool IsNTag21x(this FreefareTag tag)
        {
            return NativeMethods.is_ntag21x(tag.Handle);
        }

        public static bool TasteNTag21x(this NfcTarget target)
        {
            return NativeMethods.ntag21x_taste(target.Device.DangerousGetHandle(), target.Handle);
        }

        public static bool TasteMifareMini(this NfcTarget target)
        {
            return NativeMethods.mifare_mini_taste(target.Device.DangerousGetHandle(), target.Handle);
        }
        public static bool TasteMifareClassic1k(this NfcTarget target)
        {
            return NativeMethods.mifare_classic1k_taste(target.Device.DangerousGetHandle(), target.Handle);
        }
        public static bool TasteMifareClassic4k(this NfcTarget target)
        {
            return NativeMethods.mifare_classic4k_taste(target.Device.DangerousGetHandle(), target.Handle);
        }
        public static FreefareTag AsFreefareTag(this NfcTarget target)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));
            IntPtr ptrTag = NativeMethods.freefare_tag_new(target.Device.DangerousGetHandle(), target.Handle);
            if (ptrTag == IntPtr.Zero)
            {
                Console.WriteLine("A");
                return null;
            }
            FreefareTag tag = FreefareTag.Build(ptrTag, true, target.Device, target);
            return tag;
        }
    }
}
