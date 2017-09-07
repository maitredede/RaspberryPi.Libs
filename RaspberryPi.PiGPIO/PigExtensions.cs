using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PiGPIO
{
    /// <summary>
    /// Extension methods
    /// </summary>
    public static class PigExtensions
    {
        public static int FileOpen(this IPiGPIO client, string file, FileOpenMode mode)
        {
            int iMode = (int)mode;
            if ((iMode & (int)FileOpenMode.ReadWrite) == 0)
            {
                throw new InvalidOperationException("Must specify read and/or write");
            }
            return client.FileOpen(file, iMode);
        }

        public static Stream OpenFileStream(this IPiGPIO client, string file, FileOpenMode mode)
        {
            int handle = client.FileOpen(file, mode);
            if (handle < 0)
            {
                throw new PiGPIOException(handle);
            }
            return new ClientFileStream(client, handle, mode.HasFlag(FileOpenMode.Read), true, mode.HasFlag(FileOpenMode.Write));
        }

        public static int FileSeek(this IPiGPIO client, int handle, int offset, SeekOrigin from)
        {
            return client.FileSeek(handle, offset, (int)from);
        }

        public static string ToBinaryString(this int value)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 31; i >= 0; i--)
            {
                uint mask = 1u << i;
                if ((value & mask) == 0)
                    sb.Append("0");
                else
                    sb.Append("1");
                if (i % 4 == 0)
                {
                    sb.Append(" ");
                }
            }
            return sb.ToString();
        }

        public static string ToBinaryString(this short value)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 15; i >= 0; i--)
            {
                uint mask = 1u << i;
                if ((value & mask) == 0)
                    sb.Append("0");
                else
                    sb.Append("1");
                if (i % 4 == 0)
                {
                    sb.Append(" ");
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Get the board name
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public static string HardwareName(this IPiGPIO client)
        {
            int hwver = client.HardwareRevision();
            // http://elinux.org/RPi_HardwareHistory

            if ((hwver >= 2 && hwver <= 6) || (hwver >= 0x000D && hwver <= 0x000F))
            {
                return "Raspberry pi 1B";
            }
            if (hwver >= 7 && hwver <= 9)
            {
                return "Raspberry pi 1A";
            }
            if (hwver == 0x0010 || hwver == 0x0013)
            {
                return "Raspberry pi 1B+";
            }
            if (hwver == 0x0011 || hwver == 0x0014)
            {
                return "Compute module 1";
            }
            if (hwver == 0x0012 || hwver == 0x0015)
            {
                return "Raspberry pi 1A+";
            }

            if (hwver == 0xa01040 || hwver == 0xa01041 || hwver == 0xa21041 || hwver == 0xa22042)
            {
                return "Raspberry pi 2B";
            }
            if (hwver == 0x900021)
            {
                return "Raspberry pi 1A+";
            }
            if (hwver == 0x900032)
            {
                return "Raspberry pi 1B+";
            }
            if (hwver == 0x900092 || hwver == 0x900093)
            {
                return "Raspberry pi Zero";
            }
            if (hwver == 0x9000c1)
            {
                return "Raspberry pi Zero W";
            }
            if (hwver == 0xa02082 || hwver == 0xa22082 || hwver == 0xa32082)
            {
                return "Raspberry pi 3B";
            }
            if (hwver == 0xa020a0)
            {
                return "Compute module 3";
            }
            return null;
        }

        public static SPIHelper OpenSpi(this IPiGPIO client, int channel, int bauds, int flags)
        {
            int handle = client.SpiOpen(channel, bauds, flags);
            return new SPIHelper(client, handle);
        }

        public static void SpiWrite(this IPiGPIO client, int handle, byte[] buffer, int offset, int count)
        {
            byte[] data = new byte[count];
            Array.Copy(buffer, offset, data, 0, count);
            client.SpiWrite(handle, data);
        }

        public static WaveformBuilder BuildWaveform(this IPiGPIO client)
        {
            return new WaveformBuilder(client);
        }
    }
}
