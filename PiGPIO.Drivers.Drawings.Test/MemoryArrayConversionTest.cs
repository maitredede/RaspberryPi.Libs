using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using Xunit;
using FluentAssertions;

namespace PiGPIO.Drivers.Drawings.Test
{
    public class MemoryArrayConversionTest
    {
        private readonly Random rnd = new Random();

        [Fact]
        public void ShortToByte()
        {
            short[] arr = new short[1000];
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = (short)rnd.Next(short.MinValue, short.MaxValue);
            }

            Stopwatch sw = Stopwatch.StartNew();

            int shortLen = sizeof(short);
            int byteLen = shortLen * arr.Length;
            byte[] data = new byte[byteLen];
            for (int i = 0; i < arr.Length; i++)
            {
                byte[] item = BitConverter.GetBytes(arr[i]);
                Array.Copy(item, 0, data, i * shortLen, shortLen);
            }
            sw.Stop();
            TimeSpan hand = sw.Elapsed;
            sw.Restart();
            byte[] data2 = new byte[byteLen];
            GCHandle h = GCHandle.Alloc(arr, GCHandleType.Pinned);
            try
            {
                Marshal.Copy(h.AddrOfPinnedObject(), data2, 0, data2.Length);
            }
            finally
            {
                h.Free();
            }
            sw.Stop();
            TimeSpan marsh = sw.Elapsed;
            string because = string.Format("Hand: {0} marshal: {1}", hand, marsh);
            //data.Should().BeSameAs(data2, because);
            data.Should().Equal(data2, (x, y) => x.Equals(y), because);
        }
    }
}
