using Microsoft.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Mono.Unix;
using OpenGlToMatrix.SimpleGLES;
using RaspberryPi.LibLedMatrix;
using RaspberryPi.Userland;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

namespace OpenGlToMatrix
{
    public sealed class Program
    {
        private static CommandLineApplication app;
        private static CommandOption brightness;

        public static int Main(string[] args)
        {
            app = new CommandLineApplication();
            brightness = app.Option("--brightness", "Brightness % (1-100)", CommandOptionType.SingleValue);

            app.HelpOption("-h|--help");
            app.OnExecute(new Func<int>(RunApp));
            return app.Execute(args);
        }

        private static int RunApp()
        {
            app.Out.WriteLine("Hello World!");

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

            //long uid = UnixUserInfo.GetRealUserId();
            //if (uid != 0)
            //{
            //    app.Out.WriteLine("You must run this app as root.");
            //    return -1;
            //}

            //ServiceCollection serviceConfig = new ServiceCollection();
            //serviceConfig.AddLogging();
            //serviceConfig.AddOptions();

            //serviceConfig.AddBcm();
            //serviceConfig.AddOpenGl();
            //serviceConfig.AddLedMatrix();

            //using (ServiceProvider services = serviceConfig.BuildServiceProvider())
            //{
            //    services.GetRequiredService<ILoggerFactory>().AddConsole(LogLevel.Information);

            //    return RunApp(services);
            //}

            using (LedMatrixOptions options = new LedMatrixOptions()
            {
                chain_length = 2,
                parallel = 1,
                rows = 64,
                led_rgb_sequence = "RBG",
                pwm_bits = 8,
                show_refresh_rate = 1
            })
            using (LedMatrix matrix = new LedMatrix(options.rows, options.chain_length, options.parallel))
            using (BcmHost host = new BcmHost())
            using (Resource target = host.Dispman.CreateResource(VC_IMAGE_TYPE_T.VC_IMAGE_RGB888, matrix.CanvasWidth, matrix.CanvasHeight))
            using (Display display = host.Dispman.DisplayOpenOffscreen(target, DISPMANX_TRANSFORM_T.DISPMANX_NO_ROTATE))
            //using (Display display = host.Dispman.DisplayOpen(Screen.MAIN_LCD))
            using (ScopedElement element = ScopedElement.Create(host, display, null, destRect: display.ToRectangle(), srcRect: Rescale(display.ToRectangle())))
            using (DispmanWindow window = new DispmanWindow(element.E, display.Width, display.Height))
            using (EglDisplay eglDisp = new EglDisplay())
            using (EglContext ctx = new EglContext(eglDisp))
            using (EglSurface surface = new EglSurface(eglDisp, window))
            using (Data data = new Data())
            {
                Rectangle rect = display.ToRectangle();
                app.Out.WriteLine("Ready to go, screen is {0}x{1}", display.Width, display.Height);
                Stopwatch swatch = Stopwatch.StartNew();

                app.Out.WriteLine("Initializing 3D context");
                ctx.MakeCurrent(surface);

                app.Out.WriteLine("init_ogl");
                init_ogl();
                app.Out.WriteLine("init_model_proj");
                init_model_proj(rect, data);
                app.Out.WriteLine("init_textures");
                init_textures(data);

                app.Out.WriteLine("Initializing buffer");
                //int bufferLength = rect.Width * rect.Height * 3;
                //byte[] buffer = new byte[bufferLength];

                app.Out.WriteLine("Init complete, starting loop");
                //Render loop
                bool run = true;
                Console.CancelKeyPress += (s, e) =>
                {
                    if (run)
                    {
                        run = false;
                        e.Cancel = true;
                    }
                };

                int pitch = Utils.ALIGN_UP(3 * display.Width, 32);
                byte[] image = new byte[pitch * display.Height];
                app.Out.WriteLine("Buffer size: {0}", image.Length);
                while (run)
                {
                    update_model();
                    redraw_scene(surface);

                    //extract data to buffer
                    //target.WriteData(1, buffer, rect);
                    target.ReadData(rect, image, pitch);

                    //Display image to matrix
                    matrix.UpdateCanvas(canvas =>
                {
                    for (int x = 0; x < canvas.Width; x++)
                    {
                        for (int y = 0; y < canvas.Height; y++)
                        {
                            int baseAddr = (x * canvas.Width + y) * 3;
                            try
                            {
                                byte r = image[baseAddr + 0];
                                byte g = image[baseAddr + 1];
                                byte b = image[baseAddr + 2];
                                canvas.SetPixel(x, y, r, g, b);
                            }
                            catch (IndexOutOfRangeException ex)
                            {
                                app.Out.WriteLine("X={0} Y={1} baseAddre={2} pitch={3}", x, y, baseAddr, pitch);
                                System.Runtime.ExceptionServices.ExceptionDispatchInfo.Capture(ex).Throw();
                            }
                        }
                    }
                });
                    app.Out.Write(".");
                }

                return 0;
            }
        }

