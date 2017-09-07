using RaspberryPi.MMAL.Interop;
using RaspberryPi.MMAL.Interop.Wrappers;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace RaspberryPi.MMAL
{
    public sealed class CameraComponent : MMALComponent
    {
        public static readonly int PREVIEW_PORT = 0;
        public static readonly int VIDEO_PORT = 1;
        public static readonly int CAPTURE_PORT = 2;

        internal CameraComponent(/*MMAL_COMPONENT_T*/IntPtr handle, MMALComponentName name) : base(handle, name)
        {
        }

        public void SetStereoscopicMode(StereoScopicMode mode)
        {
            this.SetStereoscopicMode(0, mode);
            this.SetStereoscopicMode(1, mode);
            this.SetStereoscopicMode(2, mode);
        }

        public void SetStereoscopicMode(int channel, StereoScopicMode mode)
        {
            using (MMAL_PARAMETER_STEREOSCOPIC_MODE_T_Wrapper stereo = new MMAL_PARAMETER_STEREOSCOPIC_MODE_T_Wrapper())
            {
                stereo.hdr.id = MMALParameterId.MMAL_PARAMETER_STEREOSCOPIC_MODE;
                stereo.mode = mode.Mode;
                if (mode.Mode != MMAL_STEREOSCOPIC_MODE_T.MMAL_STEREOSCOPIC_MODE_NONE)
                {
                    stereo.decimate = mode.Decimate;
                    stereo.swap_eyes = mode.SwapEyes;
                }

                MMAL_STATUS_T status = NativeMethods.PortParameterSet(this.HandleWrapper.output[channel], stereo);
                //MMAL_STATUS_T status = NativeMethods.PortParameterSet(this.HandleWrapper.GetOutput()[channel], stereo);
                if (status != MMAL_STATUS_T.MMAL_SUCCESS)
                {
                    throw new MMALException(status);
                }
            }
        }

        public void SetCameraNum(int camNumber)
        {
            using (MMAL_PARAMETER_INT32_T_Wrapper param = new MMAL_PARAMETER_INT32_T_Wrapper())
            {
                param.hdr.id = MMALParameterId.MMAL_PARAMETER_CAMERA_NUM;
                param.value = camNumber;
                MMAL_STATUS_T status = NativeMethods.PortParameterSet(this.HandleWrapper.control, param);
                if (status != MMAL_STATUS_T.MMAL_SUCCESS)
                {
                    throw new MMALException(status);
                }
            }
        }

        public int OutputCount { get { return this.HandleWrapper.output_num; } }

        /// <summary>
        /// Set camera sensor mode. See forum/doc https://www.raspberrypi.org/documentation/raspbian/applications/camera.md
        /// </summary>
        /// <param name="mode">0 = auto</param>
        public void SetSensorMode(int mode)
        {
            MMAL_STATUS_T status = NativeMethods.PortParameterSet(this.HandleWrapper.control, MMALParameterId.MMAL_PARAMETER_CAMERA_CUSTOM_SENSOR_CONFIG, mode);
            if (status != MMAL_STATUS_T.MMAL_SUCCESS)
            {
                throw new MMALException(status);
            }
        }
    }
}
