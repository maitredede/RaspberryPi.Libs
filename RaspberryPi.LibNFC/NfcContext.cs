using RaspberryPi.LibNFC.Interop;
using System;
using System.Runtime.InteropServices;

namespace RaspberryPi.LibNFC
{
    /// <summary>
    /// NFC context
    /// </summary>
    public sealed class NfcContext : IDisposable, INfcContext
    {
        private readonly IntPtr m_ctx;

        /// <summary>
        /// Create a new NFC context
        /// </summary>
        public NfcContext()
        {
            NativeMethods.init(out this.m_ctx);
            if (this.m_ctx == IntPtr.Zero)
                throw new OutOfMemoryException("Can't initialize libnfc");
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
                NativeMethods.exit(this.m_ctx);

                disposedValue = true;
            }
        }

        ~NfcContext()
        {
            // Ne modifiez pas ce code. Placez le code de nettoyage dans Dispose(bool disposing) ci-dessus.
            Dispose(false);
        }

        // Ce code est ajouté pour implémenter correctement le modèle supprimable.
        public void Dispose()
        {
            // Ne modifiez pas ce code. Placez le code de nettoyage dans Dispose(bool disposing) ci-dessus.
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
        private static string _version;

        /// <summary>
        /// Version of libnfc
        /// </summary>
        public static string Version
        {
            get
            {
                if (_version == null)
                {
                    IntPtr ptr = NativeMethods.version();
                    _version = Marshal.PtrToStringAnsi(ptr);
                }
                return _version;
            }
        }

        /// <summary>
        /// Open a devive by its connectionstring, on null for default device
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public NfcDevice Open(string connectionString)
        {
            IntPtr ptr = NativeMethods.open(this.m_ctx, connectionString);
            if (ptr == IntPtr.Zero)
                return null;
            return new NfcDevice(ptr);
        }

        /// <summary>
        /// List devices
        /// </summary>
        /// <returns>Array of strings, the connection strings of the devices</returns>
        public string[] ListDevices()
        {
            const int MAX_DEVICE = 8;
            const int LENGTH = MAX_DEVICE * NativeMethods.NFC_BUFSIZE_CONNSTRING;

            IntPtr ptr = Marshal.AllocHGlobal(LENGTH);
            try
            {
                for (int i = 0; i < LENGTH; i++)
                {
                    Marshal.WriteByte(ptr, i, 0);
                }

                int size = NativeMethods.list_devices(this.m_ctx, ptr, MAX_DEVICE);
                string[] arr = new string[size];
                for (int i = 0; i < size; i++)
                {
                    arr[i] = Marshal.PtrToStringAnsi(ptr + i * NativeMethods.NFC_BUFSIZE_CONNSTRING);
                }
                return arr;
            }
            finally
            {
                if (ptr != IntPtr.Zero)
                    Marshal.FreeHGlobal(ptr);
            }
        }
    }
}
