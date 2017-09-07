using System;
using System.Collections.Generic;
using System.Text;

namespace RaspberryPi.LibNFC
{
    public sealed class NfcEmulatedTag
    {
        private readonly NfcDevice m_device;

        internal NfcEmulatedTag(NfcDevice device)
        {
            this.m_device = device;
        }
    }
}
