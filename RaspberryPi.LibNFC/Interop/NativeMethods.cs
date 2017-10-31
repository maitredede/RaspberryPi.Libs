using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Text;

namespace RaspberryPi.LibNFC.Interop
{
    internal static class NativeMethods
    {
        public const string LIB = "libnfc";
        public const int NFC_BUFSIZE_CONNSTRING = 1024;

        #region Library initialization/deinitialization
        [DllImport(LIB, EntryPoint = "nfc_init", CallingConvention = CallingConvention.Cdecl)]
        public static extern void init(out IntPtr context);

        [DllImport(LIB, EntryPoint = "nfc_exit", CallingConvention = CallingConvention.Cdecl)]
        public static extern void exit(IntPtr context);

        [DllImport(LIB, EntryPoint = "nfc_register_driver", CallingConvention = CallingConvention.Cdecl)]
        public static extern NfcError register_driver(IntPtr driver);
        #endregion

        #region NFC Device/Hardware manipulation
        [DllImport(LIB, EntryPoint = "nfc_open", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr open(IntPtr context, [MarshalAs(UnmanagedType.LPStr, SizeConst = NFC_BUFSIZE_CONNSTRING)]string connestring);

        [DllImport(LIB, EntryPoint = "nfc_close", CallingConvention = CallingConvention.Cdecl)]
        public static extern void close(IntPtr pnd);

        [DllImport(LIB, EntryPoint = "nfc_list_devices", CallingConvention = CallingConvention.Cdecl)]
        public static extern int list_devices(IntPtr context, IntPtr connstrings, int connstrings_len);
        //NFC_EXPORT size_t nfc_list_devices(nfc_context* context, nfc_connstring connstrings[], size_t connstrings_len) ATTRIBUTE_NONNULL(1);

        [DllImport(LIB, EntryPoint = "nfc_abort_command", CallingConvention = CallingConvention.Cdecl)]
        public static extern NfcError abort_command(IntPtr device);

        [DllImport(LIB, EntryPoint = "nfc_idle", CallingConvention = CallingConvention.Cdecl)]
        public static extern NfcError idle(IntPtr device);
        #endregion

        #region NFC initiator: act as "reader"
        [DllImport(LIB, EntryPoint = "nfc_initiator_init", CallingConvention = CallingConvention.Cdecl)]
        public static extern NfcError initiator_init(IntPtr device);
        [DllImport(LIB, EntryPoint = "nfc_initiator_init_secure_element", CallingConvention = CallingConvention.Cdecl)]
        public static extern NfcError initiator_init_secure_element(IntPtr device);

        //[DllImport(LIB, EntryPoint = "nfc_initiator_poll_target", CallingConvention = CallingConvention.Cdecl)]
        ////public static extern int initiator_poll_target(IntPtr device,
        ////     [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)]
        ////    NfcModulation[] pnmTargetTypes, int szTargetTypes, byte uiPollNr, byte uiPeriod, out IntPtr pnt);
        //public static extern int initiator_poll_target(IntPtr device, IntPtr pnmTargetTypes, uint szTargetTypes, byte uiPollNr, byte uiPeriod, ref IntPtr pnt);

        [DllImport(LIB, EntryPoint = "nfc_initiator_poll_target", CallingConvention = CallingConvention.Cdecl)]
        public static extern int initiator_poll_target(IntPtr device,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)]NfcModulation[] pnmTargetTypes,
            uint szTargetTypes,
            byte uiPollNr,
            byte uiPeriod,
            IntPtr pnt);

        [DllImport(LIB, EntryPoint = "nfc_initiator_target_is_present", CallingConvention = CallingConvention.Cdecl)]
        public static extern NfcError initiator_target_is_present(IntPtr device, IntPtr pnt);

        //NFC_EXPORT int nfc_initiator_select_passive_target(nfc_device* pnd, const nfc_modulation nm, const uint8_t* pbtInitData, const size_t szInitData, nfc_target *pnt);
        //NFC_EXPORT int nfc_initiator_list_passive_targets(nfc_device* pnd, const nfc_modulation nm, nfc_target ant[], const size_t szTargets);
        //NFC_EXPORT int nfc_initiator_select_dep_target(nfc_device* pnd, const nfc_dep_mode ndm, const nfc_baud_rate nbr, const nfc_dep_info* pndiInitiator, nfc_target *pnt, const int timeout);
        //NFC_EXPORT int nfc_initiator_poll_dep_target(nfc_device* pnd, const nfc_dep_mode ndm, const nfc_baud_rate nbr, const nfc_dep_info* pndiInitiator, nfc_target *pnt, const int timeout);
        //NFC_EXPORT int nfc_initiator_deselect_target(nfc_device* pnd);
        //NFC_EXPORT int nfc_initiator_transceive_bytes(nfc_device* pnd, const uint8_t* pbtTx, const size_t szTx, uint8_t *pbtRx, const size_t szRx, int timeout);
        //NFC_EXPORT int nfc_initiator_transceive_bits(nfc_device* pnd, const uint8_t* pbtTx, const size_t szTxBits, const uint8_t* pbtTxPar, uint8_t *pbtRx, const size_t szRx, uint8_t *pbtRxPar);
        //NFC_EXPORT int nfc_initiator_transceive_bytes_timed(nfc_device* pnd, const uint8_t* pbtTx, const size_t szTx, uint8_t *pbtRx, const size_t szRx, uint32_t *cycles);
        //NFC_EXPORT int nfc_initiator_transceive_bits_timed(nfc_device* pnd, const uint8_t* pbtTx, const size_t szTxBits, const uint8_t* pbtTxPar, uint8_t *pbtRx, const size_t szRx, uint8_t *pbtRxPar, uint32_t* cycles);
        #endregion

