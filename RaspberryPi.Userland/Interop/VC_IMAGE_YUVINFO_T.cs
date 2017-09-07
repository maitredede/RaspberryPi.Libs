using System;
using System.Collections.Generic;
using System.Text;

namespace RaspberryPi.Userland.Interop
{
    internal enum VC_IMAGE_YUVINFO_T:ushort
    {
        VC_IMAGE_YUVINFO_UNSPECIFIED = 0,   /* Unknown or unset - defaults to BT601 interstitial */

        /* colour-space conversions data [4 bits] */
        /* Note that colour conversions for SMPTE 170M are identical to BT.601 */
        VC_IMAGE_YUVINFO_CSC_ITUR_BT601 = 1,   /* ITU-R BT.601-5 [SDTV] (compatible with VideoCore-II) */
        VC_IMAGE_YUVINFO_CSC_ITUR_BT709 = 2,   /* ITU-R BT.709-3 [HDTV] */
        VC_IMAGE_YUVINFO_CSC_JPEG_JFIF = 3,   /* JPEG JFIF */
        VC_IMAGE_YUVINFO_CSC_FCC = 4,   /* Title 47 Code of Federal Regulations (2003) 73.682 (a) (20) */
        VC_IMAGE_YUVINFO_CSC_SMPTE_240M = 5,   /* Society of Motion Picture and Television Engineers 240M (1999) */
        VC_IMAGE_YUVINFO_CSC_ITUR_BT470_2_M = 6,  /* ITU-R BT.470-2 System M */
        VC_IMAGE_YUVINFO_CSC_ITUR_BT470_2_BG = 7,  /* ITU-R BT.470-2 System B,G */
        VC_IMAGE_YUVINFO_CSC_JPEG_JFIF_Y16_255 = 8, /* JPEG JFIF, but with 16..255 luma */
        VC_IMAGE_YUVINFO_CSC_CUSTOM = 15,  /* Custom colour matrix follows header */
        VC_IMAGE_YUVINFO_CSC_SMPTE_170M = VC_IMAGE_YUVINFO_CSC_ITUR_BT601,

        /* co-sited flags, assumed interstitial if not co-sited [2 bits] */
        VC_IMAGE_YUVINFO_H_COSITED = 256,
        VC_IMAGE_YUVINFO_V_COSITED = 512,

        VC_IMAGE_YUVINFO_TOP_BOTTOM = 1024,
        VC_IMAGE_YUVINFO_DECIMATED = 2048,
        VC_IMAGE_YUVINFO_PACKED = 4096,

        /* Certain YUV image formats can either be V/U interleaved or U/V interleaved */
        VC_IMAGE_YUVINFO_IS_VU = 0x8000,

        /* Force Metaware to use 16 bits */
        VC_IMAGE_YUVINFO_FORCE_ENUM_16BIT = 0xffff,
    }
}
