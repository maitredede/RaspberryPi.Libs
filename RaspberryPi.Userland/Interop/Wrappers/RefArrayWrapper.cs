using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace RaspberryPi.Userland.Interop.Wrappers
{
    internal sealed class RefArrayWrapper<T> : BaseArrayWrapper<T>
    {
        public RefArrayWrapper(Func<IntPtr> arrayPtrGetter, Func<int> arraySizeGetter, int itemSize, Func<IntPtr, T> wrapperBuilder)
            : base(arrayPtrGetter, arraySizeGetter, itemSize, wrapperBuilder)
        {
        }

        protected override T[] GetData()
        {
            int count = this.m_arraySize();
            IntPtr ptr = this.m_arrayPtr();
            if (ptr == IntPtr.Zero)
                return null;

            T[] items = new T[count];

            for (int i = 0; i < count; i++)
            {
                IntPtr itemPtr = ptr + i * Marshal.SizeOf<IntPtr>();
                IntPtr realPtr = Marshal.ReadIntPtr(itemPtr);
                items[i] = this.m_wrapperBuilder(realPtr);
            }
            return items;
        }
    }
}
