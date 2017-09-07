using System;
using System.Collections.Generic;
using System.Text;

namespace RaspberryPi.MMAL
{
    [System.Diagnostics.DebuggerDisplay("{Value}")]
    public sealed class MMALComponentName : IComparable<MMALComponentName>, IEquatable<MMALComponentName>
    {
        public static readonly MMALComponentName VideoDecoder = new MMALComponentName("vc.ril.video_decode");
        public static readonly MMALComponentName Camera = new MMALComponentName("vc.ril.camera");
        public static readonly MMALComponentName CameraInfo = new MMALComponentName("vc.camera_info");

        private readonly string m_value;
        public string Value { get { return this.m_value; } }

        internal MMALComponentName(string value)
        {
            this.m_value = value;
        }

        int IComparable<MMALComponentName>.CompareTo(MMALComponentName other)
        {
            return string.Compare(this.Value, other.Value, StringComparison.Ordinal);
        }

        bool IEquatable<MMALComponentName>.Equals(MMALComponentName other)
        {
            return this.Value.Equals(other.Value);
        }

        public override bool Equals(object obj)
        {
            if (obj is MMALComponentName mmalName)
            {
                return ((IEquatable<MMALComponentName>)this).Equals(mmalName);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }
    }
}
