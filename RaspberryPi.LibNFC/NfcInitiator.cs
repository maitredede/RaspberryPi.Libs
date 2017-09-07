using RaspberryPi.LibNFC.Interop;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RaspberryPi.LibNFC
{
    public sealed class NfcInitiator
    {
        private readonly NfcDevice m_device;

        internal NfcInitiator(NfcDevice device)
        {
            this.m_device = device;
        }

        public NfcInitiatorTarget Poll(NfcModulation[] modulations, byte pollNr, byte period)
        {
            IntPtr nfcTarget;
            int count = NativeMethods.initiator_poll_target(this.m_device.Handle, modulations, modulations.Length, pollNr, period, out nfcTarget);
            if (count < 0)
                NfcException.Raise((NfcError)count);
            if (count == 0)
                return null;
            NfcInitiatorTarget target = new NfcInitiatorTarget(this, nfcTarget);
            return target;
        }
    }
}
