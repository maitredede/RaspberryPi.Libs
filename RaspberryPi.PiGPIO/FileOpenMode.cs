using System;
using System.Collections.Generic;
using System.Text;

namespace PiGPIO
{
    /// <summary>
    /// File open mode
    /// </summary>
    [Flags]
    public enum FileOpenMode
    {
        /// <summary>
        /// open file for reading
        /// </summary>
        Read = 1,
        /// <summary>
        /// open file for writing
        /// </summary>
        Write = 2,
        /// <summary>
        /// open file for reading and writing
        /// </summary>
        ReadWrite = Read | Write,
        /// <summary>
        /// All writes append data to the end of the file
        /// </summary>
        Append = 4,
        /// <summary>
        /// The file is created if it doesn't exist
        /// </summary>
        Create = 8,
        /// <summary>
        /// The file is truncated
        /// </summary>
        Trunc = 16,
    }
}
