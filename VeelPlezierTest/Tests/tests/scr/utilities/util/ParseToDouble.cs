using System;
using VeelPlezier.scr.utilities;
using Xunit;

namespace VeelPlezierTest.Tests.tests.scr.utilities.util
{
    public sealed class ParseToDouble
    {
        [Theory]
        [InlineData("1.00")]
        [InlineData("5681592")]
        [InlineData("1.79769313486232")]
        public void ParseToDoubleSuccessTest(string doubleToParse)
        {
            Assert.False(Math.Abs(Util.ParseToDouble(doubleToParse) - (-1.00)) < 0.01);
        }

        [Theory]
        [InlineData("ffs")]
        [InlineData("2,.29")]
        public void ParseToDoubleFailureTest(string doubleToParse)
        {
            Assert.True(Math.Abs(Util.ParseToDouble(doubleToParse) - (-1.00)) < 0.01);
        }
    }
}