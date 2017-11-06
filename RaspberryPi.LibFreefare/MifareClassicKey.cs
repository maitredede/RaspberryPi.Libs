using System;
using System.Collections.Generic;
using System.Text;

namespace RaspberryPi.LibFreefare
{
    public sealed class MifareClassicKey
    {
        public static readonly int SIZE = 6;
        private static readonly byte[] FORUM_CLASSIC_KEY_A_VALUE = { 0xd3, 0xf7, 0xd3, 0xf7, 0xd3, 0xf7 };

        public static readonly MifareClassicKey FORUM_CLASSIC_KEY_A = new MifareClassicKey(FORUM_CLASSIC_KEY_A_VALUE);

        private byte[] m_value;

        public bool IsValid()
        {
            return this.m_value != null && this.m_value.Length == SIZE;
        }

        public MifareClassicKey(byte[] value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            if (value.Length != SIZE)
                throw new ArgumentOutOfRangeException(nameof(value), "Length must be " + SIZE);
            this.m_value = value;
        }

        public byte[] Value => this.m_value;
    }
}
