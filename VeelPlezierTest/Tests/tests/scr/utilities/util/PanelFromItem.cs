using System;
using System.Windows.Controls;
using VeelPlezier.scr.items.objects;
using VeelPlezier.scr.utilities;
using VeelPlezierTest.Tests.test_utilities;
using Xunit;

namespace VeelPlezierTest.Tests.tests.scr.utilities.util
{
    public sealed class PanelFromItem
    {
        [WpfTheory]
        [InlineData("0,00", "nl", "NlName", 5, "nl")]
        [InlineData("0,00", "en", "EnName", 32653, "en")]
        public void PanelFromItemSuccessTest(string price, string nameKey, string name, int amount,
            string languageShortCode)
        {
            Panel panel = Util.PanelFromItem(new Item(new[]
            {
                new Name(nameKey, name)
            }, price), amount, TestUtil.GetTranslationLanguageByCode(languageShortCode));

            Assert.NotNull(panel);
        }

        [WpfTheory]
        [InlineData("0,00", "nl", "NlName", 5, "en")]
        [InlineData("0,00", "en", "EnName", 24222, "nl")]
        public void PanelFromItemFailureTest(string price, string nameKey, string name, int amount,
            string languageShortCode)
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                Util.PanelFromItem(new Item(new[]
                {
                    new Name(nameKey, name)
                }, price), amount, TestUtil.GetTranslationLanguageByCode(languageShortCode));
            });
        }
    }
}