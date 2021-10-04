using System;
using VeelPlezier.scr.items.objects;
using VeelPlezierTest.Tests.test_utilities;
using Xunit;

namespace VeelPlezierTest.Tests.scr.items.objects.ItemTests
{
    public sealed class GetTranslationByKey
    {
        [Theory]
        [InlineData("0.00", "nl", "NlName")]
        [InlineData("0.00", "en", "EnName")]
        [InlineData("0.00", "^&^&%^#%#edsfjewirnwe", "Name")]
        public void GetTranslationByKeySuccessTest(string price, string nameKey, string name)
        {
            Item item = TestUtil.GenerateItem(price, nameKey, name);

            Assert.Equal(name, item.GetTranslationByKey(nameKey));
        }

        [Theory]
        [InlineData("0.00", "Nl", "NlName", "FakeName")]
        [InlineData("0.00", "En", "EnName", "FakeName")]
        [InlineData("0.00", "En", "EnName", "^!@$x")]
        public void GetTranslationByKeyFailureTest(string price, string nameKey, string name, string fakeName)
        {
            Item item = TestUtil.GenerateItem(price, nameKey, name);

            Assert.Throws<InvalidOperationException>(() => { item.GetTranslationByKey(fakeName); });
        }
    }
}