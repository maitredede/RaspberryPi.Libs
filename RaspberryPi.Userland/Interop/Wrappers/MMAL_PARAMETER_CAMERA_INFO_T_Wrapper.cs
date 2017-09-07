using RaspberryPi.Userland.Interop;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace RaspberryPi.Userland.Interop.Wrappers
{
    internal sealed class MMAL_PARAMETER_CAMERA_INFO_T_Wrapper : BaseParameter<MMAL_PARAMETER_CAMERA_INFO_T>
    {
        public uint num_cameras { get { return this.Handler.ReadUInt32(); } }
        public uint num_flashes { get { return this.Handler.ReadUInt32(); } }

        private ByvalArrayWrapper<MMAL_PARAMETER_CAMERA_INFO_CAMERA_T_Wrapper> m_cameras;
        public ByvalArrayWrapper<MMAL_PARAMETER_CAMERA_INFO_CAMERA_T_Wrapper> cameras
        {
            get
            {
                if (this.m_cameras == null)
                {
                    this.m_cameras = new ByvalArrayWrapper<MMAL_PARAMETER_CAMERA_INFO_CAMERA_T_Wrapper>(
                        () => this.Ptr + this.Handler.GetOffset(nameof(this.cameras)),
                        () => (int)this.num_cameras,
                        MMAL_PARAMETER_CAMERA_INFO_CAMERA_T_Wrapper.Size,
                        p => new MMAL_PARAMETER_CAMERA_INFO_CAMERA_T_Wrapper(p));
                }
                return this.m_cameras;
            }
        }
    }
}
