using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace RaspberryPi.LibLedMatrix.Debugging
{
    public static class LedMatrixOptionExtensions
    {
        public static void Dump(this LedMatrixOptions options, StreamWriter output)
        {
            output.WriteLine("--- {0} ---", options.GetType().Name);
            foreach(MemberInfo mi in options.GetType().GetMembers())
            {
                if(mi.MemberType == MemberTypes.Field)
                {
                    output.WriteLine("{0}={1}", mi.Name, ((FieldInfo)mi).GetValue(options));
                    continue;
                }
                if(mi.MemberType==MemberTypes.Property && ((PropertyInfo)mi).CanRead)
                {
                    output.WriteLine("{0}={1}", mi.Name, ((PropertyInfo)mi).GetValue(options));
                    continue;
                }
            }
        }
    }
}
