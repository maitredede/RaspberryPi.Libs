using RaspberryPi.LibFreefare.Interop;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace RaspberryPi.LibFreefare
{
    public sealed class FreefareTag : IDisposable
    {
        private readonly IntPtr m_ptr;
        private readonly bool m_dispose;

        private TagType? m_type;
        private string m_name;
        private string m_uid;

        internal FreefareTag(IntPtr ptr, bool dispose)
        {
            this.m_ptr = ptr;
            this.m_dispose = dispose;
        }

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

                // TODO: libérer les ressources non managées (objets non managés) et remplacer un finaliseur ci-dessous.
                // TODO: définir les champs de grande taille avec la valeur Null.
                if (this.m_dispose)
                {
                    NativeMethods.freefare_free_tag(this.m_ptr);
                }

                disposedValue = true;
            }
        }

        ~FreefareTag()
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

        public TagType Type
        {
            get
            {
                if (!this.m_type.HasValue)
                {
                    this.m_type = NativeMethods.freefare_get_tag_type(this.m_ptr);
                }
                return this.m_type.Value;
            }
        }

        public string Name
        {
            get
            {
                if (this.m_name == null)
                {
                    IntPtr namePtr = NativeMethods.freefare_get_tag_friendly_name(this.m_ptr);
                    this.m_name = Marshal.PtrToStringAnsi(namePtr);
                }
                return this.m_name;
            }
        }

        public string UID
        {
            get
            {
                if (this.m_uid == null)
                {
                    this.m_uid = NativeMethods.freefare_get_tag_uid(this.m_ptr);
                }
                return this.m_uid;
            }
        }
    }
}
