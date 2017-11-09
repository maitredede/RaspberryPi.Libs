using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.CommandLineUtils;
using System;
using RaspberryPi.LibLedMatrix;
using RaspberryPi.Userland;
using RaspberryPi.Userland.Interfaces;
using RaspberryPi.Userland.MMAL;

namespace RaspberryPi.CameraToLedPanel
{
    public static class Program
    {
        private static CommandLineApplication app;

        public static int Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            app = new CommandLineApplication();
            app.OnExecute(new Func<int>(RunApplication));
            return app.Execute(args);
        }

        private static int RunApplication()
        {
            ServiceCollection serviceConfig = new ServiceCollection();
            serviceConfig.AddSingleton<IBcmHost, BcmHost>();
            serviceConfig.AddSingleton<LedMatrixOptions>(svc =>
            {
                return new LedMatrixOptions()
                {
                    chain_length = 2,
                    rows = 64,
                    led_rgb_sequence = "RBG"
                };
            });
            serviceConfig.AddSingleton<LedMatrix>(svc =>
            {
                return new LedMatrix(svc.GetRequiredService<LedMatrixOptions>());
            });

            using (ServiceProvider svcProvider = serviceConfig.BuildServiceProvider())
            {
                return RunApplication(svcProvider);
            }
        }

        private static int RunApplication(IServiceProvider svc)
        {
            //TODO put in config
            const int camNumber = 0;

            IBcmHost host = svc.GetRequiredService<IBcmHost>();
            LedMatrix matrix = svc.GetRequiredService<LedMatrix>();

            CameraInfo camInfo;
            using (var camInfoComp = host.MMAL.ComponentCreateCameraInfo())
            {
                Console.WriteLine("Getting camera info");
                camInfo = camInfoComp.GetCameraInfo(camNumber);
            }
            Console.WriteLine($"Camera name='{camInfo.Name}' res={camInfo.Width}x{camInfo.Height}");
            //create_camera_component
            using (var camera = host.MMAL.ComponentCreateCamera())
            {
                create_camera_component(camera, camNumber);

                //camera.Enable();

                while (true)
                {
                    //TODO : capture image
                    matrix.UpdateCanvas(canvas =>
                    {
                        //TODO : put image to canvas
                    });
                }
            }

            app.Error.WriteLine("Camera capture not implemented");
            return -1;
        }

        private static void create_camera_component(CameraComponent camera, int camNumber)
        {
            Console.WriteLine($"Camera created name={camera.Name.Value} nativeName={camera.NativeName()}");
            //camera.Dump();
            camera.SetStereoscopicMode(StereoScopicMode.Default);
            camera.SetCameraNum(camNumber);
            if (camera.OutputCount == 0)
            {
                Console.WriteLine("Camera doesn't have output ports");
                return;
            }
            //camera sensor mode
            camera.SetSensorMode(0);
            //TODO : camera settings events
        }
    }
}
