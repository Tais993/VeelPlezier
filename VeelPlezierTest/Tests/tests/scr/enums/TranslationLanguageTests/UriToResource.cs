using VeelPlezier.scr.enums;
using VeelPlezier.scr.utilities;
using Xunit;

namespace VeelPlezierTest.Tests.tests.scr.enums.TranslationLanguageTests
{
    public sealed class UriToResource
    {
        [Theory]
        [InlineData("en")]
        [InlineData("nl")]
        public void UriToResourceSuccessTest(string languageCode)
        {
            TranslationLanguage language = Util.LanguageValueOf(languageCode);
            Assert.NotNull(language?.UriToResource);
        }
    }
}