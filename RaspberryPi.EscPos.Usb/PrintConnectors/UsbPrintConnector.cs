using System;
using System.Collections.Generic;
using System.Text;
using LibUsbDotNet;
using LibUsbDotNet.Main;

namespace RaspberryPi.EscPos.PrintConnectors
{
    public sealed class UsbPrintConnector : IPrintConnector
    {
        public static UsbPrintConnector Open(int vendorId, int productId, string profile = null)
        {
            UsbDeviceFinder finder = new UsbDeviceFinder(vendorId, productId);
            UsbDevice device = UsbDevice.OpenUsbDevice(finder);
            if (device == null)
                return null;

            return new UsbPrintConnector(device);
        }

        private readonly UsbDevice m_usb;
        private readonly IUsbDevice m_wholeUsbDevice;
        private readonly UsbEndpointReader m_reader;
        private readonly UsbEndpointWriter m_writer;

        private UsbPrintConnector(UsbDevice usb)
        {
            this.m_usb = usb;
            this.m_wholeUsbDevice = this.m_usb as IUsbDevice;
            if (this.m_wholeUsbDevice != null)
            {
                // This is a "whole" USB device. Before it can be used,
                // the desired configuration and interface must be selected.

                // Select config #1
                this.m_wholeUsbDevice.SetConfiguration(1);

                // Claim interface #0.
                this.m_wholeUsbDevice.ClaimInterface(0);
            }

            // open read endpoint 1.
            this.m_reader = this.m_usb.OpenEndpointReader(ReadEndpointID.Ep01);

            // open write endpoint 1.
            this.m_writer = this.m_usb.OpenEndpointWriter(WriteEndpointID.Ep01);
        }

        public int Read(byte[] buffer, int offset, int count)
        {
            ErrorCode error = this.m_reader.Read(buffer, offset, count, 5000, out int read);
            if (error != ErrorCode.Success)
            {
                throw new UsbConnectorException("Error reading from usb: " + error);
            }
            return read;
        }

        public void Write(byte[] buffer, int offset, int count)
        {
            ErrorCode error = this.m_writer.Write(buffer, offset, count, 5000, out int transferLength);
            if (error != ErrorCode.Success)
            {
                throw new UsbConnectorException("Error writing to usb: " + error);
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
                    this.m_wholeUsbDevice?.ReleaseInterface(0);
                    this.m_usb.Close();
                }

                // TODO: libérer les ressources non managées (objets non managés) et remplacer un finaliseur ci-dessous.
                // TODO: définir les champs de grande taille avec la valeur Null.

                disposedValue = true;
            }
        }

        // TODO: remplacer un finaliseur seulement si la fonction Dispose(bool disposing) ci-dessus a du code pour libérer les ressources non managées.
        // ~UsbPrintConnector() {
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
    }
}
