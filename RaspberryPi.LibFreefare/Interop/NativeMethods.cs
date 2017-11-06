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

        [DllImport(LIB, EntryPoint = "freefare_strerror", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr freefare_strerror(IntPtr tag);

        [DllImport(LIB, EntryPoint = "freefare_selected_tag_is_present", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool freefare_selected_tag_is_present(IntPtr device);

        [DllImport(LIB, EntryPoint = "freefare_tag_new", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr freefare_tag_new(IntPtr device, IntPtr target);

        //FreefareTag freefare_tag_new(nfc_device* device, nfc_target target);
        //int freefare_strerror_r(FreefareTag tag, char* buffer, size_t len);
        //void freefare_perror(FreefareTag tag, const char*string);

        #region ntag21x
        public static readonly int NTAG_PROT = 0x80;
        public static readonly int NTAG_CFGLCK = 0x40;
        public static readonly int NTAG_NFC_CNT_EN = 0x20;
        public static readonly int NTAG_NFC_CNT_PWD_PROT = 0x10;
        public static readonly int NTAG_AUTHLIM = 0x07;

        [DllImport(LIB, EntryPoint = "ntag21x_get_subtype", CallingConvention = CallingConvention.Cdecl)]
        public static extern NTagSubtype ntag21x_get_subtype(IntPtr tag);
        [DllImport(LIB, EntryPoint = "is_ntag21x", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool is_ntag21x(IntPtr tag);

        [DllImport(LIB, EntryPoint = "ntag21x_connect", CallingConvention = CallingConvention.Cdecl)]
        public static extern int ntag21x_connect(IntPtr tag);
        [DllImport(LIB, EntryPoint = "ntag21x_disconnect", CallingConvention = CallingConvention.Cdecl)]
        public static extern int ntag21x_disconnect(IntPtr tag);
        [DllImport(LIB, EntryPoint = "ntag21x_get_info", CallingConvention = CallingConvention.Cdecl)]
        public static extern int ntag21x_get_info(IntPtr tag);

        [DllImport(LIB, EntryPoint = "ntag21x_key_new", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr ntag21x_key_new(IntPtr data, IntPtr pack);
        [DllImport(LIB, EntryPoint = "ntag21x_key_free", CallingConvention = CallingConvention.Cdecl)]
        public static extern void ntag21x_key_free(IntPtr key);

        //[DllImport(LIB, EntryPoint = "ntag21x_is_auth_supported", CallingConvention = CallingConvention.Cdecl)]
        //public static extern bool ntag21x_is_auth_supported(IntPtr device, nfc_iso14443a_info nai);

        [DllImport(LIB, EntryPoint = "ntag21x_taste", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool ntag21x_taste(IntPtr device, IntPtr target);

        //        FreefareTag ntag21x_tag_new(nfc_device* device, nfc_target target);
        //        FreefareTag ntag21x_tag_reuse(FreefareTag tag);  /* Copy data from Ultralight tag to new NTAG21x, don't forget to free your old tag */
        //        void ntag21x_tag_free(FreefareTag tag);

        //        uint8_t ntag21x_get_last_page(FreefareTag tag);  /* Get last page address based on gathered info from function above */
        //        int ntag21x_read_signature(FreefareTag tag, uint8_t* data); /* Get tag signature */
        //        int ntag21x_set_pwd(FreefareTag tag, uint8_t data[4]);  /* Set password */
        //        int ntag21x_set_pack(FreefareTag tag, uint8_t data[2]);  /* Set pack */
        //        int ntag21x_set_key(FreefareTag tag, const NTAG21xKey key); /* Set key */
        //        int ntag21x_set_auth(FreefareTag tag, uint8_t byte); /* Set AUTH0 byte (from which page starts password protection) */
        //        int ntag21x_get_auth(FreefareTag tag, uint8_t*byte); /* Get AUTH0 byte */
        //        int ntag21x_access_enable(FreefareTag tag, uint8_t byte); /* Enable access feature in ACCESS byte */
        //        int ntag21x_access_disable(FreefareTag tag, uint8_t byte); /* Disable access feature in ACCESS byte */
        //        int ntag21x_get_access(FreefareTag tag, uint8_t*byte); /* Get ACCESS byte */
        //        int ntag21x_check_access(FreefareTag tag, uint8_t byte, bool* result); /* Check if access feature is enabled */
        //        int ntag21x_get_authentication_limit(FreefareTag tag, uint8_t*byte); /* Get authentication limit */
        //        int ntag21x_set_authentication_limit(FreefareTag tag, uint8_t byte); /* Set authentication limit (0x00 = disabled, [0x01,0x07] = valid range, > 0x07 invalid range) */
        //        int ntag21x_read(FreefareTag tag, uint8_t page, uint8_t* data); /* Read 16 bytes starting from page */
        //        int ntag21x_read4(FreefareTag tag, uint8_t page, uint8_t* data); /* Read 4 bytes on page */
        //        int ntag21x_fast_read(FreefareTag tag, uint8_t start_page, uint8_t end_page, uint8_t* data); /* Read n*4 bytes from range [start_page,end_page] */
        //        int ntag21x_fast_read4(FreefareTag tag, uint8_t page, uint8_t* data); /* Fast read certain page */
        //        int ntag21x_read_cnt(FreefareTag tag, uint8_t* data);  /* Read 3-byte NFC counter if enabled else it returns error */
        //        int ntag21x_write(FreefareTag tag, uint8_t page, uint8_t data[4]);  /* Write 4 bytes to page */
        //        int ntag21x_compatibility_write(FreefareTag tag, uint8_t page, uint8_t data[4]);  /* Writes 4 bytes to page with mifare classic write */
        //        int ntag21x_authenticate(FreefareTag tag, const NTAG21xKey key);  /* Authenticate with tag */
        //        bool ntag21x_is_auth_supported(nfc_device* device, nfc_iso14443a_info nai);  /* Check if tag supports 21x commands */

        #endregion

        #region mifare_classic
        [DllImport(LIB, EntryPoint = "mifare_mini_taste", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool mifare_mini_taste(IntPtr device, IntPtr target);
        [DllImport(LIB, EntryPoint = "mifare_classic1k_taste", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool mifare_classic1k_taste(IntPtr device, IntPtr target);
        [DllImport(LIB, EntryPoint = "mifare_classic4k_taste", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool mifare_classic4k_taste(IntPtr device, IntPtr target);

        //FreefareTag mifare_mini_tag_new(nfc_device* device, nfc_target target);
        //FreefareTag mifare_classic1k_tag_new(nfc_device* device, nfc_target target);
        //FreefareTag mifare_classic4k_tag_new(nfc_device* device, nfc_target target);
        //void mifare_classic_tag_free(FreefareTag tag);

        //typedef unsigned char MifareClassicBlock[16];

        //typedef uint8_t MifareClassicSectorNumber;
        //typedef unsigned char MifareClassicBlockNumber;

        [DllImport(LIB, EntryPoint = "mifare_classic_connect", CallingConvention = CallingConvention.Cdecl)]
        public static extern int mifare_classic_connect(IntPtr tag);
        [DllImport(LIB, EntryPoint = "mifare_classic_disconnect", CallingConvention = CallingConvention.Cdecl)]
        public static extern int mifare_classic_disconnect(IntPtr tag);

        [DllImport(LIB, EntryPoint = "mifare_classic_authenticate", CallingConvention = CallingConvention.Cdecl)]
        public static extern int mifare_classic_authenticate(IntPtr tag, byte block, IntPtr key, MifareClassicKeyType keyType);

        //int mifare_classic_authenticate(FreefareTag tag, const MifareClassicBlockNumber block, const MifareClassicKey key, const MifareClassicKeyType key_type);
        //int mifare_classic_read(FreefareTag tag, const MifareClassicBlockNumber block, MifareClassicBlock *data);
        //int mifare_classic_init_value(FreefareTag tag, const MifareClassicBlockNumber block, const int32_t value, const MifareClassicBlockNumber adr);
        //int mifare_classic_read_value(FreefareTag tag, const MifareClassicBlockNumber block, int32_t *value, MifareClassicBlockNumber* adr);
        //int mifare_classic_write(FreefareTag tag, const MifareClassicBlockNumber block, const MifareClassicBlock data);

        //int mifare_classic_increment(FreefareTag tag, const MifareClassicBlockNumber block, const uint32_t amount);
        //int mifare_classic_decrement(FreefareTag tag, const MifareClassicBlockNumber block, const uint32_t amount);
        //int mifare_classic_restore(FreefareTag tag, const MifareClassicBlockNumber block);
        //int mifare_classic_transfer(FreefareTag tag, const MifareClassicBlockNumber block);

        //int mifare_classic_get_trailer_block_permission(FreefareTag tag, const MifareClassicBlockNumber block, const uint16_t permission, const MifareClassicKeyType key_type);
        //int mifare_classic_get_data_block_permission(FreefareTag tag, const MifareClassicBlockNumber block, const unsigned char permission, const MifareClassicKeyType key_type);

        //int mifare_classic_format_sector(FreefareTag tag, const MifareClassicSectorNumber sector);

        //void mifare_classic_trailer_block(MifareClassicBlock* block, const MifareClassicKey key_a, uint8_t ab_0, uint8_t ab_1, uint8_t ab_2, uint8_t ab_tb, const uint8_t gpb, const MifareClassicKey key_b);

        //MifareClassicSectorNumber mifare_classic_block_sector(MifareClassicBlockNumber block);
        //MifareClassicBlockNumber mifare_classic_sector_first_block(MifareClassicSectorNumber sector);
        //size_t mifare_classic_sector_block_count(MifareClassicSectorNumber sector);
        //MifareClassicBlockNumber mifare_classic_sector_last_block(MifareClassicSectorNumber sector);

        //#define C_000 0
        //#define C_001 1
        //#define C_010 2
        //#define C_011 3
        //#define C_100 4
        //#define C_101 5
        //#define C_110 6
        //#define C_111 7
        //#define C_DEFAULT 255

        ///* MIFARE Classic Access Bits */
        //#define MCAB_R 0x8
        //#define MCAB_W 0x4
        //#define MCAB_D 0x2
        //#define MCAB_I 0x1

        //#define MCAB_READ_KEYA         0x400
        //#define MCAB_WRITE_KEYA        0x100
        //#define MCAB_READ_ACCESS_BITS  0x040
        //#define MCAB_WRITE_ACCESS_BITS 0x010
        //#define MCAB_READ_KEYB         0x004
        //#define MCAB_WRITE_KEYB        0x001
        #endregion
    }
}
