using System;
using System.Collections.Generic;
using System.Text;

namespace RaspberryPi.EscPos
{
    public interface IRawPrinter
    {
        /// <summary>
        /// Moves print position to next horizontal tab position
        /// </summary>
        void HT();
        /// <summary>
        /// Prints the data in the print buffer and performs a line feed based on the set line feed amount
        /// </summary>
        void LF();
        /// <summary>
        /// Prints all buffered data to the print region collectively, then recovers to the standard mode.
        /// </summary>
        void FF();
        /// <summary>
        /// When an automatic line feed is enabled, this command functions in the same way as LF (print and line feed). When the automatic line feed is disabled, this command is ignored
        /// </summary>
        void CR();
        /// <summary>
        /// Deletes all print data in the currently set print region in page mode.
        /// </summary>
        void CAN();
        /// <summary>
        /// Transmits the status specified by n in real-time.
        /// </summary>
        /// <param name="n">
        /// n = 1: Transmit printer status
        /// n = 2: Transmit offline cause status
        /// n = 3: Transmit error cause status
        /// n = 4: Transmit continuous paper detector status
        /// n = 5: Transmit presenter paper detector status
        /// </param>
        byte DLE_EO(byte n);
        /// <summary>
        /// Responds to requests n specifications from the host in real-time.
        /// </summary>
        /// <param name="n">
        /// n = 1: Recover from the error and start printing from the line where the error occurred.
        /// n = 2: Recover from error after clearing the reception buffer and print buffer.
        /// </param>
        void DLE_ENQ(byte n);
    }
}
