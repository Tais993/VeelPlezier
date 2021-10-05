using VeelPlezier.scr.utilities;
using Xunit;

namespace VeelPlezierTest.Tests.tests.scr.utilities.util
{
    public sealed class ParseToInt
    {
        [Theory]
        [InlineData("1")]
        [InlineData("5835129")]
        [InlineData("2147483647")]
        public void ParseToIntSuccessTest(string intToParse)
        {
            Assert.False(Util.ParseToInt(intToParse) == -1);
        }

        [Theory]
        [InlineData("ffs")]
        [InlineData("5022951295159125")]
        [InlineData("2.00")]
        public void ParseToIntFailureTest(string intToParse)
        {
            Assert.True(Util.ParseToInt(intToParse) == -1);
        }
    }
}