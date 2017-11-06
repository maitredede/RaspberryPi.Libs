using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Helpers;
using SixLabors.ImageSharp.Formats;
using System;
using System.Collections.Generic;
using System.Text;

namespace RaspberryPi.LibLedMatrix
{
    public static class LedCanvasExtensions
    {
        public static void UpdateCanvasAsImageSharp(this LedMatrix matrix, Action<IImageProcessingContext<Rgba32>> method)
        {
            matrix.UpdateCanvas(canvas =>
            {
                using (Image<Rgba32> img = new Image<Rgba32>(canvas.Width, canvas.Height))
                {
                    img.Mutate(method);

                    byte[] pixelData = img.SavePixelData();
                    for (int x = 0; x < canvas.Width; x++)
                    {
                        for (int y = 0; y < canvas.Height; y++)
                        {
                            int baseAddr = (x * canvas.Width + y) * 4;
                            byte r = pixelData[baseAddr + 0];
                            byte g = pixelData[baseAddr + 1];
                            byte b = pixelData[baseAddr + 2];
                            canvas.SetPixel(x, y, r, g, b);
                        }
                    }
                }
            });
        }
    }
}
