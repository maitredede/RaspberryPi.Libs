using System;
using System.Collections.Generic;
using System.Text;

namespace RaspberryPi.PiGPIO.Drivers.Mathertel.Radio
{
    public class SI4703 : Radio
    {
        /// <summary>
        /// I2C address of Si4703
        /// </summary>
        public static readonly int SI4703_ADR = 0x10;
        /// <summary>
        /// This is the number of attempts we will try to contact the device before erroring out
        /// </summary>
        public static readonly int I2C_FAIL_MAX = 10;

        #region RegisterNames
        public static readonly int DEVICEID = 0x00;
        public static readonly int CHIPID = 0x01;
        public static readonly int POWERCFG = 0x02;
        public static readonly int CHANNEL = 0x03;
        public static readonly int SYSCONFIG1 = 0x04;
        public static readonly int SYSCONFIG2 = 0x05;
        public static readonly int SYSCONFIG3 = 0x06;
        public static readonly int STATUSRSSI = 0x0A;
        public static readonly int READCHAN = 0x0B;
        public static readonly int RDSA = 0x0C;
        public static readonly int RDSB = 0x0D;
        public static readonly int RDSC = 0x0E;
        public static readonly int RDSD = 0x0F;
        #endregion

        private readonly IPiGPIO m_pigpio;
        private readonly int m_reset;
        private readonly int m_sdio;
        private readonly ushort[] registers;
        private readonly bool m_inEurope;

        public SI4703(IPiGPIO pigpio, int gpioReset, int gpioSDIO, bool inEurope)
        {
            if (pigpio == null)
                throw new ArgumentNullException(nameof(pigpio));
            this.registers = new ushort[16];
            this.m_pigpio = pigpio;
            this.m_reset = gpioReset;
            this.m_sdio = gpioSDIO;
            this.m_inEurope = inEurope;
        }

        public override void Init()
        {
            //To get the Si4703 inito 2-wire mode, SEN needs to be high and SDIO needs to be low after a reset
            //The breakout board has SEN pulled high, but also has SDIO pulled high. Therefore, after a normal power up
            //The Si4703 will be in an unknown state. RST must be controlled

            this.m_pigpio.SetMode(this.m_reset, Mode.Output);
            this.m_pigpio.SetMode(this.m_sdio, Mode.Output); //SDIO (or SDA) is connected to A4 for I2C on arduino, GPIO2 (pin 3) on Raspbery pi
            this.m_pigpio.Write(this.m_sdio, false); //A low SDIO indicates a 2-wire interface
            this.m_pigpio.Write(this.m_reset, false); //Put Si4703 into reset
            System.Threading.Thread.Sleep(1); //Some delays while we allow pins to settle

            this.m_pigpio.Write(this.m_reset, true); //Bring Si4703 out of reset with SDIO set to low and SEN pulled high with on-board resistor
            System.Threading.Thread.Sleep(1); //Allow Si4703 to come out of reset

            throw new NotImplementedException("Wire.begin();");
        }

        public override void Term()
        {
            throw new NotImplementedException();
        }

        public override void SetVolume(byte newVolume)
        {
            if (newVolume > 15)
                newVolume = 15;
            this._readRegisters(); //Read the current register set
            this.registers[SYSCONFIG2] &= 0xFFF0; //Clear volume bits
            this.registers[SYSCONFIG2] |= newVolume; //Set new volume
            this._saveRegisters(); //Update
            base.SetVolume(newVolume);
        }

        private void _readRegisters()
        {
            throw new NotImplementedException();
        }

        private void _saveRegisters()
        {
            throw new NotImplementedException();
        }
    }
}
