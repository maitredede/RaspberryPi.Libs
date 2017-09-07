using System;
using System.Collections.Generic;
using System.Text;

namespace PiGPIO
{
    public class PiGPIOException : Exception
    {
        public int ErrorCode { get; }

        internal PiGPIOException(int error) : base(LookupError(error))
        {
            this.ErrorCode = error;
        }

        private static string LookupError(int error)
        {
            switch (error)
            {
                case -1: // PI_INIT_FAILED
                    return "gpioInitialise failed";
                case -2: // PI_BAD_USER_GPIO
                    return "GPIO not 0-31";
                case -3: // PI_BAD_GPIO
                    return "GPIO not 0-53";
                case -4: // PI_BAD_MODE
                    return "mode not 0-7";
                case -5: // PI_BAD_LEVEL
                    return "level not 0-1";
                case -6: // PI_BAD_PUD
                    return "pud not 0-2";
                case -7: // PI_BAD_PULSEWIDTH
                    return "pulsewidth not 0 or 500-2500";
                case -8: // PI_BAD_DUTYCYCLE
                    return "dutycycle outside set range";
                case -9: // PI_BAD_TIMER
                    return "timer not 0-9";
                case -10: // PI_BAD_MS
                    return "ms not 10-60000";
                case -11: // PI_BAD_TIMETYPE
                    return "timetype not 0-1";
                case -12: // PI_BAD_SECONDS
                    return "seconds < 0";
                case -13: // PI_BAD_MICROS
                    return "micros not 0-999999";
                case -14: // PI_TIMER_FAILED
                    return "gpioSetTimerFunc failed";
                case -15: // PI_BAD_WDOG_TIMEOUT
                    return "timeout not 0-60000";
                case -16: // PI_NO_ALERT_FUNC
                    return "DEPRECATED";
                case -17: // PI_BAD_CLK_PERIPH
                    return "clock peripheral not 0-1";
                case -18: // PI_BAD_CLK_SOURCE
                    return "DEPRECATED";
                case -19: // PI_BAD_CLK_MICROS
                    return "clock micros not 1, 2, 4, 5, 8, or 10";
                case -20: // PI_BAD_BUF_MILLIS
                    return "buf millis not 100-10000";
                case -21: // PI_BAD_DUTYRANGE
                    return "dutycycle range not 25-40000";
                case -22: // PI_BAD_SIGNUM
                    return "signum not 0-63";
                case -23: // PI_BAD_PATHNAME
                    return "can't open pathname";
                case -24: // PI_NO_HANDLE
                    return "no handle available";
                case -25: // PI_BAD_HANDLE
                    return "unknown handle";
                case -26: // PI_BAD_IF_FLAGS
                    return "ifFlags > 3";
                case -27: // PI_BAD_PRIM_CHANNEL
                    return "DMA primary channel not 0-14";
                case -28: // PI_BAD_SOCKET_PORT
                    return "socket port not 1024-32000";
                case -29: // PI_BAD_FIFO_COMMAND
                    return "unrecognized fifo command";
                case -30: // PI_BAD_SECO_CHANNEL
                    return "DMA secondary channel not 0-6";
                case -31: // PI_NOT_INITIALISED
                    return "function called before gpioInitialise";
                case -32: // PI_INITIALISED
                    return "function called after gpioInitialise";
                case -33: // PI_BAD_WAVE_MODE
                    return "waveform mode not 0-3";
                case -34: // PI_BAD_CFG_INTERNAL
                    return "bad parameter in gpioCfgInternals call";
                case -35: // PI_BAD_WAVE_BAUD
                    return "baud rate not 50-250K(RX)/50-1M(TX)";
                case -36: // PI_TOO_MANY_PULSES
                    return "waveform has too many pulses";
                case -37: // PI_TOO_MANY_CHARS
                    return "waveform has too many chars";
                case -38: // PI_NOT_SERIAL_GPIO
                    return "no bit bang serial read on GPIO";
                case -39: // PI_BAD_SERIAL_STRUC
                    return "bad (null) serial structure parameter";
                case -40: // PI_BAD_SERIAL_BUF
                    return "bad (null) serial buf parameter";
                case -41: // PI_NOT_PERMITTED
                    return "GPIO operation not permitted";
                case -42: // PI_SOME_PERMITTED
                    return "one or more GPIO not permitted";
                case -43: // PI_BAD_WVSC_COMMND
                    return "bad WVSC subcommand";
                case -44: // PI_BAD_WVSM_COMMND
                    return "bad WVSM subcommand";
                case -45: // PI_BAD_WVSP_COMMND
                    return "bad WVSP subcommand";
                case -46: // PI_BAD_PULSELEN
                    return "trigger pulse length not 1-100";
                case -47: // PI_BAD_SCRIPT
                    return "invalid script";
                case -48: // PI_BAD_SCRIPT_ID
                    return "unknown script id";
                case -49: // PI_BAD_SER_OFFSET
                    return "add serial data offset > 30 minutes";
                case -50: // PI_GPIO_IN_USE
                    return "GPIO already in use";
                case -51: // PI_BAD_SERIAL_COUNT
                    return "must read at least a byte at a time";
                case -52: // PI_BAD_PARAM_NUM
                    return "script parameter id not 0-9";
                case -53: // PI_DUP_TAG
                    return "script has duplicate tag";
                case -54: // PI_TOO_MANY_TAGS
                    return "script has too many tags";
                case -55: // PI_BAD_SCRIPT_CMD
                    return "illegal script command";
                case -56: // PI_BAD_VAR_NUM
                    return "script variable id not 0-149";
                case -57: // PI_NO_SCRIPT_ROOM
                    return "no more room for scripts";
                case -58: // PI_NO_MEMORY
                    return "can't allocate temporary memory";
                case -59: // PI_SOCK_READ_FAILED
                    return "socket read failed";
                case -60: // PI_SOCK_WRIT_FAILED
                    return "socket write failed";
                case -61: // PI_TOO_MANY_PARAM
                    return "too many script parameters (> 10)";
                case -62: // PI_SCRIPT_NOT_READY
                    return "script initialising";
                case -63: // PI_BAD_TAG
                    return "script has unresolved tag";
                case -64: // PI_BAD_MICS_DELAY
                    return "bad MICS delay (too large)";
                case -65: // PI_BAD_MILS_DELAY
                    return "bad MILS delay (too large)";
                case -66: // PI_BAD_WAVE_ID
                    return "non existent wave id";
                case -67: // PI_TOO_MANY_CBS
                    return "No more CBs for waveform";
                case -68: // PI_TOO_MANY_OOL
                    return "No more OOL for waveform";
                case -69: // PI_EMPTY_WAVEFORM
                    return "attempt to create an empty waveform";
                case -70: // PI_NO_WAVEFORM_ID
                    return "no more waveforms";
                case -71: // PI_I2C_OPEN_FAILED
                    return "can't open I2C device";
                case -72: // PI_SER_OPEN_FAILED
                    return "can't open serial device";
                case -73: // PI_SPI_OPEN_FAILED
                    return "can't open SPI device";
                case -74: // PI_BAD_I2C_BUS
                    return "bad I2C bus";
                case -75: // PI_BAD_I2C_ADDR
                    return "bad I2C address";
                case -76: // PI_BAD_SPI_CHANNEL
                    return "bad SPI channel";
                case -77: // PI_BAD_FLAGS
                    return "bad i2c/spi/ser open flags";
                case -78: // PI_BAD_SPI_SPEED
                    return "bad SPI speed";
                case -79: // PI_BAD_SER_DEVICE
                    return "bad serial device name";
                case -80: // PI_BAD_SER_SPEED
                    return "bad serial baud rate";
                case -81: // PI_BAD_PARAM
                    return "bad i2c/spi/ser parameter";
                case -82: // PI_I2C_WRITE_FAILED
                    return "i2c write failed";
                case -83: // PI_I2C_READ_FAILED
                    return "i2c read failed";
                case -84: // PI_BAD_SPI_COUNT
                    return "bad SPI count";
                case -85: // PI_SER_WRITE_FAILED
                    return "ser write failed";
                case -86: // PI_SER_READ_FAILED
                    return "ser read failed";
                case -87: // PI_SER_READ_NO_DATA
                    return "ser read no data available";
                case -88: // PI_UNKNOWN_COMMAND
                    return "unknown command";
                case -89: // PI_SPI_XFER_FAILED
                    return "spi xfer/read/write failed";
                case -90: // PI_BAD_POINTER
                    return "bad (NULL) pointer";
                case -91: // PI_NO_AUX_SPI
                    return "no auxiliary SPI on Pi A or B";
                case -92: // PI_NOT_PWM_GPIO
                    return "GPIO is not in use for PWM";
                case -93: // PI_NOT_SERVO_GPIO
                    return "GPIO is not in use for servo pulses";
                case -94: // PI_NOT_HCLK_GPIO
                    return "GPIO has no hardware clock";
                case -95: // PI_NOT_HPWM_GPIO
                    return "GPIO has no hardware PWM";
                case -96: // PI_BAD_HPWM_FREQ
                    return "hardware PWM frequency not 1-125M";
                case -97: // PI_BAD_HPWM_DUTY
                    return "hardware PWM dutycycle not 0-1M";
                case -98: // PI_BAD_HCLK_FREQ
                    return "hardware clock frequency not 4689-250M";
                case -99: // PI_BAD_HCLK_PASS
                    return "need password to use hardware clock 1";
                case -100: // PI_HPWM_ILLEGAL
                    return "illegal, PWM in use for main clock";
                case -101: // PI_BAD_DATABITS
                    return "serial data bits not 1-32";
                case -102: // PI_BAD_STOPBITS
                    return "serial (half) stop bits not 2-8";
                case -103: // PI_MSG_TOOBIG
                    return "socket/pipe message too big";
                case -104: // PI_BAD_MALLOC_MODE
                    return "bad memory allocation mode";
                case -105: // PI_TOO_MANY_SEGS
                    return "too many I2C transaction segments";
                case -106: // PI_BAD_I2C_SEG
                    return "an I2C transaction segment failed";
                case -107: // PI_BAD_SMBUS_CMD
                    return "SMBus command not supported by driver";
                case -108: // PI_NOT_I2C_GPIO
                    return "no bit bang I2C in progress on GPIO";
                case -109: // PI_BAD_I2C_WLEN
                    return "bad I2C write length";
                case -110: // PI_BAD_I2C_RLEN
                    return "bad I2C read length";
                case -111: // PI_BAD_I2C_CMD
                    return "bad I2C command";
                case -112: // PI_BAD_I2C_BAUD
                    return "bad I2C baud rate, not 50-500k";
                case -113: // PI_CHAIN_LOOP_CNT
                    return "bad chain loop count";
                case -114: // PI_BAD_CHAIN_LOOP
                    return "empty chain loop";
                case -115: // PI_CHAIN_COUNTER
                    return "too many chain counters";
                case -116: // PI_BAD_CHAIN_CMD
                    return "bad chain command";
                case -117: // PI_BAD_CHAIN_DELAY
                    return "bad chain delay micros";
                case -118: // PI_CHAIN_NESTING
                    return "chain counters nested too deeply";
                case -119: // PI_CHAIN_TOO_BIG
                    return "chain is too long";
                case -120: // PI_DEPRECATED
                    return "deprecated function removed";
                case -121: // PI_BAD_SER_INVERT
                    return "bit bang serial invert not 0 or 1";
                case -122: // PI_BAD_EDGE
                    return "bad ISR edge value, not 0-2";
                case -123: // PI_BAD_ISR_INIT
                    return "bad ISR initialisation";
                case -124: // PI_BAD_FOREVER
                    return "loop forever must be last command";
                case -125: // PI_BAD_FILTER
                    return "bad filter parameter";
                case -126: // PI_BAD_PAD
                    return "bad pad number";
                case -127: // PI_BAD_STRENGTH
                    return "bad pad drive strength";
                case -128: // PI_FIL_OPEN_FAILED
                    return "file open failed";
                case -129: // PI_BAD_FILE_MODE
                    return "bad file mode";
                case -130: // PI_BAD_FILE_FLAG
                    return "bad file flag";
                case -131: // PI_BAD_FILE_READ
                    return "bad file read";
                case -132: // PI_BAD_FILE_WRITE
                    return "bad file write";
                case -133: // PI_FILE_NOT_ROPEN
                    return "file not open for read";
                case -134: // PI_FILE_NOT_WOPEN
                    return "file not open for write";
                case -135: // PI_BAD_FILE_SEEK
                    return "bad file seek";
                case -136: // PI_NO_FILE_MATCH
                    return "no files match pattern";
                case -137: // PI_NO_FILE_ACCESS
                    return "no permission to access file";
                case -138: // PI_FILE_IS_A_DIR
                    return "file is a directory";
                case -139: // PI_BAD_SHELL_STATUS
                    return "bad shell return status";
                case -140: // PI_BAD_SCRIPT_NAME
                    return "bad script name";
                case -141: // PI_BAD_SPI_BAUD
                    return "bad SPI baud rate, not 50-500k";
                case -142: // PI_NOT_SPI_GPIO
                    return "no bit bang SPI in progress on GPIO";
                case -143: // PI_BAD_EVENT_ID
                    return "bad event id";
                default:
                    return $"Unknown error code : {error}";
            }
        }
    }
}
