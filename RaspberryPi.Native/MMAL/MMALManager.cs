using RaspberryPi.MMAL.Interop;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace RaspberryPi.MMAL
{
    public sealed class MMALManager
    {
        internal MMALManager()
        {
        }

        private /*MMAL_COMPONENT_T*/ IntPtr CreateComponentHandle(MMALComponentName componentName)
        {
            Console.WriteLine("Creating component " + componentName.Value);
            //var status = NativeMethods.ComponentCreate(componentName.Value, out MMAL_COMPONENT_T handle);
            //if (status != MMAL_STATUS_T.MMAL_SUCCESS)
            //    throw new MMALException(status);

            //handle.DumpFields();

            //return handle;

            var status = NativeMethods.ComponentCreate(componentName.Value, out IntPtr handle);
            if (status != MMAL_STATUS_T.MMAL_SUCCESS)
                throw new MMALException(status);
            return handle;

            //Console.WriteLine($"Ptr={handle}");
            //for (int i = 1; i <= 100; i++)
            //{
            //    byte b = Marshal.ReadByte(handle, i - 1);
            //    Console.Write("{0:X2} ", b);
            //    if (i % 16 == 0)
            //        Console.WriteLine();
            //    else if (i % 8 == 0)
            //        Console.Write("  ");

            //}
            //IntPtr namePtr = Marshal.ReadIntPtr(handle, Marshal.SizeOf<IntPtr>() * 2);
            //Console.WriteLine($"namePtr={namePtr}");
            //string name = Marshal.PtrToStringAnsi(namePtr);
            //Console.WriteLine($"name={name}");

            //MMAL_COMPONENT_T comp = Marshal.PtrToStructure<MMAL_COMPONENT_T>(handle);

            //comp.DumpFields();

            ////throw new NotImplementedException();
            //return comp;
        }

        public CameraComponent ComponentCreateCamera()
        {
            var compName = MMALComponentName.Camera;
            var handle = this.CreateComponentHandle(compName);
            return new CameraComponent(handle, compName);
        }

        public CameraInfoComponent ComponentCreateCameraInfo()
        {
            var compName = MMALComponentName.CameraInfo;
            var handle = this.CreateComponentHandle(compName);
            return new CameraInfoComponent(handle, compName);
        }

        /// <summary>
        /// Create an instance of a component.
        /// The newly created component will expose ports to the client .All the exposed ports are
        /// disabled by default.
        /// Note that components are reference counted and creating a component automatically
        /// acquires a reference to it (released when Dispose is called).
        /// </summary>
        /// <param name="componentName">name of the component to create, e.g. "video_decode"</param>
        /// <returns>returned component on success</returns>
        internal MMALComponent ComponentCreate(MMALComponentName componentName)
        {
            if (componentName == null)
                throw new ArgumentNullException(nameof(componentName));
            var handle = this.CreateComponentHandle(componentName);
            return new MMALComponent(handle, componentName);
        }
    }
}
