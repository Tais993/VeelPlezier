using System;
using System.Windows.Controls;
using VeelPlezierTest.Tests.tests.scr.items.ItemHandlerTests;
using Xunit;

// ReSharper disable All

namespace VeelPlezierTest.Tests.ItemHandlerTests
{
    public sealed class TranslateNameToCurrentLang
    {
        [WpfTheory]
        [InlineData(ItemHandlerExt.Json, "friet", "en")]
        [InlineData(ItemHandlerExt.Json, "fake", "nl")]
        public void TranslateNameToCurrentLangSuccessTest(string json, string itemToTranslate, string currentLang)
        {
            ComboBox comboBox = new ComboBox();
            ItemHandlerExt itemHandlerExt = new ItemHandlerExt(comboBox);
            itemHandlerExt.SetupItemHandler(json);

            Assert.NotNull(itemHandlerExt.TranslateNameToCurrentLang(itemToTranslate, currentLang));
        }

        [WpfTheory]
        [InlineData(ItemHandlerExt.Json, "aaaa", "nl")]
        [InlineData(ItemHandlerExt.Json, "q4r1af", "fake")]
        public void TranslateNameToCurrentLangFailTest(string json, string itemToTranslate, string currentLang)
        {
            ComboBox comboBox = new ComboBox();
            ItemHandlerExt itemHandlerExt = new ItemHandlerExt(comboBox);
            itemHandlerExt.SetupItemHandler(json);

            Assert.Null(itemHandlerExt.TranslateNameToCurrentLang(itemToTranslate, currentLang));
        }

        [WpfTheory]
        [InlineData(ItemHandlerExt.Json, null, null)]
        public void TranslateNameToCurrentLangThrowTest(string json, string itemToTranslate, string currentLang)
        {
            ComboBox comboBox = new ComboBox();
            ItemHandlerExt itemHandlerExt = new ItemHandlerExt(comboBox);
            itemHandlerExt.SetupItemHandler(json);

            Assert.Throws<NullReferenceException>(() =>
            {
                itemHandlerExt.TranslateNameToCurrentLang(itemToTranslate, currentLang);
            });
        }
    }
}