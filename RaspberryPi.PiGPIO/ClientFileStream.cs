using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RaspberryPi.PiGPIO
{
    internal sealed class ClientFileStream : Stream
    {
        private readonly IPiGPIO m_pigs;
        private int m_handle;
        private long m_position;

        public override bool CanRead { get; }
        public override bool CanSeek { get; }
        public override bool CanWrite { get; }
        public override long Length => throw new NotSupportedException();

        internal ClientFileStream(IPiGPIO client, int handle, bool canRead, bool canSeek, bool canWrite)
        {
            this.m_pigs = client;
            this.m_handle = handle;
            this.CanRead = canRead;
            this.CanSeek = canSeek;
            this.CanWrite = canWrite;
        }

        protected override void Dispose(bool disposing)
        {
            if (this.m_handle > -1)
            {
                this.m_pigs.FileClose(this.m_handle);
                this.m_handle = -1;
            }
            base.Dispose(disposing);
        }

        public override long Position
        {
            get => this.m_position;
            set => this.Seek(value, SeekOrigin.Begin);
        }

        public override void Flush()
        {
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            byte[] data = this.m_pigs.FileRead(this.m_handle, count);
            Array.Copy(data, 0, buffer, offset, data.Length);
            this.m_position += data.Length;
            return data.Length;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            this.m_position = this.m_pigs.FileSeek(this.m_handle, (int)offset, origin);
            return this.m_position;
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            byte[] data = new byte[count];
            Array.Copy(buffer, offset, data, 0, count);
            this.m_pigs.FileWrite(this.m_handle, data);
            this.m_position += data.Length;
        }
    }
}
