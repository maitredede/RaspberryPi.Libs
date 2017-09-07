using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace RaspberryPi.Userland.Interop
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MMAL_AUDIO_FORMAT_T
    {
        public uint channels;           /**< Number of audio channels */
        public uint sample_rate;        /**< Sample rate */

        public uint bits_per_sample;    /**< Bits per sample */
        public uint block_align;        /**< Size of a block of data */
    }
}
