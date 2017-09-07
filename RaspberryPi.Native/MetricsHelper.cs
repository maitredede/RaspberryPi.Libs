using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace RaspberryPi
{
    /// <summary>
    /// Helper methods to gather metrics
    /// </summary>
    public static class MetricsHelper
    {
        private static readonly Regex regResponseLine = new Regex(@"^(?<name>[A-Za-z0-9]+)(?:\((?<id>\d+)\))?=(?<value>[A-Za-z0-9]*)", RegexOptions.Compiled | RegexOptions.CultureInvariant);
        private static readonly Regex regUnit = new Regex(@"^(?<value>\d+)(?<unit>[A-Za-z']{1,2})?", RegexOptions.Compiled | RegexOptions.CultureInvariant);

        private static long GenCmdUnitLong(string command)
        {
            string response = BcmHost.GenCmd(command);
            Match m = regResponseLine.Match(response);
            if (m.Success)
            {
                Match mUnit = regUnit.Match(m.Groups["value"].Value);
                if (mUnit.Success)
                {
                    long value = long.Parse(mUnit.Groups["value"].Value);
                    if (mUnit.Groups["unit"].Success)
                    {
                        switch (mUnit.Groups["unit"].Value)
                        {
                            case "K":
                                value *= 1024;
                                break;
                            case "M":
                                value *= 1024 * 1024;
                                break;
                            case "G":
                                value *= 1024 * 1024;
                                break;
                            case "'C":
                            case "V":
                                break;
                            default:
                                throw new InvalidOperationException("Unknown unit: " + mUnit.Groups["unit"].Value);
                        }
                    }
                    return value;
                }
                else
                {
                    return long.Parse(m.Groups["value"].Value);
                }
            }
            else
            {
                throw new InvalidOperationException($"Can't parse response '{response}'");
            }
        }

        private static double GenCmdUnitDouble(string command)
        {
            string response = BcmHost.GenCmd(command);
            Match m = regResponseLine.Match(response);
            if (m.Success)
            {
                Match mUnit = regUnit.Match(m.Groups["value"].Value);
                if (mUnit.Success)
                {
                    double value = double.Parse(mUnit.Groups["value"].Value);
                    if (mUnit.Groups["unit"].Success)
                    {
                        switch (mUnit.Groups["unit"].Value)
                        {
                            case "K":
                                value *= 1024;
                                break;
                            case "M":
                                value *= 1024 * 1024;
                                break;
                            case "G":
                                value *= 1024 * 1024;
                                break;
                            case "'C":
                            case "V":
                                break;
                            default:
                                throw new InvalidOperationException("Unknown unit: " + mUnit.Groups["unit"].Value);
                        }
                    }
                    return value;
                }
                else
                {
                    return double.Parse(m.Groups["value"].Value);
                }
            }
            else
            {
                throw new InvalidOperationException($"Can't parse response '{response}'");
            }
        }

        public static long DedicatedMemoryCpu()
        {
            return GenCmdUnitLong("get_mem arm");
        }

        public static long DedicatedMemoryGpu()
        {
            return GenCmdUnitLong("get_mem gpu");
        }

        public static double GpuTemp()
        {
            return GenCmdUnitDouble("measure_temp");
        }

        public static double CpuTemp()
        {
            using (FileStream fs = File.OpenRead("/sys/class/thermal/thermal_zone0/temp"))
            using (StreamReader sr = new StreamReader(fs))
            {
                double value = double.Parse(sr.ReadLine());
                return value / 1000;
            }
        }
    }
}
