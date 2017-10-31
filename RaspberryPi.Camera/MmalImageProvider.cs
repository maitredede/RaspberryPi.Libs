using Microsoft.Extensions.Logging;
using RaspberryPi.Userland;
using RaspberryPi.Userland.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RaspberryPi.Camera
{
    public class MmalImageProvider : ICameraStillImageProvider
    {
        private readonly IBcmHost m_bcm;
        private readonly ILogger<ICameraStillImageProvider> m_logger;

        public MmalImageProvider(IBcmHost host, ILogger<ICameraStillImageProvider> logger)
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
