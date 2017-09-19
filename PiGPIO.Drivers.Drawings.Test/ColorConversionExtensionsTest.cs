using System;
using Xunit;

namespace PiGPIO.Drivers.Drawings.Test
{
    public class ColorConversionExtensions
    {
        [Fact]
        public void TestConsoleColorConversions()
        {
            foreach (ConsoleColor color in Enum.GetValues(typeof(ConsoleColor)))
            {
                try
                {
                    color.ToColor();
                }
                catch (Exception ex)
                {
                    throw new Exception($"Failed for value {color}", ex);
                }
            }
        }
    }
}
