using System;
using VeelPlezier.scr.enums;
using VeelPlezier.scr.utilities;
using Xunit;

namespace VeelPlezierTest.Tests.scr.utilities.util
{
    public sealed class ScreenTypeValueOf
    {
        [Theory]
        [InlineData("StartScreen")]
        [InlineData("MainScreen")]
        [InlineData("SettingsScreen")]
        public void ScreenTypeValueOfSuccessTest(string screenTypeName)
        {
            Assert.IsType<ScreenType>(Util.ScreenTypeValueOf(screenTypeName));
        }

        [Theory]
        [InlineData("21ffs")]
        [InlineData("%$")]
        public void ScreenTypeValueOfFailureTest(string screenTypeName)
        {
            Assert.Throws<ArgumentException>(() => { Util.ScreenTypeValueOf(screenTypeName); });
        }

        [Theory]
        [InlineData(null)]
        public void ScreenTypeValueOfThrowTest(string screenTypeName)
        {
            Assert.Throws<ArgumentNullException>(() => { Util.ScreenTypeValueOf(screenTypeName); });
        }
    }
}