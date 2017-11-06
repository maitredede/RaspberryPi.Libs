using System;
using RaspberryPi.LibLedMatrix;
using SixLabors.ImageSharp;
using System.Diagnostics;
using SixLabors.Primitives;
using System.Threading;
using System.IO;
using Microsoft.Extensions.CommandLineUtils;

namespace LedMatrixPoc
{
    public static class Program
    {
        private static CommandLineApplication app;
        private static CommandArgument image;
        private static CommandOption brightness;

        public static int Main(string[] args)
        {
            app = new CommandLineApplication();
            image = app.Argument("image", "Image to display");
            brightness = app.Option("--brightness", "Brightness % (1-100)", CommandOptionType.SingleValue);

            app.HelpOption("-h|--help");
            app.OnExecute(new Func<int>(RunApp));
            return app.Execute(args);
        }

        private static int RunApp()
        {
            Console.WriteLine("Hello World!");

            if (String.IsNullOrEmpty(image.Value))
            {
                app.ShowHelp();
                return -1;
            }
            if (!File.Exists(image.Value))
            {
                app.Error.WriteLine("File not found");
                return -1;
            }
            int brightnessValue = 0;
            if (brightness.HasValue())
            {
                if (int.TryParse(brightness.Value(), out brightnessValue))
                {
                    if (brightnessValue < 1 || brightnessValue > 100)
                    {
                        app.Error.WriteLine("Brightness value outside allowed values");
                        return -1;
                    }
                }
                else
                {
                    app.Error.WriteLine("Invalid brightness value");
                    return -1;
                }
            }

            Stopwatch swatch = Stopwatch.StartNew();
            using (LedMatrixOptions options = new LedMatrixOptions()
            {
                chain_length = 2,
                rows = 64,
                led_rgb_sequence = "RBG",
                brightness = brightnessValue,
            })
            using (Image<Rgba32> gif = Image.Load(image.Value))
            //using (LedMatrix matrix = new LedMatrix(rows: 64, chained: 2, parallel: 1))
            using (LedMatrix matrix = new LedMatrix(options))
            {
                Size size = new Size(matrix.CanvasWidth, matrix.CanvasHeight);
                int framesCount = gif.Frames.Count;
                Console.WriteLine("Canvas size: {0}x{1}", size.Width, size.Height);

                gif.Mutate(ops =>
                {
                    ops.Resize(size);
                });
                //string path = Path.Combine(Environment.CurrentDirectory, "block64.gif");
                //Console.WriteLine("Saving path={0}", path);
                //using (var output = File.Create(path))
                //{
                //    gif.SaveAsGif(output);
                //}
                bool run = true;
                Console.CancelKeyPress += (s, e) =>
                {
                    if (run)
                    {
                        run = false;
                        e.Cancel = true;
                    }
                };
                while (run)
                {
                    for (int i = 0; i < framesCount; i++)
                    {
                        long cur = swatch.ElapsedMilliseconds;
                        var frame = gif.Frames[i];
                        matrix.UpdateCanvas(canvas =>
                        {
                            for (int x = 0; x < size.Width; x++)
                            {
                                for (int y = 0; y < size.Height; y++)
                                {
                                    Rgba32 pix = frame[x, y];
                                    canvas.SetPixel(x, y, pix.R, pix.G, pix.B);
                                }
                            }
                            Console.Write(".");
                        });
                        //matrix.UpdateCanvasAsImageSharp(canvasImage =>
                        //{
                        //    canvasImage.DrawImage(frames[i].Image, 1, size, Point.Empty);
                        //});
                        long elapsed = swatch.ElapsedMilliseconds - cur;
                        long wait = frame.MetaData.FrameDelay * 10 - elapsed;
                        if (wait > 0)
                        {
                            Thread.Sleep((int)wait);
                        }
                    }
                }
            }
            return 0;
        }

        //private static Frame[] LoadFrames(Size size)
        //{
        //    using (Image<Rgba32> gif = Image.Load("block.gif"))
        //    {

        //        gif.Mutate(ops =>
        //        {
        //            ops.Resize(size);
        //        });
        //        string path = Path.Combine(Environment.CurrentDirectory, "block64.gif");
        //        Console.WriteLine("Saving path={0}", path);
        //        using (var output = File.Create(path))
        //        {
        //            gif.SaveAsGif(output);
        //        }

        //        int framesCount = gif.Frames.Count;
        //        Frame[] frames = new Frame[framesCount];
        //        for (int i = 0; i < framesCount; i++)
        //        {
        //            ImageFrame<Rgba32> frame = gif.Frames[i];
        //            frames[i].Image = Image.LoadPixelData<Rgba32>(frame.SavePixelData(), size.Width, size.Height);
        //            // frames[i].Image = gif.Frames.CloneFrame(i);
        //            frames[i].Delay = frame.MetaData.FrameDelay;
        //        }
        //        return frames;
        //    }
        //}
    }
}
