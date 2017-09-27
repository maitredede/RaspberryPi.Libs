using SixLabors.ImageSharp.Formats;
using System;
using System.Reflection;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Text;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System.IO;
using RaspberryPi.PiGPIO.Drivers.Dede.PixelFormats;

namespace RaspberryPi.PiGPIO.Drivers.Dede
{
    [Obsolete("no usage", true)]
    internal sealed class DMDImageEncoder : IImageEncoder
    {
        private static readonly Action<Image<BitPixel>, Stream> EncodeImageAction;
        private readonly FreetronicsDMDSurface m_dmd;

        static DMDImageEncoder()
        {
            Type actionType = Expression.GetActionType(typeof(Image<BitPixel>), typeof(Stream));
            EncodeImageAction = (Action<Image<BitPixel>, Stream>)typeof(DMDImageEncoder).GetMethod(nameof(EncodeImage), BindingFlags.Public | BindingFlags.Static).CreateDelegate(actionType);
        }

        internal DMDImageEncoder(FreetronicsDMDSurface dmd)
        {
            this.m_dmd = dmd;
        }

        public void Encode<TPixel>(Image<TPixel> image, Stream stream) where TPixel : struct, IPixel<TPixel>
        {
            if (typeof(TPixel) == typeof(BitPixel))
            {
                EncodeImageAction.DynamicInvoke(image, stream);
            }
            throw new NotSupportedException();
        }

        public static void EncodeImage(Image<BitPixel> image, Stream stream)
        {
            ImageFrame<BitPixel> pixels = image.Frames.RootFrame;

            //const int scan = 4;
            //const int rowCount = 16 / scan;
            int quartHeight = image.Height / 4;
            int rowLength = image.Width * quartHeight / 8;
            for (int rowIndex = quartHeight - 1; rowIndex >= 0; rowIndex--)
            {

            }
        }
    }
}
