using System;
using System.Collections.Generic;
using System.Text;

namespace RaspberryPi.LibNFC
{
    public enum NfcError
    {
        /// <summary>
        /// Success (no error)
        /// </summary>
        Success = 0,
        /// <summary>
        /// Input / output error, device may not be usable anymore without re-open it
        /// </summary>
        EIO = -1,
        /// <summary>
        /// Invalid argument(s)
        /// </summary>
        EINVARG = -2,
        /// <summary>
        /// Operation not supported by device
        /// </summary>
        EDEVNOTSUPP = -3,
        /// <summary>
        /// No such device
        /// </summary>
        NFC_ENOTSUCHDEV = -4,
        /// <summary>
        /// Buffer overflow
        /// </summary>
        NFC_EOVFLOW = -5,
        /// <summary>
        /// Operation timed out
        /// </summary>
        NFC_ETIMEOUT = -6,
        /// <summary>
        /// Operation aborted (by user)
        /// </summary>
        NFC_EOPABORTED = -7,
        /// <summary>
        /// Not (yet) implemented
        /// </summary>
        NFC_ENOTIMPL = -8,
        /// <summary>
        /// Target released
        /// </summary>
        NFC_ETGRELEASED = -10,
        /// <summary>
        /// Error while RF transmission
        /// </summary>
        NFC_ERFTRANS = -20,
        /// <summary>
        /// MIFARE Classic: authentication failed
        /// </summary>
        NFC_EMFCAUTHFAIL = -30,
        /// <summary>
        /// Software error (allocation, file/pipe creation, etc.)
        /// </summary>
        NFC_ESOFT = -80,
        /// <summary>
        /// Device's internal chip error
        /// </summary>
        NFC_ECHIP = -90
    }
}
