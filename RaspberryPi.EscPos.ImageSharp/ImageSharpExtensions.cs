using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace RaspberryPi.EscPos
{
    public static class ImageSharpExtensions
    {
        /// <summary>
        /// Prints an image on the printer
        /// </summary>
        /// <param name="printer"></param>
        /// <param name="image">Image to print</param>
        public static void PrintImage(this EscPosPrinter printer, Image<Rgba32> image)
        {
            if (printer == null)
                throw new ArgumentNullException(nameof(printer));
            if (image == null)
                throw new ArgumentNullException(nameof(image));

            throw new NotImplementedException();
        }
    }
}
