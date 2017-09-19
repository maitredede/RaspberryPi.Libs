using System;
using System.Collections.Generic;
using System.Text;

namespace RaspberryPi.PiGPIO
{
    public interface IPiGPIO
    {
        /// <summary>
        /// Sets the GPIO mode, typically input or output
        /// </summary>
        /// <param name="gpio"></param>
        /// <param name="mode"></param>
        void SetMode(int gpio, Mode mode);

        /// <summary>
        /// Gets the GPIO mode
        /// </summary>
        /// <param name="gpio"></param>
        /// <returns></returns>
        Mode GetMode(int gpio);

        /// <summary>
        /// Sets or clears resistor pull ups or downs on the GPIO
        /// </summary>
        /// <param name="gpio"></param>
        /// <param name="pud"></param>
        void SetPullUpDown(int gpio, PullUpDown pud);

        /// <summary>
        /// Write GPIO value
        /// </summary>
        /// <param name="gpio"></param>
        /// <param name="value"></param>
        void Write(int gpio, bool value);

        /// <summary>
        /// Read GPIO value
        /// </summary>
        /// <param name="gpio"></param>
        /// <returns></returns>
        bool Read(int gpio);

        /// <summary>
        /// Sets a noise filter on a GPIO.
        ///
        /// Level changes on the GPIO are ignored until a level which has been stable for <paramref name="steady"/> microseconds is detected.
        /// Level changes on the GPIO are then reported for <paramref name="active"/> microseconds after which the process repeats.
        /// </summary>
        /// <param name="gpio"></param>
        /// <param name="steady"></param>
        /// <param name="active"></param>
        void NoiseFilter(int gpio, int steady, int active);

        CallbackInfo AddCallback(int gpio, Edge either, GpioCallback callback);
        void RemoveCallback(CallbackInfo callback);

        int HardwareRevision();

        #region I2C
        int I2COpen(int bus, int address, int flags);
        void I2CClose(int handle);

        byte[] I2CReadBytes(int handle, int num);
        #endregion

        #region Files
        int FileOpen(string file, int mode);
        int FileClose(int handle);
        byte[] FileRead(int handle, int count);
        int FileSeek(int handle, int offset, int from);
        void FileWrite(int handle, byte[] data);
        #endregion

        #region SPI
        int SpiOpen(int chan, int baud, int flags);
        void SpiClose(int handle);
        void SpiWrite(int handle, byte[] data);
        byte[] SpiRead(int handle, int count);
        byte[] SpiXfer(int handle, byte[] txBuffer);
        #endregion

        #region Waveforms
        void WaveFormNew();
        void WaveformAppend(Pulse[] pulse);
        int WaveformCreate();
        void WaveformDelete(int waveformId);
        #endregion

        #region BitBang SPI
        void BSpiOpen(int gpioCS, int gpioMiso, int gpioMosi, int gpioClk, int bauds, int flags);
        void BSpiClose(int gpioCS);
        byte[] BSpiXfer(int gpioCS, byte[] txBuffer);
        #endregion
    }
}
