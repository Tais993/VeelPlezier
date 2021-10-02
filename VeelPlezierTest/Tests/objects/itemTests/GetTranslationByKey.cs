using System;
using Xunit;

namespace VeelPlezierTest.Tests.objects.itemTests
{
    public sealed class GetTranslationByKey
    {
        [Theory]
        [InlineData(1, "en")]
        [InlineData(1, "nl")]
        [InlineData(1, "fake")]
        [InlineData(2, "en")]
        [InlineData(2, "nl")]
        public void GetItemByNameSuccessTest(int itemNumber, string lang)
        {
            Item item = ItemExt.GetItemByInt(itemNumber);

            Assert.Equal(ItemExt.GetExpectedTranslation(itemNumber, lang), (string) item.GetTranslationByKey(lang));
        }

        [Theory]
        [InlineData(1, "en", "nl")]
        [InlineData(1, "nl", "fake")]
        [InlineData(2, "en", "nl")]
        [InlineData(2, "fake", "nl")]
        public void GetItemByNameFailTest(int itemNumber, string lang, string fakeLang)
        {
            Item item = ItemExt.GetItemByInt(itemNumber);

            Assert.NotEqual<string>(ItemExt.GetExpectedTranslation(itemNumber, lang),
                item.GetTranslationByKey(fakeLang));
        }

        [Theory]
        [InlineData(1, null)]
        [InlineData(-10, null)]
        [InlineData(-5, "14212")]
        public void GetItemByNameThrowTest(int itemNumber, string lang)
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Item item = ItemExt.GetItemByInt(itemNumber);
                Assert.Equal(ItemExt.GetExpectedTranslation(itemNumber, lang), (string) item.GetTranslationByKey(lang));
            });
        }
    }
}