        private static Rectangle Rescale(Rectangle rectangle)
        {
            return new Rectangle(0, 0, rectangle.Width << 16, rectangle.Height << 16);
        }

        //private static int RunApp(IServiceProvider services)
        //{
        //    Stopwatch swatch = Stopwatch.StartNew();

        //    LedMatrix matrix = services.GetRequiredService<LedMatrix>();
        //    EglSurface surface = services.GetRequiredService<EglSurface>();
        //    EglContext ctx = services.GetRequiredService<EglContext>();
        //    Data data = services.GetRequiredService<Data>();

        //    app.Out.WriteLine("Initializing 3D context");
        //    ctx.MakeCurrent(surface);

        //    init_ogl();
        //    init_model_proj(matrix, data);
        //    init_textures(data);


        //    app.Out.WriteLine("Initializing buffer");
        //    int bufferLength = matrix.CanvasWidth * matrix.CanvasHeight * 3;
        //    byte[] buffer = new byte[bufferLength];
        //    var bufferRect = new Rectangle(0, 0, matrix.CanvasWidth, matrix.CanvasHeight);

        //    app.Out.WriteLine("Init complete, starting loop");
        //    //Resource target = services.GetRequiredService<Resource>();
        //    //Render loop
        //    bool run = true;
        //    Console.CancelKeyPress += (s, e) =>
        //    {
        //        if (run)
        //        {
        //            run = false;
        //            e.Cancel = true;
        //            app.Out.WriteLine();
        //            app.Out.WriteLine("Cancel required");
        //        }
        //    };

        //    while (run)
        //    {
        //        update_model();
        //        redraw_scene(surface);

        //        ////extract data to buffer
        //        //target.WriteData(1, buffer, bufferRect);

        //        ////Display image to matrix
        //        //matrix.UpdateCanvas(canvas =>
        //        //{
        //        //    for (int x = 0; x < canvas.Width; x++)
        //        //    {
        //        //        for (int y = 0; y < canvas.Height; y++)
        //        //        {
        //        //            int baseAddr = (x * canvas.Width + y) * 3;
        //        //            byte r = buffer[baseAddr + 0];
        //        //            byte g = buffer[baseAddr + 1];
        //        //            byte b = buffer[baseAddr + 2];
        //        //            canvas.SetPixel(x, y, r, g, b);
        //        //        }
        //        //    }
        //        //});
        //        app.Out.Write(".");
        //    }

        //    return 0;
        //}

        private static void init_ogl()
        {
            GL.ClearColor(0.15f, 0.25f, 0.35f, 1);
            GlDumpError();
            GL.Enable(EnableCap.CullFace);
            GlDumpError();
            GL.MatrixMode(MatrixMode.Modelview);
            GlDumpError();
        }

        private static void init_model_proj(Rectangle rect, Data data)
        {
            float nearp = 1.0f;
            float farp = 500.0f;
            float hht;
            float hwd;

            ///////////////////
            // init_model_proj
            GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);
            GlDumpError();

            GL.Viewport(rect.Left, rect.Top, rect.Width, rect.Height);
            GlDumpError();

            GL.MatrixMode(MatrixMode.Projection);
            GlDumpError();
            GL.LoadIdentity();
            GlDumpError();

            hht = nearp * (float)Math.Tan(45.0 / 2.0 / 180.0 * Math.PI);
            hwd = hht * (float)rect.Width / (float)rect.Height;

            GL.Frustumf(-hwd, hwd, -hht, hht, nearp, farp);
            GlDumpError();

            GL.EnableClientState(EnableCap.VertexArray);
            GlDumpError();
            GL.VertexPointer(3, VertexPointerType.Byte, 0, data.QuadX);
            GlDumpError();

