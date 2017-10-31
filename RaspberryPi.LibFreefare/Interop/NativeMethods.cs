using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace RaspberryPi.LibFreefare.Interop
{
    internal static class NativeMethods
    {
        public const string LIB = "libfreefare";

        [DllImport(LIB, EntryPoint = "freefare_get_tags", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr freefare_get_tags(IntPtr nfcDevice);
        [DllImport(LIB, EntryPoint = "freefare_free_tags", CallingConvention = CallingConvention.Cdecl)]
        public static extern void freefare_free_tags(IntPtr tags);
        [DllImport(LIB, EntryPoint = "freefare_free_tag", CallingConvention = CallingConvention.Cdecl)]
        public static extern void freefare_free_tag(IntPtr tag);

        [DllImport(LIB, EntryPoint = "freefare_get_tag_friendly_name", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr freefare_get_tag_friendly_name(IntPtr tag);
        [DllImport(LIB, EntryPoint = "freefare_get_tag_type", CallingConvention = CallingConvention.Cdecl)]
        public static extern TagType freefare_get_tag_type(IntPtr tag);
        [DllImport(LIB, EntryPoint = "freefare_get_tag_uid", CallingConvention = CallingConvention.Cdecl)]
        public static extern string freefare_get_tag_uid(IntPtr tag);

        //FreefareTag freefare_tag_new(nfc_device* device, nfc_target target);
        //bool freefare_selected_tag_is_present(nfc_device* device);

        //const char* freefare_strerror(FreefareTag tag);
        //int freefare_strerror_r(FreefareTag tag, char* buffer, size_t len);
        //void freefare_perror(FreefareTag tag, const char*string);



        //bool felica_taste(nfc_device* device, nfc_target target);

        //#define FELICA_SC_RW 0x0009
        //#define FELICA_SC_RO 0x000b

        //FreefareTag felica_tag_new(nfc_device* device, nfc_target target);
        //void felica_tag_free(FreefareTag tag);

        //ssize_t felica_read(FreefareTag tag, uint16_t service, uint8_t block, uint8_t* data, size_t length);
        //ssize_t felica_read_ex(FreefareTag tag, uint16_t service, uint8_t block_count, uint8_t blocks[], uint8_t* data, size_t length);
        //ssize_t felica_write(FreefareTag tag, uint16_t service, uint8_t block, uint8_t* data, size_t length);
        //ssize_t felica_write_ex(FreefareTag tag, uint16_t service, uint8_t block_count, uint8_t blocks[], uint8_t* data, size_t length);



        //bool mifare_ultralight_taste(nfc_device* device, nfc_target target);
        //bool mifare_ultralightc_taste(nfc_device* device, nfc_target target);
        //FreefareTag mifare_ultralight_tag_new(nfc_device* device, nfc_target target);
        //FreefareTag mifare_ultralightc_tag_new(nfc_device* device, nfc_target target);
        //void mifare_ultralight_tag_free(FreefareTag tag);
        //void mifare_ultralightc_tag_free(FreefareTag tag);

        //int mifare_ultralight_connect(FreefareTag tag);
        //int mifare_ultralight_disconnect(FreefareTag tag);

        //int mifare_ultralight_read(FreefareTag tag, const MifareUltralightPageNumber page, MifareUltralightPage *data);
        //int mifare_ultralight_write(FreefareTag tag, const MifareUltralightPageNumber page, const MifareUltralightPage data);

        //int mifare_ultralightc_authenticate(FreefareTag tag, const MifareDESFireKey key);
        //bool is_mifare_ultralight(FreefareTag tag);
        //bool is_mifare_ultralightc(FreefareTag tag);
        //bool is_mifare_ultralightc_on_reader(nfc_device* device, nfc_iso14443a_info nai);



        //bool ntag21x_taste(nfc_device* device, nfc_target target);
    }
}
