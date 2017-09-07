using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace RaspberryPi.Camera
{
    public static class CameraDependencyInjectionExtensions
    {
        public static IServiceCollection AddRaspiStillCameraImageProvider(this IServiceCollection services)
        {
            services.AddSingleton<ICameraStillImageProvider, RaspiStillImageProvider>();
            return services;
        }

        public static IServiceCollection AddMmalCameraImageProvicer(this IServiceCollection services)
        {
            services.TryAddSingleton<BcmHost>();
            services.AddSingleton<ICameraStillImageProvider, MmalImageProvider>();
            return services;
        }
    }
}
