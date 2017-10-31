using RaspberryPi.LibFreefare.Interop;
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Runtime.InteropServices;

namespace RaspberryPi.LibFreefare
{
    public sealed class FreefareTagList : IDisposable, ICollection<FreefareTag>
    {
        private readonly IntPtr m_ptr;
        private readonly FreefareTag[] m_tags;

        internal FreefareTagList(IntPtr ptr)
        {
            this.m_ptr = ptr;

            List<FreefareTag> lst = new List<FreefareTag>();
            int i = 0;
            IntPtr ptrTag;
            while ((ptrTag = Marshal.ReadIntPtr(this.m_ptr, i++)) != IntPtr.Zero)
            {
                Console.WriteLine("i={0} ptr={1}", i, ptrTag);

                TagType type = NativeMethods.freefare_get_tag_type(ptrTag);
                FreefareTag tag;
                switch (type)
                {
                    case TagType.NTAG_21x:
                        tag = new NTAG21xTag(ptrTag, false);
                        break;
                    default:
                        tag = new GenericTag(ptrTag, false);
                        break;
                }
                lst.Add(tag);
            }
            this.m_tags = lst.ToArray();
        }

        public int Count => this.m_tags.Length;

        public bool IsReadOnly => true;

        #region IDisposable Support
        private bool disposedValue = false; // Pour détecter les appels redondants

        void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: supprimer l'état managé (objets managés).
                }

                NativeMethods.freefare_free_tags(this.m_ptr);

                disposedValue = true;
            }
        }

        ~FreefareTagList()
        {
            // Ne modifiez pas ce code. Placez le code de nettoyage dans Dispose(bool disposing) ci-dessus.
            Dispose(false);
        }

        // Ce code est ajouté pour implémenter correctement le modèle supprimable.
        void IDisposable.Dispose()
        {
            // Ne modifiez pas ce code. Placez le code de nettoyage dans Dispose(bool disposing) ci-dessus.
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

        void ICollection<FreefareTag>.Add(FreefareTag item)
        {
            throw new NotSupportedException();
        }

        void ICollection<FreefareTag>.Clear()
        {
            throw new NotSupportedException();
        }

        public bool Contains(FreefareTag item)
        {
            for (int i = 0; i < this.m_tags.Length; i++)
            {
                if (this.m_tags[i].Equals(item))
                    return true;
            }
            return false;
        }

        void ICollection<FreefareTag>.CopyTo(FreefareTag[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        bool ICollection<FreefareTag>.Remove(FreefareTag item)
        {
            throw new NotSupportedException();
        }

        public IEnumerator<FreefareTag> GetEnumerator()
        {
            return ((IEnumerable<FreefareTag>)this.m_tags).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.m_tags.GetEnumerator();
        }
    }
}
