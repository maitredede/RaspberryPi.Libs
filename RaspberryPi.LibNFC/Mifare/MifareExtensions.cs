using RaspberryPi.LibNFC.Interop;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace RaspberryPi.LibNFC.Mifare
{
    public static class MifareExtensions
    {
        public static bool TasteMifareMini(this NfcTarget target)
        {
            NfcModulation nm = target.ReadModulation();
            nfc_iso14443a_info nai = target.ReadInfo<nfc_iso14443a_info>();
            return nm.ModulationType == NfcModulationType.ISO14443A && (
                nai.btSak == 0x09
                );
        }

        public static bool TasteMifareClassic1k(this NfcTarget target)
        {
            NfcModulation nm = target.ReadModulation();
            nfc_iso14443a_info nai = target.ReadInfo<nfc_iso14443a_info>();
            return nm.ModulationType == NfcModulationType.ISO14443A && (
                nai.btSak == 0x08 ||
                nai.btSak == 0x28 ||
                nai.btSak == 0x68 ||
                nai.btSak == 0x88
                );
        }

        public static bool TasteMifareClassic4k(this NfcTarget target)
        {
            NfcModulation nm = target.ReadModulation();
            nfc_iso14443a_info nai = target.ReadInfo<nfc_iso14443a_info>();
            return nm.ModulationType == NfcModulationType.ISO14443A && (
                nai.btSak == 0x18 ||
                nai.btSak == 0x38
                );
        }
    }
}