            reset_model();
        }

        private static void reset_model()
        {
            // reset model position
            GL.MatrixMode(MatrixMode.Modelview);
            GlDumpError();
            GL.LoadIdentity();
            GlDumpError();
            GL.Translatef(0, 0, -50);
            GlDumpError();

            // reset model rotation
            rot[0] = 45;
            rot[1] = 30;
            rot[2] = 0;

            rateOfRotationPS[0] = 0.5f;
            rateOfRotationPS[1] = 0.5f;
            rateOfRotationPS[2] = 0;
            distance = 40;
        }

        private static float[] rateOfRotationPS = { 30, 45, 60 }; //degrees
        private static float[] rot = { 0, 0, 0 };
        private static float distance = 40;
        private static float distance_inc = 0;

        private static uint[] tex;
        public const int IMAGE_SIZE = 128;
        public const int GL_NEAREST = 0x2600;

        private static void GlDumpError()
        {
            ErrorCode err = GL.GetError();
            if (err != ErrorCode.NoError)
            {
                throw new InvalidOperationException("OpenGL Error : " + err);
            }
        }

        private static void init_textures(Data data)
        {
            tex = new uint[6];

            IntPtr ptr = Marshal.AllocHGlobal(4 * tex.Length);
            try
            {
                GL.GenTextures(tex.Length, ptr);
                GlDumpError();
                for (int i = 0; i < tex.Length; i++)
                {
                    tex[i] = (uint)Marshal.ReadInt32(ptr, i * 4);
                }
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }

            StringBuilder sb = new StringBuilder("tex:");
            for (int i = 0; i < tex.Length; i++)
            {
                sb.AppendFormat(" {0:X2}", tex[i]);
            }
            app.Out.WriteLine(sb);

            // setup first texture
            GL.BindTexture(TextureTarget.Texture2D, tex[0]);
            GlDumpError();
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, IMAGE_SIZE, IMAGE_SIZE, 0, PixelFormat.Rgb, PixelType.UnsignedByte, data.TexBuf1);
            GlDumpError();
            GL.TexParameterf(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (float)GL_NEAREST);
            GlDumpError();
            GL.TexParameterf(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (float)GL_NEAREST);
            GlDumpError();

