using System;
using System.Collections.Generic;
using System.Text;

namespace PiGPIO.Drivers.Mathertel.Radio
{
    public abstract class Radio
    {
        protected byte _volume;
        protected bool _bassBoost;
        protected bool _mono;
        protected bool _mute;
        protected bool _softMute;

        protected RadioBand _band;
        protected ushort _freq;

        protected ushort _freqLow;
        protected ushort _freqHigh;
        protected ushort _freqSteps;

        protected ReceiveRDSDelegate _sendRDS;

        public Radio()
        {
        }

        public abstract void Init();
        public abstract void Term();

        public virtual byte GetVolume() => this._volume;
        public virtual void SetVolume(byte newVolume) => this._volume = newVolume;
    }
}
