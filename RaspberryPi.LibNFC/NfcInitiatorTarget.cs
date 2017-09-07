using System;
using System.Collections.Generic;
using System.Text;

namespace RaspberryPi.LibNFC
{
    public sealed class NfcInitiatorTarget
    {
        private readonly NfcInitiator m_initiator;
        private readonly IntPtr m_ptr;

        internal NfcInitiatorTarget(NfcInitiator initiator, IntPtr ptr)
        {
            this.m_initiator = initiator;
            this.m_ptr = ptr;
        }
    }
}
