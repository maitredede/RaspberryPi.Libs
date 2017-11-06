using RaspberryPi.LibNFC.Interop;
using System;
using System.Collections.Generic;
using System.Text;

namespace RaspberryPi.LibNFC.Mifare
{
    public abstract class MifareTag : IDisposable
    {
        private readonly NfcSelectedTarget m_target;
        private nfc_iso14443a_info? m_info;
        private string m_uid;

        internal MifareTag(NfcSelectedTarget target)
        {
            this.m_target = target;
        }

        private void EnsureInfo()
        {
            if (!this.m_info.HasValue)
                this.m_info = this.m_target.Target.ReadInfo<nfc_iso14443a_info>();
        }

        public string UID
        {
            get
            {
                if (this.m_uid == null)
                {
                    this.EnsureInfo();

                    StringBuilder sb = new StringBuilder();
                    if (this.m_info.Value.abtUid[0] == 0x08)
                    {
                        sb.Append("3");
                    }
                    else
                    {
                        sb.Append("1");
                    }
                    for (int i = 0; i < this.m_info.Value.szUidLen; i++)
                    {
                        sb.AppendFormat("{0:X2}", this.m_info.Value.abtUid[i]);
                    }
                    this.m_uid = sb.ToString();
                }
                return this.m_uid;
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // Pour détecter les appels redondants

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    this.m_target.Dispose();
                }

                disposedValue = true;
            }
        }

        // TODO: remplacer un finaliseur seulement si la fonction Dispose(bool disposing) ci-dessus a du code pour libérer les ressources non managées.
        // ~MifareTag() {
        //   // Ne modifiez pas ce code. Placez le code de nettoyage dans Dispose(bool disposing) ci-dessus.
        //   Dispose(false);
        // }

        // Ce code est ajouté pour implémenter correctement le modèle supprimable.
        void IDisposable.Dispose()
        {
            // Ne modifiez pas ce code. Placez le code de nettoyage dans Dispose(bool disposing) ci-dessus.
            Dispose(true);
            // TODO: supprimer les marques de commentaire pour la ligne suivante si le finaliseur est remplacé ci-dessus.
            // GC.SuppressFinalize(this);
        }
        #endregion

        public bool IsPresent() => this.m_target.IsPresent();
    }
}
