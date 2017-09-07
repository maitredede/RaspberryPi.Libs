using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using MMAL_FOURCC_T = System.UInt32;

namespace RaspberryPi.MMAL.Interop
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MMAL_ES_FORMAT_T
    {
        [MarshalAs(UnmanagedType.I4)]
        public MMAL_ES_TYPE_T type;           /**< Type of the elementary stream */

        [MarshalAs(UnmanagedType.U4)]
        public MMAL_FOURCC_T encoding;        /**< FourCC specifying the encoding of the elementary stream.
                                    * See the \ref MmalEncodings "pre-defined encodings" for some
                                    * examples.
                                    */
        [MarshalAs(UnmanagedType.U4)]
        public MMAL_FOURCC_T encoding_variant;/**< FourCC specifying the specific encoding variant of
                                    * the elementary stream. See the \ref MmalEncodingVariants
                                    * "pre-defined encoding variants" for some examples.
                                    */

        //MMAL_ES_SPECIFIC_FORMAT_T* es; /**< Type specific information for the elementary stream */
        public IntPtr es; /**< Type specific information for the elementary stream */

        [MarshalAs(UnmanagedType.U4)]
        public uint bitrate;              /**< Bitrate in bits per second */
        [MarshalAs(UnmanagedType.U4)]
        public uint flags;                /**< Flags describing properties of the elementary stream.
                                    * See \ref elementarystreamflags "Elementary stream flags".
                                    */

        [MarshalAs(UnmanagedType.U4)]
        public uint extradata_size;       /**< Size of the codec specific data */
        public IntPtr extradata;           /**< Codec specific data */
    }
}
