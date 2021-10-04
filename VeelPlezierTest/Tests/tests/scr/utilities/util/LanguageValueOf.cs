using System;
using VeelPlezier.scr.utilities;
using Xunit;

namespace VeelPlezierTest.Tests.scr.utilities.util
{
    public sealed class LanguageValueOf
    {
        [Theory]
        [InlineData("en")]
        [InlineData("dutch")]
        public void LanguageValueOfSuccessTest(string languageCode)
        {
            Assert.NotNull(Util.LanguageValueOf(languageCode));
        }

        [Theory]
        [InlineData("21ffs")]
        [InlineData("%$")]
        public void LanguageValueOfFailureTest(string languageCode)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => { Util.LanguageValueOf(languageCode); });
        }

        [Theory]
        [InlineData(null)]
        public void LanguageValueOfThrowTest(string languageCode)
        {
            Assert.Throws<NullReferenceException>(() => { Util.LanguageValueOf(languageCode); });
        }
    }
}