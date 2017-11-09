using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RaspberryPi.LibLedMatrix;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace OpenGlToMatrix
{
    internal static class ConfigureLedMatrixExtensions
    {
        public static void AddLedMatrix(this IServiceCollection services)
        {
            services.AddSingleton<LedMatrixOptions>(svc =>
            {
                svc.GetRequiredService<ILogger<Program>>().LogDebug("Initializing LedMatrixOptions");
                return new LedMatrixOptions
                {
                    chain_length = 2,
                    rows = 64,
                    led_rgb_sequence = "RBG",
                };
            });
            services.AddSingleton<LedMatrix>(svc =>
            {
                LedMatrixOptions options = svc.GetRequiredService<LedMatrixOptions>();

                svc.GetRequiredService<ILogger<Program>>().LogDebug("Initializing LedMatrix");
                try
                {
                    return new LedMatrix(options);
                }
                catch (Exception ex)
                {
                svc.GetRequiredService<ILogger<Program>>().LogWarning(ex, "Error initializing LedMatrix");
                }

                svc.GetRequiredService<ILogger<Program>>().LogDebug("Initializing LedMatrix, without options");
                return new LedMatrix(rows: options.rows, chained: options.chain_length, parallel: options.parallel);
            });
        }
    }
}
