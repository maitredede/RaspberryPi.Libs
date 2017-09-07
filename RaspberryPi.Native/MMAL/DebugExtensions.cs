using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace RaspberryPi.MMAL
{
    public static class DebugExtensions
    {
        public static void Dump(this CameraComponent camera)
        {
            Console.Write("Control port name : ");
            Console.WriteLine(camera.HandleWrapper.control.name);
            Console.Write("Output ports : ");
            Console.WriteLine(camera.HandleWrapper.output_num);
            var data = camera.HandleWrapper.output;
            for (int i = 0; i < data.Length; i++)
            {
                Console.Write("port " + i + ": ");
                Console.WriteLine(data[i].name);
            }
        }

        public static string NativeName(this MMALComponent comp)
        {
            return comp.HandleWrapper.name;
        }

        internal static void DumpMemory(IntPtr ptr, int count)
        {
            for (int i = 1; i <= count; i++)
            {
                byte b = Marshal.ReadByte(ptr, i);
                Console.Write("{0:X2} ", b);
                if (i % 16 == 0)
                {
                    Console.WriteLine();
                }
                else if (i % 8 == 0)
                {
                    Console.Write("  ");
                }
            }
        }
    }
}