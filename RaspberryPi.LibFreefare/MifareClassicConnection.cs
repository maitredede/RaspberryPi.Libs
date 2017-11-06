using RaspberryPi.LibFreefare.Interop;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace RaspberryPi.LibFreefare
{
    public sealed class MifareClassicConnection : IDisposable
    {
        private readonly MifareClassicTag m_tag;

        internal MifareClassicConnection(MifareClassicTag tag)
        {
            this.m_tag = tag;
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

                int code = NativeMethods.mifare_classic_disconnect(this.m_tag.Handle);
                if (code < 0)
                {
                    Console.WriteLine("Error disconnecting tag: " + this.m_tag.LastError());
                }
                // TODO: libérer les ressources non managées (objets non managés) et remplacer un finaliseur ci-dessous.
                // TODO: définir les champs de grande taille avec la valeur Null.

                disposedValue = true;
            }
        }

        ~MifareClassicConnection()
        {
            // Ne modifiez pas ce code. Placez le code de nettoyage dans Dispose(bool disposing) ci-dessus.
            Dispose(false);
        }

        // Ce code est ajouté pour implémenter correctement le modèle supprimable.
        void IDisposable.Dispose()
        {
            // Ne modifiez pas ce code. Placez le code de nettoyage dans Dispose(bool disposing) ci-dessus.
            Dispose(true);
            // TODO: supprimer les marques de commentaire pour la ligne suivante si le finaliseur est remplacé ci-dessus.
            GC.SuppressFinalize(this);
        }
        #endregion

        public void Authenticate(byte block, MifareClassicKey key, MifareClassicKeyType keyType)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));
            if (!key.IsValid())
                throw new ArgumentException("Key is not valid", nameof(key));
            int ret;
            GCHandle h = GCHandle.Alloc(key.Value, GCHandleType.Pinned);
            try
            {
                ret = NativeMethods.mifare_classic_authenticate(this.m_tag.Handle, block, h.AddrOfPinnedObject(), keyType);
            }
            finally
            {
                h.Free();
            }

            if (ret < 0)
                throw new Exception("Authentication error: " + this.m_tag.LastError());
        }
    }
}
