using VeelPlezier.scr.enums;
using VeelPlezier.scr.utilities;
using Xunit;

namespace VeelPlezierTest.Tests.tests.scr.enums.TranslationLanguageTests
{
    public sealed class LanguageShortCode
    {
        [Theory]
        [InlineData("en")]
        [InlineData("nl")]
        public void LanguageShortCodeSuccessTest(string languageCode)
        {
            TranslationLanguage language = Util.LanguageValueOf(languageCode);
            Assert.Equal(languageCode, language?.LanguageShortCode);
        }
    }
}