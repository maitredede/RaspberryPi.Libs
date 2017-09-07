using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RaspberryPi.Camera
{
    public interface ICameraStillImageProvider
    {
        Task<byte[]> CaptureImage(CaptureImageConfig parameters);
    }
}
