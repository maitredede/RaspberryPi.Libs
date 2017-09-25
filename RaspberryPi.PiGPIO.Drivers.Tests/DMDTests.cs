using Moq;
using RaspberryPi.PiGPIO.Drivers.Dede;
using RaspberryPi.PiGPIO.Drivers.Freetronics;
using System;
using Xunit;

namespace RaspberryPi.PiGPIO.Drivers.Tests
{
    public class DMDTests
    {
        [Theory]
        [InlineData(1, 1)]
        [InlineData(1, 10)]
        [InlineData(10, 1)]
        [InlineData(100, 100)]
        public void DMDSurface_should_SPI_same_bits(int panelsWidth, int panelsHeight)
        {
            PigsGenerator gpioRef = new PigsGenerator(Environment.NewLine);
            DMDPinLayout layout = new DMDPinLayout(1, 2, 3, 4, 5, 6);
            DMD dmdRef = new DMD(gpioRef, layout, panelsWidth, panelsHeight);
            dmdRef.Init();
            this.DrawPattern(dmdRef);
            dmdRef.ScanFull();

            PigsGenerator gpioTest = new PigsGenerator();
            FreetronicsDMDSurface dmdTest = new FreetronicsDMDSurface(gpioTest, layout, panelsWidth, panelsHeight);
            dmdTest.Init(false);
            this.DrawPattern(dmdTest);
            ((IDMDInternals)dmdTest).ScanFull();

            Assert.Equal(gpioRef.ToString(), gpioTest.ToString());
        }

        private void DrawPattern(IDMDInternals dmd)
        {
            int panelsWidth = dmd.PanelsWide;
            int panelsHeight = dmd.PanelsTall;

            for (int i = 0; i < Math.Max(panelsWidth * 32, panelsHeight * 16); i++)
            {
                ((Freetronics.IDMDInternals)dmd).SetPixel(i, i, true);
            }
        }
    }
}

//public interface ITarget
//{
//    void A(string val);
//    void B(int val);
//}

//public class RefClass
//{
//    private readonly ITarget m_target;
//    public RefClass(ITarget target) { this.m_target = target; }
//    public void Work()
//    {
//        this.m_target.A("A");
//        this.m_target.A("B");
//        this.m_target.B(0);
//        this.m_target.B(1);
//    }
//}

//public class TestClass
//{
//    private readonly ITarget m_target;
//    public TestClass(ITarget target) { this.m_target = target; }
//    public void Work()
//    {
//        this.m_target.A("B");
//        this.m_target.B(1);
//        this.m_target.B(0);
//        this.m_target.A("A");
//    }
//}

//public class WorkTest
//{
//    [Fact]
//    public void Work_is_done_in_sequence()
//    {
//        var refTarget = new Mock<ITarget>();
//        var refClass = new RefClass(refTarget.Object);
//        refClass.Work();

//        var testTarget = new Mock<ITarget>();
//        var testClass = new TestClass(testTarget.Object);
//        testClass.Work();

//        //TODO : compare refTarget and testTarget method call sequence
//    }
//}