            // setup second texture - reuse first image
            GL.BindTexture(TextureTarget.Texture2D, tex[1]);
            GlDumpError();
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, IMAGE_SIZE, IMAGE_SIZE, 0, PixelFormat.Rgb, PixelType.UnsignedByte, data.TexBuf1);
            GlDumpError();
            GL.TexParameterf(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (float)GL_NEAREST);
            GlDumpError();
            GL.TexParameterf(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (float)GL_NEAREST);
            GlDumpError();

            // third texture
            GL.BindTexture(TextureTarget.Texture2D, tex[2]);
            GlDumpError();
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, IMAGE_SIZE, IMAGE_SIZE, 0, PixelFormat.Rgb, PixelType.UnsignedByte, data.TexBuf2);
            GlDumpError();
            GL.TexParameterf(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (float)GL_NEAREST);
            GlDumpError();
            GL.TexParameterf(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (float)GL_NEAREST);
            GlDumpError();

            // fourth texture  - reuse second image
            GL.BindTexture(TextureTarget.Texture2D, tex[3]);
            GlDumpError();
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, IMAGE_SIZE, IMAGE_SIZE, 0, PixelFormat.Rgb, PixelType.UnsignedByte, data.TexBuf2);
            GlDumpError();
            GL.TexParameterf(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (float)GL_NEAREST);
            GlDumpError();
            GL.TexParameterf(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (float)GL_NEAREST);
            GlDumpError();

            //fifth texture
            GL.BindTexture(TextureTarget.Texture2D, tex[4]);
            GlDumpError();
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, IMAGE_SIZE, IMAGE_SIZE, 0, PixelFormat.Rgb, PixelType.UnsignedByte, data.TexBuf3);
            GlDumpError();
            GL.TexParameterf(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (float)GL_NEAREST);
            GlDumpError();
            GL.TexParameterf(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (float)GL_NEAREST);
            GlDumpError();

            // sixth texture  - reuse third image
            GL.BindTexture(TextureTarget.Texture2D, tex[5]);
            GlDumpError();
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, IMAGE_SIZE, IMAGE_SIZE, 0, PixelFormat.Rgb, PixelType.UnsignedByte, data.TexBuf3);
            GlDumpError();
            GL.TexParameterf(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (float)GL_NEAREST);
            GlDumpError();
            GL.TexParameterf(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (float)GL_NEAREST);
            GlDumpError();

            // setup overall texture environment
            GL.TexCoordPointer(2, TexCoordPointerType.Float, 0, data.TexCoords);
            GlDumpError();
            GL.EnableClientState(EnableCap.TextureCoordArray);
            GlDumpError();

            GL.Enable(EnableCap.Texture2D);
            GlDumpError();
        }

        private static void update_model()
        {
            // update position
            rot[0] = inc_and_wrap_angle(rot[0], rateOfRotationPS[0]);
            rot[1] = inc_and_wrap_angle(rot[1], rateOfRotationPS[1]);
            rot[2] = inc_and_wrap_angle(rot[2], rateOfRotationPS[2]);
            distance = inc_and_clip_distance(distance, distance_inc);

            GL.LoadIdentity();
            // move camera back to see the cube
            GL.Translatef(0.0f, 0.0f, -distance);

            // Rotate model to new position
            GL.Rotatef(rot[0], 0.0f, 1.0f, 0.0f);
            GL.Rotatef(rot[1], 0.0f, 0.0f, 1.0f);
            GL.Rotatef(rot[2], 1.0f, 0.0f, 0.0f);
        }

        private static float inc_and_wrap_angle(float angle, float angle_inc)
        {
            angle += angle_inc;

            if (angle >= 360.0)
                angle -= 360.0f;
            else if (angle <= 0)
                angle += 360.0f;

            return angle;
        }

        private static float inc_and_clip_distance(float dist, float dist_inc)
        {
            dist += dist_inc;

            if (dist >= 120.0f)
                dist = 120.0f;
            else if (dist <= 40.0f)
                dist = 40.0f;

            return dist;
        }

        private static void redraw_scene(EglSurface surface)
        {
            // Start with a clear screen
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GlDumpError();

            // Draw first (front) face:
            // Bind texture surface to current vertices
            GL.BindTexture(TextureTarget.Texture2D, tex[0]);
            GlDumpError();

            // Need to rotate textures - do this by rotating each cube face
            GL.Rotatef(270.0f, 0.0f, 0.0f, 1.0f); // front face normal along z axis
            GlDumpError();

            // draw first 4 vertices
            GL.DrawArrays(PrimitiveType.TriangleStrip, 0, 4);
            GlDumpError();

            // same pattern for other 5 faces - rotation chosen to make image orientation 'nice'
            GL.BindTexture(TextureTarget.Texture2D, tex[1]);
            GlDumpError();
            GL.Rotatef(90.0f, 0.0f, 0.0f, 1.0f); // back face normal along z axis
            GlDumpError();
            GL.DrawArrays(PrimitiveType.TriangleStrip, 4, 4);
            GlDumpError();

            GL.BindTexture(TextureTarget.Texture2D, tex[2]);
            GlDumpError();
            GL.Rotatef(90.0f, 1.0f, 0.0f, 0.0f); // left face normal along x axis
            GlDumpError();
            GL.DrawArrays(PrimitiveType.TriangleStrip, 8, 4);
            GlDumpError();

            GL.BindTexture(TextureTarget.Texture2D, tex[3]);
            GlDumpError();
            GL.Rotatef(90.0f, 1.0f, 0.0f, 0.0f); // right face normal along x axis
            GlDumpError();
            GL.DrawArrays(PrimitiveType.TriangleStrip, 12, 4);
            GlDumpError();

            GL.BindTexture(TextureTarget.Texture2D, tex[4]);
            GlDumpError();
            GL.Rotatef(270.0f, 0.0f, 1.0f, 0.0f); // top face normal along y axis
            GlDumpError();
            GL.DrawArrays(PrimitiveType.TriangleStrip, 16, 4);
            GlDumpError();

            GL.BindTexture(TextureTarget.Texture2D, tex[5]);
            GlDumpError();
            GL.Rotatef(90.0f, 0.0f, 1.0f, 0.0f); // bottom face normal along y axis
            GlDumpError();
            GL.DrawArrays(PrimitiveType.TriangleStrip, 20, 4);
            GlDumpError();

            surface.SwapBuffers();
        }
    }
}
