using RaspberryPi.Interop;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace RaspberryPi.MMAL.Interop.Wrappers
{
    internal sealed class MMAL_COMPONENT_T_Wrapper
    {
        private readonly InteropHandler<MMAL_COMPONENT_T> m_handler;

        public MMAL_COMPONENT_T_Wrapper(IntPtr ptr)
        {
            this.m_handler = new InteropHandler<MMAL_COMPONENT_T>(() => ptr);
        }

        public string name { get { return this.m_handler.ReadString(); } }

        private MMAL_PORT_T_Wrapper m_control;
        public MMAL_PORT_T_Wrapper control
        {
            get
            {
                if (this.m_control == null)
                {
                    this.m_control = new MMAL_PORT_T_Wrapper(() => this.m_handler.ReadIntPtr(nameof(MMAL_COMPONENT_T.control)));
                }
                return this.m_control;
            }
        }

        public int output_num { get { return (int)this.m_handler.ReadUInt32(); } }

        private RefArrayWrapper<MMAL_PORT_T_Wrapper> m_output;
        public RefArrayWrapper<MMAL_PORT_T_Wrapper> output
        {
            get
            {
                if (this.m_output == null)
                {
                    //Console.WriteLine("############################################");
                    //var data = this.GetOutput();
                    //Console.WriteLine("############################################");
                    this.m_output = new RefArrayWrapper<MMAL_PORT_T_Wrapper>(
                       () => this.m_handler.ReadIntPtr(nameof(MMAL_COMPONENT_T.output)),
                       () => this.output_num,
                       MMAL_PORT_T_Wrapper.Size,
                       ptr => new MMAL_PORT_T_Wrapper(() => ptr)
                        );
                    //var data2 = this.m_output.Items;
                    //for (int i = 0; i < data2.Length; i++)
                    //{
                    //    Console.Write("port " + i + ": ");
                    //    Console.WriteLine(data2[i].name);
                    //}
                    //Console.WriteLine("############################################");
                }
                return this.m_output;
            }
        }

        //public MMAL_PORT_T_Wrapper[] GetOutput()
        //{
        //    IntPtr ptr = this.m_handler.ReadIntPtr(nameof(MMAL_COMPONENT_T.output));
        //    int arrLength = this.output_num;
        //    MMAL_PORT_T_Wrapper[] arr;
        //    try
        //    {
        //        Console.WriteLine("Reading as array of ptr");
        //        arr = this.GetPortArray_ArrayPtr(ptr, arrLength);
        //        for (int i = 0; i < arr.Length; i++)
        //        {
        //            Console.Write("port " + i + ": ");
        //            Console.WriteLine(arr[i].name);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.GetType().FullName + ": " + ex.Message);
        //        arr = null;
        //    }
        //    //try
        //    //{
        //    //    Console.WriteLine("Reading as array of structs");
        //    //    arr = this.GetPortArray_ArrayStruct(ptr, arrLength);
        //    //    for (int i = 0; i < arr.Length; i++)
        //    //    {
        //    //        Console.Write("port " + i + ": ");
        //    //        Console.WriteLine(arr[i].name);
        //    //    }
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    Console.WriteLine(ex.GetType().FullName + ": " + ex.Message);
        //    //    arr = null;
        //    //}

        //    return arr;
        //}

        //private MMAL_PORT_T_Wrapper[] GetPortArray_ArrayStruct(IntPtr ptr, int arrLength)
        //{
        //    if (ptr == IntPtr.Zero)
        //        return null;
        //    MMAL_PORT_T_Wrapper[] arr = new MMAL_PORT_T_Wrapper[arrLength];
        //    for (int i = 0; i < arrLength; i++)
        //    {
        //        var itemPtr = ptr + i * MMAL_PORT_T_Wrapper.Size;
        //        arr[i] = new MMAL_PORT_T_Wrapper(() => itemPtr);
        //    }
        //    return arr;
        //}

        //private MMAL_PORT_T_Wrapper[] GetPortArray_ArrayPtr(IntPtr ptr, int arrLength)
        //{
        //    if (ptr == IntPtr.Zero)
        //        return null;
        //    MMAL_PORT_T_Wrapper[] arr = new MMAL_PORT_T_Wrapper[arrLength];
        //    for (int i = 0; i < arrLength; i++)
        //    {
        //        var itemPtr = ptr + i * Marshal.SizeOf<IntPtr>();
        //        var itemValue = Marshal.ReadIntPtr(itemPtr);
        //        arr[i] = new MMAL_PORT_T_Wrapper(() => itemValue);
        //    }
        //    return arr;
        //}
    }
}
