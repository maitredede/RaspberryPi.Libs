using RaspberryPi.LibFreefare.Interop;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace RaspberryPi.LibFreefare
{
    public sealed class NTAG21xKey : IDisposable
    {
        private readonly IntPtr m_ptr;

        public NTAG21xKey(byte[] data, byte[] pack)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            if (data.Length != 4)
                throw new ArgumentOutOfRangeException(nameof(data), "Data length must be 4");
            if (pack == null)
                throw new ArgumentNullException(nameof(pack));
            if (pack.Length != 2)
                throw new ArgumentOutOfRangeException(nameof(pack), "Pack length must be 2");

            GCHandle hData = GCHandle.Alloc(data, GCHandleType.Pinned);
            try
            {
                GCHandle hPack = GCHandle.Alloc(pack, GCHandleType.Pinned);
                try
                {
                    this.m_ptr = NativeMethods.ntag21x_key_new(hData.AddrOfPinnedObject(), hPack.AddrOfPinnedObject());
                }
                finally
                {
                    hPack.Free();
                }
            }
            finally
            {
                hData.Free();
            }
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

                NativeMethods.ntag21x_key_free(this.m_ptr);

                disposedValue = true;
            }
        }

        ~NTAG21xKey()
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
    }
}
