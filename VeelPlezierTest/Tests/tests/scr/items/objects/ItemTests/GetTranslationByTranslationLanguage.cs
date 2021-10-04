using System;
using VeelPlezier.scr.enums;
using VeelPlezier.scr.items.objects;
using VeelPlezierTest.Tests.test_utilities;
using Xunit;

namespace VeelPlezierTest.Tests.scr.items.objects.ItemTests
{
    public sealed class GetTranslationByTranslationLanguage
    {
        [Theory]
        [InlineData("0.00", "nl", "NlName", "nl")]
        [InlineData("0.00", "en", "EnName", "en")]
        public void GetTranslationByKeySuccessTest(string price, string nameKey, string name,
            string translationLanguageKey)
        {
            Item item = TestUtil.GenerateItem(price, nameKey, name);

            TranslationLanguage translationLanguage = TestUtil.GetTranslationLanguageByCode(translationLanguageKey);
            Assert.Equal(name, item.GetTranslationByTranslationLanguage(translationLanguage));
        }

        [Theory]
        [InlineData("0.00", "nl", "NlName", "en")]
        [InlineData("0.00", "nl", "EnName", "null")]
        [InlineData("0.00", "en", "EnName", "nl")]
        public void GetTranslationByKeyFailureTest(string price, string nameKey, string name,
            string translationLanguageKey)
        {
            Item item = TestUtil.GenerateItem(price, nameKey, name);

            Assert.Throws<InvalidOperationException>(() =>
            {
                item.GetTranslationByTranslationLanguage(
                    TestUtil.GetTranslationLanguageByCode(translationLanguageKey));
            });
        }
    }
}