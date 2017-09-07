using RaspberryPi.MMAL.Interop.Wrappers;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace RaspberryPi.MMAL.Interop
{
    internal static class NativeMethods
    {
        public const string LIB = "libmmal";
        public const int MMAL_PARAMETER_CAMERA_INFO_MAX_CAMERAS = 4;
        public const int MMAL_PARAMETER_CAMERA_INFO_MAX_FLASHES = 2;
        public const int MMAL_PARAMETER_CAMERA_INFO_MAX_STR_LEN = 16;

        #region Groups
        /** Common parameter ID group, used with many types of component. */
        public const int MMAL_PARAMETER_GROUP_COMMON = (0 << 16);
        /** Camera-specific parameter ID group. */
        public const int MMAL_PARAMETER_GROUP_CAMERA = (1 << 16);
        /** Video-specific parameter ID group. */
        public const int MMAL_PARAMETER_GROUP_VIDEO = (2 << 16);
        /** Audio-specific parameter ID group. */
        public const int MMAL_PARAMETER_GROUP_AUDIO = (3 << 16);
        /** Clock-specific parameter ID group. */
        public const int MMAL_PARAMETER_GROUP_CLOCK = (4 << 16);
        /** Miracast-specific parameter ID group. */
        public const int MMAL_PARAMETER_GROUP_MIRACAST = (5 << 16);
        #endregion

        //[DllImport(LIB, EntryPoint = "mmal_component_create", CallingConvention = CallingConvention.Cdecl)]
        //public static extern MMAL_STATUS_T ComponentCreate(string name, out MMAL_COMPONENT_T component);
        [DllImport(LIB, EntryPoint = "mmal_component_create", CallingConvention = CallingConvention.Cdecl)]
        public static extern MMAL_STATUS_T ComponentCreate(string name, out IntPtr component);

        //[DllImport(LIB, EntryPoint = "mmal_component_destroy", CallingConvention = CallingConvention.Cdecl)]
        //public static extern MMAL_STATUS_T ComponentDestroy(MMAL_COMPONENT_T component);
        [DllImport(LIB, EntryPoint = "mmal_component_destroy", CallingConvention = CallingConvention.Cdecl)]
        public static extern MMAL_STATUS_T ComponentDestroy(IntPtr component);

        //[DllImport(LIB, EntryPoint = "mmal_component_acquire", CallingConvention = CallingConvention.Cdecl)]
        //public static extern void ComponentAcquire(MMAL_COMPONENT_T component);
        [DllImport(LIB, EntryPoint = "mmal_component_acquire", CallingConvention = CallingConvention.Cdecl)]
        public static extern void ComponentAcquire(IntPtr component);

        //[DllImport(LIB, EntryPoint = "mmal_component_release", CallingConvention = CallingConvention.Cdecl)]
        //public static extern MMAL_STATUS_T ComponentRelease(MMAL_COMPONENT_T component);
        [DllImport(LIB, EntryPoint = "mmal_component_release", CallingConvention = CallingConvention.Cdecl)]
        public static extern MMAL_STATUS_T ComponentRelease(IntPtr component);

        //[DllImport(LIB, EntryPoint = "mmal_component_enable", CallingConvention = CallingConvention.Cdecl)]
        //public static extern MMAL_STATUS_T ComponentEnable(MMAL_COMPONENT_T component);
        [DllImport(LIB, EntryPoint = "mmal_component_enable", CallingConvention = CallingConvention.Cdecl)]
        public static extern MMAL_STATUS_T ComponentEnable(IntPtr component);

        //[DllImport(LIB, EntryPoint = "mmal_component_disable", CallingConvention = CallingConvention.Cdecl)]
        //public static extern MMAL_STATUS_T ComponentDisable(MMAL_COMPONENT_T component);
        [DllImport(LIB, EntryPoint = "mmal_component_disable", CallingConvention = CallingConvention.Cdecl)]
        public static extern MMAL_STATUS_T ComponentDisable(IntPtr component);

        /// <summary>
        /// Get a parameter from a port.
        /// The size field must be set on input to the maximum size of the parameter
        /// (including the header) and will be set on output to the actual size of the
        /// parameter retrieved.
        /// \note If MMAL_ENOSPC is returned, the parameter is larger than the size
        /// given.The given parameter will have been filled up to its size and then
        /// the size field set to the full parameter's size. This can be used to
        /// resize the parameter buffer so that a second call should succeed.
        /// </summary>
        /// <param name="port">The port to which the request is sent</param>
        /// <param name="param">The pointer to the header of the parameter to get</param>
        /// <returns>MMAL_SUCCESS on success</returns>
        //[DllImport(LIB, EntryPoint = "mmal_port_parameter_get", CallingConvention = CallingConvention.Cdecl)]
        //public static extern MMAL_STATUS_T PortParameterGet(/*MMAL_PORT_T*/IntPtr port, /*ref MMAL_PARAMETER_HEADER_T*/IntPtr param);

        [DllImport(LIB, EntryPoint = "mmal_port_parameter_get", CallingConvention = CallingConvention.Cdecl)]
        public static extern MMAL_STATUS_T PortParameterGet(IntPtr port, IntPtr param);

        public static MMAL_STATUS_T PortParameterGet<T>(MMAL_PORT_T_Wrapper port, BaseParameter<T> param)
        {
            return PortParameterGet(port.Ptr(), param.Ptr);
        }

        [DllImport(LIB, EntryPoint = "mmal_port_parameter_set", CallingConvention = CallingConvention.Cdecl)]
        public static extern MMAL_STATUS_T PortParameterSet(IntPtr port, IntPtr param);

        public static MMAL_STATUS_T PortParameterSet<T>(MMAL_PORT_T_Wrapper port, BaseParameter<T> param)
        {
            return PortParameterSet(port.Ptr(), param.Ptr);
        }

        [DllImport(LIB, EntryPoint = "mmal_port_parameter_set_uint32", CallingConvention = CallingConvention.Cdecl)]
        public static extern MMAL_STATUS_T PortParameterSet(IntPtr port, MMALParameterId id, uint value);

        [DllImport(LIB, EntryPoint = "mmal_port_parameter_get_uint32", CallingConvention = CallingConvention.Cdecl)]
        public static extern MMAL_STATUS_T PortParameterGet(IntPtr port, MMALParameterId id, out uint value);

        [DllImport(LIB, EntryPoint = "mmal_port_parameter_set_int32", CallingConvention = CallingConvention.Cdecl)]
        public static extern MMAL_STATUS_T PortParameterSet(IntPtr port, MMALParameterId id, int value);
        [DllImport(LIB, EntryPoint = "mmal_port_parameter_get_int32", CallingConvention = CallingConvention.Cdecl)]
        public static extern MMAL_STATUS_T PortParameterGet(IntPtr port, MMALParameterId id, out int value);

        public static MMAL_STATUS_T PortParameterSet(MMAL_PORT_T_Wrapper port, MMALParameterId id, uint value)
        {
            return PortParameterSet(port.Ptr(), id, value);
        }

        public static MMAL_STATUS_T PortParameterSet(MMAL_PORT_T_Wrapper port, MMALParameterId id, int value)
        {
            return PortParameterSet(port.Ptr(), id, value);
        }
    }
}
