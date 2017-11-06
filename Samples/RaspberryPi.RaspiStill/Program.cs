using RaspberryPi;
using RaspberryPi.Userland;
using RaspberryPi.Userland.MMAL;
using System;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace RaspberryPi.RaspiStill
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Raspberry pi image capture sample program");
            int camNumber = 0;
            using (BcmHost host = new BcmHost())
            {
                Console.WriteLine("Host created");
                //set_sensor_defaults
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
                }
            }
            Console.WriteLine("Tadaaa !");
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
