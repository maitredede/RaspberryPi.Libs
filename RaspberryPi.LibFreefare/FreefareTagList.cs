using RaspberryPi.LibFreefare.Interop;
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace RaspberryPi.LibFreefare
{
    public sealed class FreefareTagList : IDisposable, ICollection<FreefareTag>
    {
        private readonly IntPtr[] m_ptrs;
        private readonly FreefareTag[] m_tags;

        internal FreefareTagList(IntPtr[] ptrs)
        {
            this.m_ptrs = ptrs;

            this.m_tags = new FreefareTag[this.m_ptrs.Length];
            for (int i = 0; i < this.m_ptrs.Length; i++)
            {
                this.m_tags[i] = new FreefareTag(this.m_ptrs[i], false);
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // Pour détecter les appels redondants

        public int Count => throw new NotImplementedException();

        public bool IsReadOnly => throw new NotImplementedException();

        void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: supprimer l'état managé (objets managés).
                }

                NativeMethods.freefare_free_tags(this.m_ptrs);

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
            for(int i = 0; i < this.m_tags.Length; i++)
            {
                if (this.m_tags[i].Equals(item))
                    return true;
            }
            return false;
        }

        public void CopyTo(FreefareTag[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        bool ICollection<FreefareTag>.Remove(FreefareTag item)
        {
            throw new NotSupportedException();
        }

        public IEnumerator<FreefareTag> GetEnumerator()
        {
            return (IEnumerator<FreefareTag>)this.m_tags.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.m_tags.GetEnumerator();
        }
    }
}
