using System;
using System.Collections.Generic;
using System.Text;

namespace PiGPIO
{
    internal struct NotificationData
    {
        public ushort Seq { get; set; }
        public ushort Flags { get; set; }
        public uint Tick { get; set; }
        public uint Level { get; set; }
        public uint Changed { get; set; }

        public NotificationData(ushort seq, ushort flags, uint tick, uint level, uint changed)
        {
            this.Seq = seq;
            this.Flags = flags;
            this.Tick = tick;
            this.Level = level;
            this.Changed = changed;
        }
    }
}
