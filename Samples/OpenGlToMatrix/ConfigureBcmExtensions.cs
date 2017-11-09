using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenGlToMatrix.SimpleGLES;
using RaspberryPi.LibLedMatrix;
using RaspberryPi.Userland;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace OpenGlToMatrix
{
    internal static class ConfigureBcmExtensions
    {
        public static void AddBcm(this IServiceCollection services)
        {
            services.AddSingleton<BcmHost>(svc =>
            {
                svc.GetRequiredService<ILogger<Program>>().LogDebug("Initializing BcmHost");
                return new BcmHost();
            });
            services.AddSingleton<Resource>(svc =>
            {
                BcmHost host = svc.GetRequiredService<BcmHost>();
                LedMatrix matrix = svc.GetRequiredService<LedMatrix>();

                svc.GetRequiredService<ILogger<Program>>().LogDebug("Initializing Resource");
                return host.Dispman.CreateResource(VC_IMAGE_TYPE_T.VC_IMAGE_RGB888, matrix.CanvasWidth, matrix.CanvasHeight);
            });
            services.AddSingleton<Display>(svc =>
            {
                BcmHost host = svc.GetRequiredService<BcmHost>();
                //Resource target = svc.GetRequiredService<Resource>();

                svc.GetRequiredService<ILogger<Program>>().LogDebug("Initializing Display");
                //return host.Dispman.DisplayOpenOffscreen(target, DISPMANX_TRANSFORM_T.DISPMANX_NO_ROTATE);
                return host.Dispman.DisplayOpen(Screen.MAIN_LCD);
            });
            services.AddSingleton<Element>(svc =>
            {
                BcmHost host = svc.GetRequiredService<BcmHost>();
                Display display = svc.GetRequiredService<Display>();
                LedMatrix matrix = svc.GetRequiredService<LedMatrix>();
                //Resource resource = svc.GetRequiredService<Resource>();

                Rectangle srcRect = new Rectangle(0, 0, matrix.CanvasWidth, matrix.CanvasHeight);
                Rectangle dstRect = new Rectangle(0, 0, matrix.CanvasWidth << 16, matrix.CanvasHeight << 16);

                Update update = host.Dispman.UpdateStart(0);
                svc.GetRequiredService<ILogger<Program>>().LogDebug("Initializing Element");
                Element element = host.Dispman.ElementAdd(update, display, 0, dstRect, null, srcRect, Protection.None, null, null, DISPMANX_TRANSFORM_T.DISPMANX_NO_ROTATE);
                update.SubmitSync();
                return element;
            });
            services.AddSingleton<INativeWindow>(svc =>
            {
                LedMatrix matrix = svc.GetRequiredService<LedMatrix>();
                Element element = svc.GetRequiredService<Element>();

                svc.GetRequiredService<ILogger<Program>>().LogDebug("Initializing DispmanWindow");
                return new DispmanWindow(element, matrix.CanvasWidth, matrix.CanvasHeight);
            });
        }
    }
}
