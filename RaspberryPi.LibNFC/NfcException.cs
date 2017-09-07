using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RaspberryPi.LibNFC
{
    public sealed class NfcException : Exception
    {
        private readonly NfcError m_error;

        public NfcError Error => this.m_error;

        internal NfcException(string message, NfcError error) : base(message)
        {
            this.m_error = error;
        }

        internal static void Raise(NfcError error)
        {
            switch (error)
            {
                case NfcError.Success: return;
                case NfcError.EIO: throw new NfcException("Input / output error, device may not be usable anymore without re-open it", error);
                case NfcError.EINVARG: throw new NfcException("Invalid argument(s)", error);
                case NfcError.EDEVNOTSUPP: throw new NfcException("Operation not supported by device", error);
                case NfcError.NFC_ENOTSUCHDEV: throw new NfcException("No such device", error);
                case NfcError.NFC_EOVFLOW: throw new NfcException("Buffer overflow", error);
                case NfcError.NFC_ETIMEOUT: throw new NfcException("Operation timed out", error);
                case NfcError.NFC_EOPABORTED: throw new NfcException("Operation aborted (by user)", error);
                case NfcError.NFC_ENOTIMPL: throw new NfcException("Not (yet) implemented", error);
                case NfcError.NFC_ETGRELEASED: throw new NfcException("Target released", error);
                case NfcError.NFC_ERFTRANS: throw new NfcException("Error while RF transmission", error);
                case NfcError.NFC_EMFCAUTHFAIL: throw new NfcException("MIFARE Classic: authentication failed", error);
                case NfcError.NFC_ESOFT: throw new NfcException("Software error (allocation, file/pipe creation, etc.)", error);
                case NfcError.NFC_ECHIP: throw new NfcException("Device's internal chip error", error);
                default:
                    throw new NotImplementedException("Unknown libnfc error " + error);
            }
        }
    }
}
