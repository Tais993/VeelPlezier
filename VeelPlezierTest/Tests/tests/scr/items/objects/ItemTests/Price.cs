using VeelPlezier.scr.items.objects;
using VeelPlezierTest.Tests.test_utilities;
using Xunit;

namespace VeelPlezierTest.Tests.tests.scr.items.objects.ItemTests
{
    public sealed class Price
    {
        [Theory]
        [InlineData("10.00", "nl", "NlName")]
        [InlineData("25.00", "4291i2142141", "FirstName")]
        public void GetTranslationByKeySuccessTest(string price, string nameKey, string name)
        {
            Item item = TestUtil.GenerateItem(price, nameKey, name);

            Assert.Equal(item.Price, price);
        }
    }
}