using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace RaspberryPi.Userland
{
    /// <summary>
    /// Access to RPI LCD display stuff
    /// </summary>
    public static class LcdDisplay
    {
        public static readonly string PATH_BACKLIGHT_POWER = "/sys/class/backlight/rpi_backlight/bl_power";

        /// <summary>
        /// Backlight power on (true) or off (false)
        /// </summary>
        public static bool BacklightPower
        {
            get { return !ReadBool(PATH_BACKLIGHT_POWER); }
            set { Write(PATH_BACKLIGHT_POWER, !value); }
        }

        private static bool ReadBool(string path)
        {
            return ReadNum(path) != 0;
        }

        private static int ReadNum(string path)
        {
            string s = ReadString(path);
            return int.Parse(s);
        }

        private static string ReadString(string path)
        {
            using (StreamReader s = File.OpenText(path))
            {
                return s.ReadToEnd();
            }
        }

        private static void Write(string path, string value)
        {
            using (Stream s = File.Open(path, FileMode.Open, FileAccess.Write))
            using (StreamWriter sw = new StreamWriter(s))
            {
                sw.Write(value);
            }
        }

        private static void Write(string path, int value)
        {
            Write(path, value.ToString(CultureInfo.InvariantCulture));
        }

        private static void Write(string path, bool value)
        {
            Write(path, value ? 1 : 0);
        }
    }
}
