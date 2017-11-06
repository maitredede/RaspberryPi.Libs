using RaspberryPi.LibNFC.Interop;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace RaspberryPi.LibNFC
{
    public sealed class NfcDevice : NativeObject, INfcInitiator, INfcEmulatedTag
    {
        internal NfcDevice(IntPtr ptr) : base(ptr, true)
        {
        }

        protected override void FreeHandle()
        {
            NativeMethods.close(this.handle);
        }

        public INfcInitiator InitInitiator(bool secure)
        {
            NfcError ret;
            if (secure)
                ret = NativeMethods.initiator_init_secure_element(this.handle);
            else
                ret = NativeMethods.initiator_init(this.handle);

            NfcException.Raise(ret);
            return this;
        }

        //public INfcEmulatedTag InitEmulatedTag(Interop.NfcTarget target)
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
                    IntPtr ptr = NativeMethods.device_get_name(this.handle);
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
                    IntPtr ptr = NativeMethods.device_get_connstring(this.handle);
                    this._connstring = Marshal.PtrToStringAnsi(ptr);
                }
                return this._connstring;
            }
        }

        public string GetInfo()
        {
            int length = 4096;
            string buff = new string((char)0, length);
            NfcError code = NativeMethods.device_get_information_about(this.handle, ref buff);
            if (code != NfcError.Success)
            {
                throw new NfcException("Can't get device info", code);
            }
            return buff;
        }

        public void SetProperty(NfcProperty property, int value)
        {
            NfcError err = NativeMethods.device_set_property_int(this.handle, property, value);
            NfcException.Raise(err);
        }

        public void SetProperty(NfcProperty property, bool value)
        {
            NfcError err = NativeMethods.device_set_property_bool(this.handle, property, value);
            NfcException.Raise(err);
        }

        public NfcError LastError { get { return NativeMethods.device_get_last_error(this.handle); } }

        //NfcTarget INfcInitiator.Poll(NfcModulation[] modulations, byte pollNr, byte period)
        //{
        //    this.SetProperty(NfcProperty.NP_ACTIVATE_FIELD, false);
        //    this.SetProperty(NfcProperty.NP_HANDLE_CRC, true);
        //    this.SetProperty(NfcProperty.NP_HANDLE_PARITY, true);
        //    this.SetProperty(NfcProperty.NP_AUTO_ISO14443_4, true);
        //    this.SetProperty(NfcProperty.NP_ACTIVATE_FIELD, true);

        //    NfcTarget[] targets = new NfcTarget[10];
        //    GCHandle h = GCHandle.Alloc(targets, GCHandleType.Pinned);
        //    try
        //    {
        //        int count = NativeMethods.initiator_list_passive_targets(this.m_ptr, modulations[0], h.AddrOfPinnedObject(), targets.Length);
        //        if (count < 0)
        //            NfcException.Raise((NfcError)count);
        //        if (count == 0)
        //            return null;
        //    }
        //    finally
        //    {
        //        h.Free();
        //    }
        //}

        NfcTarget INfcInitiator.Poll(NfcModulation[] modulations, byte pollNr, byte period)
        {
            IntPtr nfcTarget = IntPtr.Zero;
            int count;

            GCHandle gc = GCHandle.Alloc(modulations, GCHandleType.Pinned);
            try
            {
                int size = Marshal.SizeOf<Interop.nfc_target>();
                nfcTarget = Marshal.AllocHGlobal(size);
                Console.WriteLine("Allocated nfcTarget memory. size={0} ptr={1}", size, nfcTarget);
                count = NativeMethods.initiator_poll_target(this.handle, modulations, (uint)modulations.Length, pollNr, period, nfcTarget);
                Console.WriteLine($"episage nfc returned {count}");
            }
            finally
            {
                Marshal.FreeHGlobal(nfcTarget);
                gc.Free();
            }

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

        private static readonly int MAX_CANDIDATES = 16;
        NfcTargetList INfcInitiator.ListPassiveTargets(NfcModulation modulation)
        {
            int size = Marshal.SizeOf<nfc_target>() * MAX_CANDIDATES;
            IntPtr ptrData = Marshal.AllocHGlobal(size);
            int count = NativeMethods.initiator_list_passive_targets(this.handle, modulation, ptrData, MAX_CANDIDATES);
            if (count < 0)
            {
                Marshal.FreeHGlobal(ptrData);
                NfcException.Raise((NfcError)count);
                return null;
            }
            if (count == 0)
            {
                Marshal.FreeHGlobal(ptrData);
                return new NfcTargetList(IntPtr.Zero, 0, this);
            }
            return new NfcTargetList(ptrData, count, this);
        }

        NfcSelectedTarget INfcInitiator.Select(NfcModulation modulation)
        {
            IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf<nfc_target>());
            int code = NativeMethods.initiator_select_passive_target(this.handle, modulation, IntPtr.Zero, 0, ptr);
            if (code < 0)
            {
                Marshal.FreeHGlobal(ptr);
                NfcException.Raise((NfcError)code);
            }
            if (code == 0)
            {
                Marshal.FreeHGlobal(ptr);
                return null;
            }
            return new NfcSelectedTarget(ptr, this);
        }
    }
}
