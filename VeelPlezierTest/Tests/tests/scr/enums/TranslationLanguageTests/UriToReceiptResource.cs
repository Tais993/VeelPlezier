using VeelPlezier.scr.enums;
using VeelPlezier.scr.utilities;
using Xunit;

namespace VeelPlezierTest.Tests.tests.scr.enums.TranslationLanguageTests
{
    public sealed class UriToReceiptResource
    {
        [Theory]
        [InlineData("en")]
        [InlineData("nl")]
        public void UriToReceiptResourceCodeSuccessTest(string languageCode)
        {
            TranslationLanguage language = Util.LanguageValueOf(languageCode);
            Assert.NotNull(language?.UriToReceiptResource);
        }
    }
}