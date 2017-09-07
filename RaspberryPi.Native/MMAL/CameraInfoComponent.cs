using System;
using System.Collections.Generic;
using System.Text;
using RaspberryPi.MMAL.Interop;
using System.Runtime.InteropServices;
using RaspberryPi.MMAL.Interop.Wrappers;

namespace RaspberryPi.MMAL
{
    public sealed class CameraInfoComponent : MMALComponent
    {
        private readonly Dictionary<int, CameraInfo> m_infoCache;

        internal CameraInfoComponent(/*MMAL_COMPONENT_T*/IntPtr handle, MMALComponentName name) : base(handle, name)
        {
            this.m_infoCache = new Dictionary<int, CameraInfo>();
        }

        public CameraInfo GetCameraInfo(int cameraNum)
        {
            if (this.m_infoCache.ContainsKey(cameraNum))
                return this.m_infoCache[cameraNum];

            CameraInfo info = new CameraInfo();
            using (MMAL_PARAMETER_CAMERA_INFO_T_Wrapper param = new MMAL_PARAMETER_CAMERA_INFO_T_Wrapper())
            {
                //param.hdr.DumpOffsets();

                //MMAL_PARAMETER_CAMERA_INFO_T param = new MMAL_PARAMETER_CAMERA_INFO_T();
                //param.hdr = new MMAL_PARAMETER_HEADER_T();
                param.hdr.id = MMALParameterId.MMAL_PARAMETER_CAMERA_INFO;
                uint size = param.hdr.size;
                param.hdr.size = size - 4u;  // Deliberately undersize to check firmware veresion
                //param.hdr.size = (uint)(MMAL_PARAMETER_CAMERA_INFO_T.Size - 4);

                MMAL_STATUS_T status = NativeMethods.PortParameterGet(this.HandleWrapper.control, param);
                if (status != MMAL_STATUS_T.MMAL_SUCCESS)
                {
                    // Running on newer firmware
                    param.hdr.size = size;
                    //param.hdr.size = (uint)(MMAL_PARAMETER_CAMERA_INFO_T.Size);
                    status = NativeMethods.PortParameterGet(this.HandleWrapper.control, param);
                    if (status == MMAL_STATUS_T.MMAL_SUCCESS && param.num_cameras > cameraNum)
                    {
                        info.Width = param.cameras[cameraNum].max_width;
                        info.Height = param.cameras[cameraNum].max_height;
                        info.Name = param.cameras[cameraNum].camera_name;

                        //info.DumpProperties();
                    }
                    else
                    {
                        Console.WriteLine($"Cannot read camera info, keeping the defaults for OV5647. status={status} param.num_cameras={param.num_cameras}");
                        //vcos_log_error("Cannot read camera info, keeping the defaults for OV5647");
                    }
                }
                else
                {
                    // Older firmware
                    // Nothing to do here, keep the defaults for OV5647
                    Console.WriteLine("Older firmware, keeping the defaults for OV5647");
                }
            }


            //Assume defaults
            if (string.IsNullOrEmpty(info.Name))
                info.Name = "OV5647";
            if (info.Width == 0)
                info.Width = 2592;
            if (info.Height == 0)
                info.Height = 1944;

            this.m_infoCache.Add(cameraNum, info);

            return info;
        }
    }
}
