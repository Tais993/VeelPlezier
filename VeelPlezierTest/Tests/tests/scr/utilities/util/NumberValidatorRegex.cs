using VeelPlezier.scr.utilities;
using Xunit;

namespace VeelPlezierTest.Tests.scr.utilities.util
{
    public sealed class NumberValidatorRegex
    {
        [Theory]
        [InlineData("24214")]
        [InlineData("12412412412412341234234.")]
        [InlineData("212209,252241321312")]
        public void NumberValidatorRegexSuccessTest(string numberToTest)
        {
            Assert.DoesNotMatch(Util.NumberValidator, numberToTest);
        }
    }
}