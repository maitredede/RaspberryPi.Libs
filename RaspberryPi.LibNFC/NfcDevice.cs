using RaspberryPi.LibNFC.Interop;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace RaspberryPi.LibNFC
{
    public sealed class NfcDevice : IDisposable, INfcInitiator
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

        public INfcInitiator InitInitiator(bool secure)
        {
            NfcError ret;
            if (secure)
                ret = NativeMethods.initiator_init_secure_element(this.m_ptr);
            else
                ret = NativeMethods.initiator_init(this.m_ptr);

            NfcException.Raise(ret);
            return this;
        }

        //public NfcEmulatedTag InitEmulatedTag(Interop.NfcTarget target)
        //{
        //    //int rxLength;
        //    //IntPtr rx;
        //    //int msTimeout;
        //    //NfcError ret = NativeMethods.target_init(this.m_ptr, target, rx, rxLength, msTimeout);
        //    //NfcException.Raise(ret);
        //    //return new NfcEmulatedTag(rx, rxLength);
        //    throw new NotImplementedException();
        //}

        private string _name;
        private string _connstring;

        public string Name
        {
            get
            {
                if (this._name == null)
                {
                    IntPtr ptr = NativeMethods.device_get_name(this.m_ptr);
                    this._name = Marshal.PtrToStringAnsi(ptr);
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
                    IntPtr ptr = NativeMethods.device_get_connstring(this.m_ptr);
                    this._connstring = Marshal.PtrToStringAnsi(ptr);
                }
                return this._connstring;
            }
        }

        public string GetInfo()
        {
            int length = 4096;
            string buff = new string((char)0, length);
            NfcError code = NativeMethods.device_get_information_about(this.m_ptr, ref buff);
            if (code != NfcError.Success)
            {
                throw new NfcException("Can't get device info", code);
            }
            return buff;
        }

        public void SetProperty(NfcProperty property, int value)
        {
            NfcError err = NativeMethods.device_set_property_int(this.m_ptr, property, value);
            NfcException.Raise(err);
        }

        public void SetProperty(NfcProperty property, bool value)
        {
            NfcError err = NativeMethods.device_set_property_bool(this.m_ptr, property, value);
            NfcException.Raise(err);
        }


        public NfcError LastError { get { return NativeMethods.device_get_last_error(this.m_ptr); } }

        NfcTarget INfcInitiator.Poll(NfcModulation[] modulations, byte pollNr, byte period)
        {
            IntPtr nfcTarget = IntPtr.Zero;
            int count;

            //HACK
            modulations = new NfcModulation[] { modulations[0] };

            GCHandle gc = GCHandle.Alloc(modulations, GCHandleType.Pinned);
            try
            {
                //int size = Marshal.SizeOf<nfc_target>();
                int size = 283;
                nfcTarget = Marshal.AllocHGlobal(size);
                Console.WriteLine("Allocated nfcTarget memory. size={0} ptr={1}", size, nfcTarget);
                count = NativeMethods.initiator_poll_target(this.m_ptr, modulations, (uint)modulations.Length, pollNr, period, nfcTarget);
                Console.WriteLine($"episage nfc returned {count}");
            }
            finally
            {
                Marshal.FreeHGlobal(nfcTarget);
                gc.Free();
            }
            // count = NativeMethods.initiator_poll_target(this.m_device.Handle, modulations, modulations.Length, pollNr, period, out nfcTarget);

            if (count < 0)
            {
                Console.WriteLine("LastError={0}", this.LastError);
                NfcException.Raise((NfcError)count);
            }
            if (count == 0)
                return null;
            NfcTarget target = new NfcTarget(this, nfcTarget);
            return target;
        }
    }
}
