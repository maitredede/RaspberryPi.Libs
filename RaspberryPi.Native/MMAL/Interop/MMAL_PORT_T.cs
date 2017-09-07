using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace RaspberryPi.MMAL.Interop
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MMAL_PORT_T
    {
        public IntPtr priv; /**< Private member used by the framework */
        [MarshalAs(UnmanagedType.LPStr)]
        public string name;/**< Port name. Used for debugging purposes (Read Only) */

        [MarshalAs(UnmanagedType.I4)]
        public MMAL_PORT_TYPE_T type;            /**< Type of the port (Read Only) */
        [MarshalAs(UnmanagedType.U2)]
        public ushort index;                   /**< Index of the port in its type list (Read Only) */
        [MarshalAs(UnmanagedType.U2)]
        public ushort index_all;               /**< Index of the port in the list of all ports (Read Only) */

        [MarshalAs(UnmanagedType.U4)]
        public uint is_enabled;              /**< Indicates whether the port is enabled or not (Read Only) */
        //public MMAL_ES_FORMAT_T format;         /**< Format of the elementary stream */
        public IntPtr format;

        [MarshalAs(UnmanagedType.U4)]
        public uint buffer_num_min;          /**< Minimum number of buffers the port requires (Read Only).
                                          This is set by the component. */
        [MarshalAs(UnmanagedType.U4)]
        public uint buffer_size_min;         /**< Minimum size of buffers the port requires (Read Only).
                                          This is set by the component. */
        [MarshalAs(UnmanagedType.U4)]
        public uint buffer_alignment_min;    /**< Minimum alignment requirement for the buffers (Read Only).
                                          A value of zero means no special alignment requirements.
                                          This is set by the component. */
        [MarshalAs(UnmanagedType.U4)]
        public uint buffer_num_recommended;  /**< Number of buffers the port recommends for optimal performance (Read Only).
                                          A value of zero means no special recommendation.
                                          This is set by the component. */
        [MarshalAs(UnmanagedType.U4)]
        public uint buffer_size_recommended; /**< Size of buffers the port recommends for optimal performance (Read Only).
                                          A value of zero means no special recommendation.
                                          This is set by the component. */
        [MarshalAs(UnmanagedType.U4)]
        public uint buffer_num;              /**< Actual number of buffers the port will use.
                                          This is set by the client. */
        [MarshalAs(UnmanagedType.U4)]
        public uint buffer_size;             /**< Actual maximum size of the buffers that will be sent
                                          to the port. This is set by the client. */

        //struct MMAL_COMPONENT_T *component;    /**< Component this port belongs to (Read Only) */
        public IntPtr component;
        //struct MMAL_PORT_USERDATA_T *userdata; /**< Field reserved for use by the client */
        public IntPtr userdata;

        [MarshalAs(UnmanagedType.U4)]
        public uint capabilities;            /**< Flags describing the capabilities of a port (Read Only).
                                       * Bitwise combination of \ref portcapabilities "Port capabilities"
                                       * values.
                                       */
    }
}
