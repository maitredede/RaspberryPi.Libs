using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace RaspberryPi.LibLedMatrix
{
    [StructLayout(LayoutKind.Sequential)]
    public sealed class LedMatrixOptions : IDisposable
    {
        private IntPtr hardware_mapping_ptr;
        public int rows;
        public int chain_length;
        public int parallel;
        public int pwm_bits;
        public int pwm_lsb_nanoseconds;
        public int brightness;
        public int scan_mode;
        private IntPtr led_rgb_sequence_ptr;
        public uint disable_hardware_pulsing;
        public uint show_refresh_rate;
        public uint inverse_colors;

        public string hardware_mapping
        {
            get
            {
                return GetString(this.hardware_mapping_ptr);
            }
            set
            {
                SetString(ref this.hardware_mapping_ptr, value);
            }
        }

        public string led_rgb_sequence
        {
            get
            {
                return GetString(this.led_rgb_sequence_ptr);
            }
            set
            {
                SetString(ref this.led_rgb_sequence_ptr, value);
            }
        }

        private static string GetString(IntPtr ptr)
        {
            if (ptr == IntPtr.Zero)
                return null;
            return Marshal.PtrToStringAnsi(ptr);
        }

        private static void SetString(ref IntPtr ptr, string value)
        {
            if (ptr != IntPtr.Zero)
                Marshal.FreeHGlobal(ptr);
            ptr = Marshal.StringToHGlobalAnsi(value);
        }

        void IDisposable.Dispose()
        {
            if (this.hardware_mapping_ptr != IntPtr.Zero)
                Marshal.FreeHGlobal(this.hardware_mapping_ptr);
        }
    }
}
