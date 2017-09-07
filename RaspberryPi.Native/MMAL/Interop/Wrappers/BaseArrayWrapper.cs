using System;
using System.Collections.Generic;
using System.Text;

namespace RaspberryPi.MMAL.Interop.Wrappers
{
    internal abstract class BaseArrayWrapper<T>
    {
        protected readonly Func<IntPtr> m_arrayPtr;
        protected readonly Func<int> m_arraySize;
        protected readonly int m_itemSize;
        protected readonly Func<IntPtr, T> m_wrapperBuilder;

        private T[] m_array;

        public BaseArrayWrapper(Func<IntPtr> arrayPtrGetter, Func<int> arraySizeGetter, int itemSize, Func<IntPtr, T> wrapperBuilder)
        {
            this.m_arrayPtr = arrayPtrGetter;
            this.m_arraySize = arraySizeGetter;
            this.m_itemSize = itemSize;
            this.m_wrapperBuilder = wrapperBuilder;
        }

        public void Invalidate()
        {
            this.m_array = null;
        }

        public int Length
        {
            get
            {
                this.EnsureData();
                return this.m_array.Length;
            }
        }

        public T this[int index]
        {
            get
            {
                this.EnsureData();
                return this.m_array[index];
            }
        }

        public T[] Items
        {
            get
            {
                this.EnsureData();
                return this.m_array;
            }
        }

        private void EnsureData()
        {
            if (this.m_array == null)
            {
                this.m_array = this.GetData();
            }
        }

        protected abstract T[] GetData();
    }
}
