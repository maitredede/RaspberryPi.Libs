using RaspberryPi.EscPos.PrintConnectors;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace RaspberryPi.EscPos
{
    public sealed class EscPosPrinter : IDisposable, IRawPrinter
    {
        private readonly bool m_ownsConnector;
        private readonly IPrintConnector m_connector;
        private readonly SemaphoreSlim m_ioLock;

        public EscPosPrinter(IPrintConnector connector, bool ownsConnector)
        {
            this.m_connector = connector ?? throw new ArgumentNullException(nameof(connector));
            this.m_ownsConnector = ownsConnector;
            this.m_ioLock = new SemaphoreSlim(1);
        }

        #region IDisposable Support
        private bool disposedValue = false; // Pour détecter les appels redondants

        void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    this.m_ioLock.Dispose();
                    if (this.m_ownsConnector)
                        this.m_connector.Dispose();
                }

                // TODO: libérer les ressources non managées (objets non managés) et remplacer un finaliseur ci-dessous.
                // TODO: définir les champs de grande taille avec la valeur Null.

                disposedValue = true;
            }
        }

        // TODO: remplacer un finaliseur seulement si la fonction Dispose(bool disposing) ci-dessus a du code pour libérer les ressources non managées.
        // ~EscPosPrinter() {
        //   // Ne modifiez pas ce code. Placez le code de nettoyage dans Dispose(bool disposing) ci-dessus.
        //   Dispose(false);
        // }

        // Ce code est ajouté pour implémenter correctement le modèle supprimable.
        public void Dispose()
        {
            // Ne modifiez pas ce code. Placez le code de nettoyage dans Dispose(bool disposing) ci-dessus.
            Dispose(true);
            // TODO: supprimer les marques de commentaire pour la ligne suivante si le finaliseur est remplacé ci-dessus.
            // GC.SuppressFinalize(this);
        }
        #endregion

        private void Write(params byte[] data)
        {
            this.m_connector.Write(data, 0, data.Length);
        }

        private byte ReadByte()
        {
            byte[] buffer = new byte[1];
            this.m_connector.Read(buffer, 0, buffer.Length);
            return buffer[0];
        }

        private IDisposable Lock()
        {
            this.m_ioLock.Wait();
            return new ScopeLock(this.m_ioLock);
        }

        void IRawPrinter.HT()
        {
            using (Lock())
                this.Write(0x09);
        }

        void IRawPrinter.LF()
        {
            using (Lock())
                this.Write(0x0A);
        }

        void IRawPrinter.FF()
        {
            using (Lock())
                this.Write(0x0C);
        }

        void IRawPrinter.CR()
        {
            using (Lock())
                this.Write(0x0D);
        }

        void IRawPrinter.CAN()
        {
            using (Lock())
                this.Write(0x18);
        }

        byte IRawPrinter.DLE_EO(byte n)
        {
            using (Lock())
            {
                this.Write(0x10, 0x04, n);
                return this.ReadByte();
            }
        }

        void IRawPrinter.DLE_ENQ(byte n)
        {
            using (Lock())
            {
                this.Write(0x10, 0x05, n);
            }
        }
    }
}
