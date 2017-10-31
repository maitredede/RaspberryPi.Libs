using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace RaspberryPi.Camera
{
    public class FakeStillImageProvider : ICameraStillImageProvider
    {
        public async Task<byte[]> CaptureImage(CaptureImageConfig parameters)
        {
            using (var resStream = typeof(FakeStillImageProvider).Assembly.GetManifestResourceStream("RaspberryPi.Camera.Resources.missing_image.jpg"))
            using (MemoryStream ms = new MemoryStream())
            {
                await resStream.CopyToAsync(ms);
                return ms.ToArray();
            }
        }
    }
}
