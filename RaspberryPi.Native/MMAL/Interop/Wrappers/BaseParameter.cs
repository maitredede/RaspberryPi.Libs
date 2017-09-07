using RaspberryPi.Interop;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace RaspberryPi.MMAL.Interop.Wrappers
{
    internal abstract class BaseParameter<T> : IDisposable
    {
        private readonly InteropHandler<T> m_handler;
        private readonly IntPtr m_ptr;

        internal IntPtr Ptr => this.m_ptr;
        internal InteropHandler<T> Handler => this.m_handler;

        private readonly int m_size;

        public BaseParameter()
        {
            this.m_size = Marshal.SizeOf<T>();
            this.m_ptr = Marshal.AllocHGlobal(this.m_size);
            this.m_handler = new InteropHandler<T>(() => this.m_ptr);

            this.hdr.size = (uint)this.m_size;
        }

        #region IDisposable Support
        private bool disposedValue = false; // Pour détecter les appels redondants

        void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                }

                Marshal.FreeHGlobal(this.m_ptr);

                disposedValue = true;
            }
        }

        ~BaseParameter()
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

        private MMAL_PARAMETER_HEADER_T_Wrapper m_hdr;
        public MMAL_PARAMETER_HEADER_T_Wrapper hdr
        {
            get
            {
                if (this.m_hdr == null)
                {
                    this.m_hdr = new MMAL_PARAMETER_HEADER_T_Wrapper(() => this.m_ptr + this.m_handler.GetOffset(nameof(this.hdr)));
                }
                return this.m_hdr;
            }
        }
    }
}
