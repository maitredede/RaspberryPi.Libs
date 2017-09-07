using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace RaspberryPi.MMAL.Interop
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MMAL_COMPONENT_T
    {
        /** Pointer to the private data of the module in use */
        //struct MMAL_COMPONENT_PRIVATE_T *priv;
        public IntPtr priv;

        /** Pointer to private data of the client */
        //struct MMAL_COMPONENT_USERDATA_T *userdata;
        public IntPtr userdata;

        /** Component name */
        //const char* name;
        [MarshalAs(UnmanagedType.LPStr)]
        public string name;

        /** Specifies whether the component is enabled or not */
        [MarshalAs(UnmanagedType.U4)]
        public uint is_enabled;

        /** All components expose a control port.
         * The control port is used by clients to set / get parameters that are global to the
         * component. It is also used to receive events, which again are global to the component.
         * To be able to receive events, the client needs to enable and register a callback on the
         * control port. */
        //[MarshalAs(UnmanagedType.LPStruct)]
        //public MMAL_PORT_T control;
        public IntPtr control;

        [MarshalAs(UnmanagedType.U4)]
        public uint input_num;   /**< Number of input ports */
        //public MMAL_PORT_T[] input;     /**< Array of input ports */
        public IntPtr input;

        [MarshalAs(UnmanagedType.U4)]
        public uint output_num;  /**< Number of output ports */
        //public MMAL_PORT_T[] output;    /**< Array of output ports */
        public IntPtr output;

        [MarshalAs(UnmanagedType.U4)]
        public uint clock_num;   /**< Number of clock ports */
        //public MMAL_PORT_T[] clock;     /**< Array of clock ports */
        public IntPtr clock;

        [MarshalAs(UnmanagedType.U4)]
        public uint port_num;    /**< Total number of ports */
        //public MMAL_PORT_T[] port;      /**< Array of all the ports (control/input/output/clock) */
        public IntPtr port;

        ///** Uniquely identifies the component's instance within the MMAL
        // * context / process. For debugging. */
        [MarshalAs(UnmanagedType.U4)]
        public uint id;
    }
}
