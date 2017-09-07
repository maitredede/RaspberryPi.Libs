using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RaspberryPi.Camera
{
    public class MmalImageProvider : ICameraStillImageProvider
    {
        private readonly BcmHost m_bcm;
        private readonly ILogger<ICameraStillImageProvider> m_logger;

        public MmalImageProvider(BcmHost host, ILogger<ICameraStillImageProvider> logger)
        {
            this.m_bcm = host;
            this.m_logger = logger;
        }

        public Task<byte[]> CaptureImage(CaptureImageConfig parameters)
        {
            throw new NotImplementedException();
        }
    }
}
