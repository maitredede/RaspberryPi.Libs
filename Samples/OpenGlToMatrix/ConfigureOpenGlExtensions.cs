using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RaspberryPi.Userland;
using RaspberryPi.Userland.SimpleGL;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace OpenGlToMatrix
{
    internal static class ConfigureOpenGlExtensions
    {
        public static void AddOpenGl(this IServiceCollection services)
        {
            services.AddSingleton<EglDisplay>(svc =>
            {
                //svc.GetRequiredService<BcmHost>();

                svc.GetRequiredService<ILogger<Program>>().LogDebug("Initializing EglDisplay");
                return new EglDisplay();
            });
            services.AddSingleton<EglContext>(svc =>
            {
                EglDisplay display = svc.GetRequiredService<EglDisplay>();

                svc.GetRequiredService<ILogger<Program>>().LogDebug("Initializing EglContext");
                return new EglContext(display);
            });
            services.AddSingleton<EglSurface>(svc =>
            {
                EglDisplay display = svc.GetRequiredService<EglDisplay>();
                INativeWindow nativeWindow = svc.GetRequiredService<INativeWindow>();

                svc.GetRequiredService<ILogger<Program>>().LogDebug("Initializing EglSurface");
                return new EglSurface(display, nativeWindow);
            });
            services.AddSingleton<Data>(svc =>
            {
                svc.GetRequiredService<ILogger<Program>>().LogDebug("Initializing Data");
                return new Data();
            });
        }
    }
}
