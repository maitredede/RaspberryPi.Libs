using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace RaspberryPi.LibNFC.Interop
{
    public abstract class NativeCollection<T> : RaspberryPi.Interop.NativeObject, ICollection<T>, IReadOnlyCollection<T>
    {
        private T[] m_items;

        public int Count
        {
            get
            {
                this.EnsureArray();
                return ((ICollection<T>)m_items).Count;
            }
        }

        public bool IsReadOnly => true;

        public NativeCollection(IntPtr collectionPtr, bool ownsHandle) : base(collectionPtr, ownsHandle)
        {
        }

        protected abstract T[] BuildArrayFromHandle(IntPtr ptr);

        protected void EnsureArray()
        {
            if (this.m_items == null)
            {
                this.m_items = this.BuildArrayFromHandle(this.handle);
            }
        }

        void ICollection<T>.Add(T item)
        {
            throw new NotSupportedException();
        }

        void ICollection<T>.Clear()
        {
            throw new NotSupportedException();
        }

        public bool Contains(T item)
        {
            this.EnsureArray();
            return ((ICollection<T>)m_items).Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            this.EnsureArray();
            ((ICollection<T>)m_items).CopyTo(array, arrayIndex);
        }

        bool ICollection<T>.Remove(T item)
        {
            throw new NotSupportedException();
        }

        public IEnumerator<T> GetEnumerator()
        {
            this.EnsureArray();
            return ((ICollection<T>)m_items).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            this.EnsureArray();
            return ((ICollection<T>)m_items).GetEnumerator();
        }
    }
}
