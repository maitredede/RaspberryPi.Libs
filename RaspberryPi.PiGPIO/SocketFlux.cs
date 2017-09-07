using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PiGPIO
{
    internal sealed class SocketFlux : IDisposable
    {
        private readonly SemaphoreSlim m_cmdlock;
        private readonly Socket m_sck;
        private NetworkStream m_ns;
        private BinaryReader m_br;
        private BinaryWriter m_bw;

        internal SocketFlux(int receiveTimeout, int sendTimeout = 5000)
        {
            this.m_sck = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            this.m_sck.SendTimeout = sendTimeout;
            this.m_sck.ReceiveTimeout = receiveTimeout;
            this.m_sck.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.NoDelay, true);
            try
            {
                this.m_sck.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);

                const int bytesperlong = 4; // 32 / 8
                const int bitsperbyte = 8;
                ulong time = 10000;
                ulong interval = 1000;

                // resulting structure
                byte[] SIO_KEEPALIVE_VALS = new byte[3 * bytesperlong];

                // array to hold input values
                ulong[] input = new ulong[3];

                // put input arguments in input array
                if (time == 0 || interval == 0) // enable disable keep-alive
                    input[0] = (0UL); // off
                else
                    input[0] = (1UL); // on

                input[1] = (time); // time millis
                input[2] = (interval); // interval millis

                // pack input into byte struct
                for (int i = 0; i < input.Length; i++)
                {
                    SIO_KEEPALIVE_VALS[i * bytesperlong + 3] = (byte)(input[i] >> ((bytesperlong - 1) * bitsperbyte) & 0xff);
                    SIO_KEEPALIVE_VALS[i * bytesperlong + 2] = (byte)(input[i] >> ((bytesperlong - 2) * bitsperbyte) & 0xff);
                    SIO_KEEPALIVE_VALS[i * bytesperlong + 1] = (byte)(input[i] >> ((bytesperlong - 3) * bitsperbyte) & 0xff);
                    SIO_KEEPALIVE_VALS[i * bytesperlong + 0] = (byte)(input[i] >> ((bytesperlong - 4) * bitsperbyte) & 0xff);
                }
                // create bytestruct for result (bytes pending on server socket)
                byte[] result = BitConverter.GetBytes(0);
                this.m_sck.IOControl(IOControlCode.KeepAliveValues, SIO_KEEPALIVE_VALS, result);
            }
            catch(PlatformNotSupportedException)
            {
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error setting socket keepalive settings: {ex.GetType().FullName}: {ex.Message}");
            }
            this.m_cmdlock = new SemaphoreSlim(1, 1);
        }

        public bool IsConnected => this.m_sck.Connected;
        public EndPoint EndPoint => this.m_sck.RemoteEndPoint;

        public void Dispose()
        {
            this.m_bw?.Dispose();
            this.m_br?.Dispose();
            this.m_ns?.Dispose();
            ((IDisposable)this.m_sck).Dispose();
            this.m_cmdlock.Dispose();
        }

        internal async Task Connect(string host, int port)
        {
            await this.m_sck.ConnectAsync(host, port);
            this.m_ns = new NetworkStream(this.m_sck, false);
            this.m_br = new BinaryReader(this.m_ns, Encoding.UTF8, true);
            this.m_bw = new BinaryWriter(this.m_ns, Encoding.UTF8, true);
        }

        internal void RunCommand(Commands command, uint p1, uint p2, int p3, byte[] extension, out int res)
        {
            this.m_cmdlock.Wait();
            try
            {
                this.m_bw.Write((uint)command);
                this.m_bw.Write(p1);
                this.m_bw.Write(p2);
                this.m_bw.Write(p3);
                if (extension != null)
                    this.m_bw.Write(extension);
                this.m_bw.Flush();

                uint cmdRet = this.m_br.ReadUInt32();
                uint p1Ret = this.m_br.ReadUInt32();
                uint p2Ret = this.m_br.ReadUInt32();
                res = this.m_br.ReadInt32();
                if (res < 0)
                {
                    throw new PiGPIOException(res);
                }
            }
            finally
            {
                this.m_cmdlock.Release();
            }
        }

        internal void RunCommand(Commands command, uint p1, uint p2, int p3, byte[] extension, out uint res)
        {
            this.m_cmdlock.Wait();
            try
            {
                this.m_bw.Write((uint)command);
                this.m_bw.Write(p1);
                this.m_bw.Write(p2);
                this.m_bw.Write(p3);
                if (extension != null)
                    this.m_bw.Write(extension);
                this.m_bw.Flush();

                uint cmdRet = this.m_br.ReadUInt32();
                uint p1Ret = this.m_br.ReadUInt32();
                uint p2Ret = this.m_br.ReadUInt32();
                int res2 = this.m_br.ReadInt32();
                if (res2 < 0)
                {
                    throw new PiGPIOException(res2);
                }
                res = unchecked((uint)res2);
            }
            finally
            {
                this.m_cmdlock.Release();
            }
        }

        internal void RunCommand(Commands command, uint p1, uint p2, int p3, byte[] extension, out int res, out byte[] resData)
        {
            this.m_cmdlock.Wait();
            try
            {
                this.m_bw.Write((uint)command);
                this.m_bw.Write(p1);
                this.m_bw.Write(p2);
                this.m_bw.Write(p3);
                if (extension != null)
                    this.m_bw.Write(extension);
                this.m_bw.Flush();

                uint cmdRet = this.m_br.ReadUInt32();
                uint p1Ret = this.m_br.ReadUInt32();
                uint p2Ret = this.m_br.ReadUInt32();
                res = this.m_br.ReadInt32();
                if (res < 0)
                {
                    throw new PiGPIOException(res);
                }
                resData = new byte[res];
                if (res > 0)
                {
                    int totalRead = 0;
                    int read;
                    while (totalRead < res)
                    {
                        read = this.m_br.Read(resData, totalRead, res - totalRead);
                        totalRead += read;
                    }
                }
            }
            finally
            {
                this.m_cmdlock.Release();
            }
        }

        internal int Receive(byte[] buffer, int offset, int size, SocketFlags socketFlags, out SocketError errorCode)
            => this.m_sck.Receive(buffer, offset, size, socketFlags, out errorCode);
    }
}
