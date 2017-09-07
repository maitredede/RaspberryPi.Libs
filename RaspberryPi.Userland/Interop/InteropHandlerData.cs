using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace RaspberryPi.Userland.Interop
{
    internal sealed class InteropHandlerData
    {
        internal InteropHandlerData(MemberInfo member, int index/*, MarshalAsAttribute marshalAs*/)
        {
            this.Member = member;
            this.Index = index;
            //this.MarshalAs = marshalAs;
        }

        public MemberInfo Member { get; }
        public int Index { get; }
        //public MarshalAsAttribute MarshalAs { get; set; }
        public int MemorySize { get; internal set; }
        public int Offset { get; internal set; }
        public Func<IntPtr, object> MarshalRead { get; set; }
        public Action<IntPtr, object> MarshalWrite { get; set; }
        public Type DataType { get; internal set; }
    }
}
