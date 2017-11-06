using System;
using System.Collections.Generic;
using System.Text;

namespace RaspberryPi.LibNFC.Interop
{
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Explicit, Size = SIZE, Pack = 1)]
    internal struct nfc_target_info
    {
        public const int SIZE = 275;

        /// nfc_iso14443a_info->Anonymous_61829c31_1c5f_4d3e_88ce_bfdd69cb86ec
        [System.Runtime.InteropServices.FieldOffsetAttribute(0)]
        public nfc_iso14443a_info nai;

        /// nfc_felica_info->Anonymous_64482df8_a345_4722_a226_daee0c512afb
        [System.Runtime.InteropServices.FieldOffsetAttribute(0)]
        public nfc_felica_info nfi;

        /// nfc_iso14443b_info->Anonymous_3e0b2b68_8495_4475_ab3a_71e0cb330c6c
        [System.Runtime.InteropServices.FieldOffsetAttribute(0)]
        public nfc_iso14443b_info nbi;

        /// nfc_iso14443bi_info->Anonymous_052453cd_f15b_40ca_b626_cb4902b698b3
        [System.Runtime.InteropServices.FieldOffsetAttribute(0)]
        public nfc_iso14443bi_info nii;

        /// nfc_iso14443b2sr_info->Anonymous_aea571a5_db10_47ff_95fd_389241410ef3
        [System.Runtime.InteropServices.FieldOffsetAttribute(0)]
        public nfc_iso14443b2sr_info nsi;

        /// nfc_iso14443b2ct_info->Anonymous_eff360d7_71ef_4cba_94fd_01b24fea1004
        [System.Runtime.InteropServices.FieldOffsetAttribute(0)]
        public nfc_iso14443b2ct_info nci;

        /// nfc_jewel_info->Anonymous_4731d7f2_a7cd_4f8c_83a2_a3180bbd8012
        [System.Runtime.InteropServices.FieldOffsetAttribute(0)]
        public nfc_jewel_info nji;

        /// nfc_barcode_info->Anonymous_7e1c0d4c_ebe9_45d7_aec2_49f5dff13a8f
        [System.Runtime.InteropServices.FieldOffsetAttribute(0)]
        public nfc_barcode_info nti;

        /// nfc_dep_info->Anonymous_7301ade3_0c3e_4bd2_a6fe_fe769f9e38d2
        [System.Runtime.InteropServices.FieldOffsetAttribute(0)]
        public nfc_dep_info ndi;
    }
}
