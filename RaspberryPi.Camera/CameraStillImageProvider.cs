using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RaspberryPi.Camera
{
    public static class CameraStillImageProvider
    {
        public static Task<byte[]> CaptureImage(this ICameraStillImageProvider provider)
        {
            return provider.CaptureImage(new CaptureImageConfig());
        }
    }
}
