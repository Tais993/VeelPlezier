using System.Linq;
using System.Windows.Controls;
using VeelPlezier.scr.enums;
using VeelPlezierTest.Tests.test_utilities;
using Xunit;

namespace VeelPlezierTest.Tests.tests.scr.items.ItemHandlerTests
{
    public sealed class ReloadItemsInDisplay
    {
        [WpfTheory]
        [InlineData("nl")]
        [InlineData("en")]
        public void ReloadItemsInDisplaySuccessTest(string languageCode)
        {
            TranslationLanguage translationLanguage = TestUtil.GetTranslationLanguageByCode(languageCode);

            ComboBox comboBox = new ComboBox();
            ItemHandlerExt itemHandler = new ItemHandlerExt(comboBox);
            itemHandler.SetupItemHandler(ItemHandlerExt.Json);

            int count = itemHandler.Items.ItemsArray.Count();
            itemHandler.ReloadItemsInDisplay(translationLanguage);

            Assert.True(comboBox.Items.Count == count);
        }
    }
}