using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace RaspberryPi.Userland.Interop
{
    /// <summary>
    /// Interop helper class
    /// </summary>
    /// <typeparam name="T">Structure of native data</typeparam>
    public sealed class InteropHandler<T>
    {
        private static readonly MethodInfo MarshalSizeOf = typeof(Marshal).GetTypeInfo().GetMethod(nameof(Marshal.SizeOf), Type.EmptyTypes);
        private static readonly MethodInfo MarshalPtrToStruct = typeof(Marshal).GetTypeInfo().GetMethod(nameof(Marshal.PtrToStructure), new[] { typeof(IntPtr) });
        private static readonly MethodInfo MarshalStructToPtr = typeof(Marshal).GetTypeInfo().GetMethods().Where(m => m.Name == nameof(Marshal.StructureToPtr) && m.IsGenericMethodDefinition).FirstOrDefault();

        private readonly InteropHandlerData[] s_data;
        private readonly Dictionary<string, InteropHandlerData> s_offsets;

        private readonly Func<IntPtr> m_ptrGetter;
        public IntPtr GetPtr()
        {
            return this.m_ptrGetter();
        }

        public InteropHandler(Func<IntPtr> ptrGetter)
        {
            this.m_ptrGetter = ptrGetter;

            var lst = new List<InteropHandlerData>();
            foreach (MemberInfo member in typeof(T).GetTypeInfo().GetMembers(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                switch (member.MemberType)
                {
                    case MemberTypes.Property:
                    case MemberTypes.Field:
                        break;
                    default:
                        continue;
                }
                //Console.WriteLine($"Analyzing {member}");
                MarshalAsAttribute marshalAs = member.GetCustomAttribute<MarshalAsAttribute>(true);
                Type t = (member as PropertyInfo)?.PropertyType ?? (member as FieldInfo).FieldType;

                InteropHandlerData data = new InteropHandlerData(member, lst.Count/*, marshalAs*/);
                data.DataType = t;

                lst.Add(data);
                if (marshalAs == null)
                {
                    if (t == typeof(IntPtr))
                    {
                        data.MemorySize = Marshal.SizeOf<IntPtr>();
                        data.MarshalRead = ptr => Marshal.ReadIntPtr(ptr);
                        data.MarshalWrite = (ptr, value) => Marshal.WriteIntPtr(ptr, (IntPtr)value);
                        continue;
                    }
                    if (t == typeof(uint))
                    {
                        data.MemorySize = Marshal.SizeOf<uint>();
                        data.MarshalRead = ptr => unchecked((uint)Marshal.ReadInt32(ptr));
                        data.MarshalWrite = (ptr, value) => Marshal.WriteInt32(ptr, unchecked((int)(uint)value));
                        continue;
                    }
                    var ti = t.GetTypeInfo();
                    Console.WriteLine("Getting  StructLayoutAttribute for " + ti.FullName);
                    StructLayoutAttribute sla;
                    try
                    {
                        sla = ti.StructLayoutAttribute;
                    }
                    catch
                    {
                        sla = null;
                    }
                    if (sla != null)
                    {
                        data.MemorySize = (int)MarshalSizeOf.MakeGenericMethod(t).Invoke(null, null);
                        data.MarshalRead = ptr => MarshalPtrToStruct.MakeGenericMethod(t).Invoke(null, new object[] { ptr });
                        data.MarshalWrite = (ptr, value) => MarshalStructToPtr.MakeGenericMethod(t).Invoke(null, new object[] { ptr, value, false });
                        continue;
                    }
                    else
                    {
                        Console.WriteLine("TODO : unknown marshaling strategy for type " + t.FullName);
                        lst.Remove(data);
                    }
                    continue;
                }
                else
                {
                    int size = marshalAs.SizeConst;
                    switch (marshalAs.Value)
                    {
                        case UnmanagedType.LPStr:
                            data.MemorySize = Marshal.SizeOf<IntPtr>();
                            data.MarshalRead = (ptr) =>
                            {
                                var stringPtr = Marshal.ReadIntPtr(ptr);
                                if (stringPtr == IntPtr.Zero)
                                    return null;
                                return Marshal.PtrToStringAnsi(stringPtr);
                            };
                            data.MarshalWrite = (ptr, value) => throw new NotImplementedException();
                            break;
                        case UnmanagedType.U2:
                            data.MemorySize = Marshal.SizeOf<ushort>();
                            data.MarshalRead = (ptr) => throw new NotImplementedException();
                            data.MarshalWrite = (ptr, value) => throw new NotImplementedException();
                            break;
                        case UnmanagedType.U4:
                            data.MemorySize = Marshal.SizeOf<uint>();
                            data.MarshalRead = ptr => unchecked((uint)Marshal.ReadInt32(ptr));
                            data.MarshalWrite = (ptr, value) => Marshal.WriteInt32(ptr, unchecked((int)(uint)value));
                            break;
                        case UnmanagedType.I4:
                            data.MemorySize = Marshal.SizeOf<int>();
                            data.MarshalRead = ptr => Marshal.ReadInt32(ptr);
                            data.MarshalWrite = (ptr, value) => Marshal.WriteInt32(ptr, (int)value);
                            break;
                        case UnmanagedType.ByValTStr:
                            data.MemorySize = size;
                            data.MarshalRead = ptr => Marshal.PtrToStringAnsi(ptr);
                            data.MarshalWrite = (ptr, value) => throw new NotImplementedException();
                            break;
                        case UnmanagedType.ByValArray:
                            data.MemorySize = size;
                            data.MarshalRead = ptr =>
                            {
                                byte[] byvalArr = new byte[size];
                                Marshal.Copy(ptr, byvalArr, 0, size);
                                return byvalArr;
                            };
                            data.MarshalWrite = (ptr, value) =>
                            {
                                byte[] byvalArr = (byte[])value;
                                Marshal.Copy(byvalArr, 0, ptr, size);
                            };
                            break;
                        default:
                            Console.WriteLine($"Unknown size for type {marshalAs.Value}");
                            break;
                    }
                }

            }
            lst.Sort((x, y) => x.Index.CompareTo(y.Index));
            s_data = lst.ToArray();
            for (int i = 0; i < s_data.Length; i++)
            {
                if (s_data[i].Index != i)
                    throw new InvalidOperationException($"Bad field index on {typeof(T).FullName} for theorical index {i}");

                int offset = 0;
                if (i > 0)
                {
                    offset = s_data[i - 1].Offset + s_data[i - 1].MemorySize;
                }
                s_data[i].Offset = offset;
            }
            s_offsets = s_data.ToDictionary(d => d.Member.Name);
        }

        public int GetOffset(string prop)
        {
            return s_offsets[prop].Offset;
        }

        public void Dump()
        {
            Console.WriteLine($"== Accessors for {typeof(T).FullName} ({s_data.Length} fields)");
            for (int i = 0; i < this.s_data.Length; i++)
            {
                var d = this.s_data[i];
                Type type = (d.Member as FieldInfo)?.FieldType ?? (d.Member as PropertyInfo)?.PropertyType ?? typeof(void);
                Console.WriteLine($"\t{d.Member.Name} @{d.Offset} s{d.MemorySize} t{type}");
            }
        }

        public void DumpHex(string property, int offset, int count)
        {
            InteropHandlerData data = this.s_offsets[property];
            int min = data.Offset + offset;
            int max = min + count;
            Console.WriteLine($"@{property} ({data.Offset}) from {offset} count {count}");
            for (int i = min; i < max; i++)
            {
                byte b = Marshal.ReadByte(this.m_ptrGetter(), i);
                Console.Write("{0:X2} ", b);
            }
            Console.WriteLine();
        }

        public string ReadString([CallerMemberName]string prop = null)
        {
            InteropHandlerData data = this.s_offsets[prop];
            if (data == null)
            {
                Console.WriteLine($"Member {prop} for type {typeof(T).Name} not found in offsets ");
            }
            if (data.MarshalRead == null)
            {
                Console.WriteLine($"Member {prop} for type {typeof(T).Name} has no MarshalRead for data type {(data.DataType)}");
            }
            try
            {
                return (string)data.MarshalRead(this.m_ptrGetter() + data.Offset);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Member {prop} for type {typeof(T).Name} error in readstring");
                System.Runtime.ExceptionServices.ExceptionDispatchInfo.Capture(ex).Throw();
                return null;
            }
            //switch (data.StringMode)
            //{
            //    case StringMode.LPStr:
            //        IntPtr ptr = this.ReadIntPtr(prop);
            //        return Marshal.PtrToStringAnsi(ptr);
            //    case StringMode.SizeConst:
            //        StringBuilder sb = new StringBuilder();
            //        for (int i = 0; i < data.TypeSize; i++)
            //        {
            //            byte b = Marshal.ReadByte(this.m_ptrGetter(), data.Offset + i);
            //            if (b == 0)
            //                break;
            //            sb.Append((char)b);
            //        }
            //        return sb.ToString();
            //    default:
            //        throw new InvalidOperationException("Invalid stringmode for property " + prop);
            //}
            //throw new NotImplementedException();
        }

        public void WriteString(string value, [CallerMemberName]string prop = null)
        {
            //InteropHandlerData data = this.s_offsets[prop];
            //switch (data.StringMode)
            //{
            //    case StringMode.LPStr:
            //        if (value == null)
            //        {
            //            this.WriteIntPtr(IntPtr.Zero, prop);
            //            return;
            //        }
            //        IntPtr ptr = Marshal.StringToHGlobalAnsi(value);
            //        this.m_allocatedPointers.Add(ptr);
            //        Marshal.WriteIntPtr(this.m_ptrGetter(), data.Offset, ptr);
            //        break;
            //    case StringMode.SizeConst:
            //        for (int i = 0; i < data.TypeSize; i++)
            //        {
            //            Marshal.WriteByte(this.m_ptrGetter(), data.Offset + i, 0);
            //        }
            //        if (string.IsNullOrEmpty(value))
            //            return;
            //        byte[] b = Encoding.UTF8.GetBytes(value);
            //        for (int i = 0; i < b.Length; i++)
            //        {
            //            Marshal.WriteByte(this.m_ptrGetter(), data.Offset + i, b[i]);
            //        }
            //        break;
            //    default:
            //        throw new InvalidOperationException("Invalid stringmode for property " + prop);
            //}
            throw new NotImplementedException();
        }

        public TStruct ReadStruct<TStruct>([CallerMemberName]string prop = null)
        {
            InteropHandlerData data = this.s_offsets[prop];
            IntPtr ptr = this.m_ptrGetter() + data.Offset;
            return Marshal.PtrToStructure<TStruct>(ptr);
        }

        public void WriteStruct<TStruct>(TStruct value, [CallerMemberName]string prop = null)
        {
            InteropHandlerData data = this.s_offsets[prop];
            IntPtr ptr = this.m_ptrGetter() + data.Offset;
            Marshal.StructureToPtr(value, ptr, false);
        }

        public void WriteIntPtr(IntPtr value, [CallerMemberName]string prop = null)
        {
            InteropHandlerData data = this.s_offsets[prop];
            Marshal.WriteIntPtr(this.m_ptrGetter(), data.Offset, value);
        }

        public IntPtr ReadIntPtr([CallerMemberName]string prop = null)
        {
            InteropHandlerData data = this.s_offsets[prop];
            return Marshal.ReadIntPtr(this.m_ptrGetter(), data.Offset);
        }

        public void WriteInt32(int value, [CallerMemberName]string prop = null)
        {
            InteropHandlerData data = this.s_offsets[prop];
            Marshal.WriteInt32(this.m_ptrGetter(), data.Offset, value);
        }

        public int ReadInt32([CallerMemberName]string prop = null)
        {
            InteropHandlerData data = this.s_offsets[prop];
            return Marshal.ReadInt32(this.m_ptrGetter(), data.Offset);
        }

        public void WriteUInt32(uint value, [CallerMemberName]string prop = null)
        {
            InteropHandlerData data = this.s_offsets[prop];
            Marshal.WriteInt32(this.m_ptrGetter(), data.Offset, unchecked((int)value));
        }

        public uint ReadUInt32([CallerMemberName]string prop = null)
        {
            InteropHandlerData data = this.s_offsets[prop];
            return unchecked((uint)Marshal.ReadInt32(this.m_ptrGetter(), data.Offset));
        }

        public void WriteBool_Byte(bool value, [CallerMemberName]string prop = null)
        {
            InteropHandlerData data = this.s_offsets[prop];
            Marshal.WriteByte(this.m_ptrGetter(), data.Offset, value ? (byte)1 : (byte)0);
        }

        public bool ReadBool_Byte([CallerMemberName]string prop = null)
        {
            InteropHandlerData data = this.s_offsets[prop];
            return Marshal.ReadByte(this.m_ptrGetter(), data.Offset) != 0;
        }
    }
}
