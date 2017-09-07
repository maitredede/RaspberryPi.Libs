using RaspberryPi.Interop;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace RaspberryPi.MMAL.Interop.Wrappers
{
    internal sealed class MMAL_PARAMETER_CAMERA_INFO_CAMERA_T_Wrapper
    {
        private readonly InteropHandler<MMAL_PARAMETER_CAMERA_INFO_CAMERA_T> m_handler;

        public static readonly int Size = Marshal.SizeOf<MMAL_PARAMETER_CAMERA_INFO_CAMERA_T>();

        public MMAL_PARAMETER_CAMERA_INFO_CAMERA_T_Wrapper(IntPtr ptr)
        {
            this.m_handler = new InteropHandler<MMAL_PARAMETER_CAMERA_INFO_CAMERA_T>(() => ptr);
        }

        public uint port_id
        {
            get { return this.m_handler.ReadUInt32(); }
        }
        public int max_width { get { return (int)this.m_handler.ReadUInt32(); } }
        public int max_height { get { return (int)this.m_handler.ReadUInt32(); } }
        public bool lens_present { get { return this.m_handler.ReadInt32() != 0; } }
        public string camera_name { get { return this.m_handler.ReadString(); } }
    }
}