        #region NFC target: act as tag (i.e. MIFARE Classic) or NFC target device
        //NFC_EXPORT int nfc_target_init(nfc_device* pnd, nfc_target* pnt, uint8_t* pbtRx, const size_t szRx, int timeout);
        //NFC_EXPORT int nfc_target_send_bytes(nfc_device* pnd, const uint8_t* pbtTx, const size_t szTx, int timeout);
        //NFC_EXPORT int nfc_target_receive_bytes(nfc_device* pnd, uint8_t* pbtRx, const size_t szRx, int timeout);
        //NFC_EXPORT int nfc_target_send_bits(nfc_device* pnd, const uint8_t* pbtTx, const size_t szTxBits, const uint8_t* pbtTxPar);
        //NFC_EXPORT int nfc_target_receive_bits(nfc_device* pnd, uint8_t* pbtRx, const size_t szRx, uint8_t *pbtRxPar);
        #endregion

        #region Error reporting
        [DllImport(LIB, EntryPoint = "nfc_strerror", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr strerror(IntPtr device);
        //NFC_EXPORT int nfc_strerror_r(const nfc_device* pnd, char* buf, size_t buflen);
        //NFC_EXPORT void nfc_perror(const nfc_device* pnd, const char* s);
        [DllImport(LIB, EntryPoint = "nfc_device_get_last_error", CallingConvention = CallingConvention.Cdecl)]
        public static extern NfcError device_get_last_error(IntPtr device);
        #endregion

        #region Special data accessors
        [DllImport(LIB, EntryPoint = "nfc_device_get_name", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr device_get_name(IntPtr device);
        [DllImport(LIB, EntryPoint = "nfc_device_get_connstring", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr device_get_connstring(IntPtr device);
        //NFC_EXPORT int nfc_device_get_supported_modulation(nfc_device* pnd, const nfc_mode mode,  const nfc_modulation_type**const supported_mt);
        //NFC_EXPORT int nfc_device_get_supported_baud_rate(nfc_device* pnd, const nfc_modulation_type nmt, const nfc_baud_rate**const supported_br);
        //NFC_EXPORT int nfc_device_get_supported_baud_rate_target_mode(nfc_device* pnd, const nfc_modulation_type nmt, const nfc_baud_rate**const supported_br);
        #endregion

        #region Properties accessors
        [DllImport(LIB, EntryPoint = "nfc_device_set_property_int", CallingConvention = CallingConvention.Cdecl)]
        public static extern NfcError device_set_property_int(IntPtr device, NfcProperty property, int value);
        [DllImport(LIB, EntryPoint = "nfc_device_set_property_bool", CallingConvention = CallingConvention.Cdecl)]
        public static extern NfcError device_set_property_bool(IntPtr device, NfcProperty property, bool value);
        #endregion

        #region Misc. functions
        //NFC_EXPORT void iso14443a_crc(uint8_t* pbtData, size_t szLen, uint8_t* pbtCrc);
        //NFC_EXPORT void iso14443a_crc_append(uint8_t* pbtData, size_t szLen);
        //NFC_EXPORT void iso14443b_crc(uint8_t* pbtData, size_t szLen, uint8_t* pbtCrc);
        //NFC_EXPORT void iso14443b_crc_append(uint8_t* pbtData, size_t szLen);
        //NFC_EXPORT uint8_t *iso14443a_locate_historical_bytes(uint8_t* pbtAts, size_t szAts, size_t* pszTk);

        //NFC_EXPORT void nfc_free(void* p);
        [DllImport(LIB, EntryPoint = "nfc_free", CallingConvention = CallingConvention.Cdecl)]
        public static extern void nfc_free(IntPtr ptr);
        [DllImport(LIB, EntryPoint = "nfc_version", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr version();
        [DllImport(LIB, EntryPoint = "nfc_device_get_information_about", CallingConvention = CallingConvention.Cdecl)]
        public static extern NfcError device_get_information_about(IntPtr device, ref string buff);
        #endregion

        #region String converter functions
        //NFC_EXPORT const char* str_nfc_modulation_type(const nfc_modulation_type nmt);
        //NFC_EXPORT const char* str_nfc_baud_rate(const nfc_baud_rate nbr);
        //NFC_EXPORT int str_nfc_target(char** buf, const nfc_target* pnt, bool verbose);
        [DllImport(LIB, EntryPoint = "str_nfc_target", CallingConvention = CallingConvention.Cdecl)]
        public static extern int str_nfc_target(out IntPtr buf, IntPtr device, bool verbose);
        #endregion
    }
}
