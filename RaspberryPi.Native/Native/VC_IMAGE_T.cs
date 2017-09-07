using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace RaspberryPi.Native
{
    [StructLayout(LayoutKind.Sequential, Size = 64)]
    internal struct VC_IMAGE_T
    {
        VC_IMAGE_TYPE_T type;
        VC_IMAGE_INFO_T info;           /* format-specific info; zero for VC02 behaviour */
        ushort width;          /* width in pixels */
        ushort height;         /* height in pixels */
        int pitch;          /* pitch of image_data array in bytes */
        int size;           /* number of bytes available in image_data array */
        IntPtr image_data;     /* pixel data */
        IntPtr extra;          /* extra data like palette pointer */
        IntPtr metadata;       /* metadata header for the image */
        IntPtr pool_object;    /* nonNULL if image was allocated from a vc_pool */
        uint mem_handle;     /* the mem handle for relocatable memory storage */
        int metadata_size;  /* size of metadata of each channel in bytes */
        int channel_offset; /* offset of consecutive channels in bytes */
        uint video_timestamp;/* 90000 Hz RTP times domain - derived from audio timestamp */
        byte num_channels;   /* number of channels (2 for stereo) */
        byte current_channel;/* the channel this header is currently pointing to */
        byte linked_multichann_flag;/* Indicate the header has the linked-multichannel structure*/
        byte is_channel_linked;     /* Track if the above structure is been used to link the header
                                                                into a linked-mulitchannel image */
        byte channel_index;         /* index of the channel this header represents while
                                                                it is being linked. */
        //byte _dummy[3];      /* pad struct to 64 bytes */
    }
}
