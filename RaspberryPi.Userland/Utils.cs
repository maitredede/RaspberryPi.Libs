using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace RaspberryPi.Userland
{
    public static class Utils
    {
        public static int ALIGN_UP(int x, int y)
        {
            return (x + (y) - 1) & ~((y) - 1);
        }

        public static short ALIGN_UP(short x, short y)
        {
            return (short)((x + (y) - 1) & ~((y) - 1));
        }

        public static ushort ALIGN_UP(ushort x, ushort y)
        {
            return (ushort)((x + (y) - 1) & ~((y) - 1));
        }

        internal static void DumpFields(this object obj)
        {
            foreach (FieldInfo f in obj.GetType().GetTypeInfo().GetFields())
            {
                Console.WriteLine($"{f.Name}={f.GetValue(obj)}");
            }
        }

        internal static void DumpProperties(this object obj)
        {
            foreach (PropertyInfo p in obj.GetType().GetTypeInfo().GetProperties())
            {
                Console.WriteLine($"{p.Name}={p.GetValue(obj)}");
            }
        }
    }
}
