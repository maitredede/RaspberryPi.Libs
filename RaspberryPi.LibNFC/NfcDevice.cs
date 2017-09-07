using RaspberryPi.LibNFC.Interop;
using System;
using System.Collections.Generic;
using System.Text;

namespace RaspberryPi.LibNFC
{
    public sealed class NfcDevice : IDisposable
    {
        private readonly IntPtr m_ptr;

        internal IntPtr Handle => this.m_ptr;

        internal NfcDevice(IntPtr ptr)
        {
            this.m_ptr = ptr;
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

                NativeMethods.close(this.m_ptr);
                // TODO: libérer les ressources non managées (objets non managés) et remplacer un finaliseur ci-dessous.
                // TODO: définir les champs de grande taille avec la valeur Null.

                disposedValue = true;
            }
        }

        ~NfcDevice()
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

        public NfcInitiator InitInitiator(bool secure)
        {
            NfcError ret;
            if (secure)
                ret = NativeMethods.initiator_init_secure_element(this.m_ptr);
            else
                ret = NativeMethods.initiator_init(this.m_ptr);

            NfcException.Raise(ret);

            return new NfcInitiator(this);
        }

        public NfcEmulatedTag InitEmulatedTag(NfcTarget target)
        {
            //int rxLength;
            //IntPtr rx;
            //int msTimeout;
            //NfcError ret = NativeMethods.target_init(this.m_ptr, target, rx, rxLength, msTimeout);
            //NfcException.Raise(ret);
            //return new NfcEmulatedTag(rx, rxLength);
            throw new NotImplementedException();
        }

        private string _name;
        private string _connstring;

        public string Name
        {
            get
            {
                if (this._name == null)
                {
                    this._name = NativeMethods.device_get_name(this.m_ptr);
                }
                return this._name;
            }
        }

        public string ConnectionString
        {
            get
            {
                if (this._connstring == null)
                {
                    this._connstring = NativeMethods.device_get_connstring(this.m_ptr);
                }
                return this._connstring;
            }
        }
    }
}
