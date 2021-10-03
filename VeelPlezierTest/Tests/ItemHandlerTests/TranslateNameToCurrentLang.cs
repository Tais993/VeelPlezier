using System;
using System.Windows.Controls;
using Xunit;

// ReSharper disable All

namespace VeelPlezierTest.Tests.ItemHandlerTests
{
    public sealed class TranslateNameToCurrentLang
    {
        [Theory]
        [InlineData(ItemHandlerExt.Json, "friet", "en")]
        [InlineData(ItemHandlerExt.Json, "fake", "nl")]
        public async void TranslateNameToCurrentLangSuccessTest(string json, string itemToTranslate, string currentLang)
        {
            await Main.StartStaTask(() =>
            {
                ComboBox comboBox = new ComboBox();
                ItemHandlerExt itemHandlerExt = new ItemHandlerExt(comboBox);
                itemHandlerExt.SetupItemHandler(json);

                Assert.NotNull(itemHandlerExt.TranslateNameToCurrentLang(itemToTranslate, currentLang));
            });
        }

        [Theory]
        [InlineData(ItemHandlerExt.Json, "aaaa", "nl")]
        [InlineData(ItemHandlerExt.Json, "q4r1af", "fake")]
        public async void TranslateNameToCurrentLangFailTest(string json, string itemToTranslate, string currentLang)
        {
            await Main.StartStaTask(() =>
            {
                ComboBox comboBox = new ComboBox();
                ItemHandlerExt itemHandlerExt = new ItemHandlerExt(comboBox);
                itemHandlerExt.SetupItemHandler(json);

                Assert.Null(itemHandlerExt.TranslateNameToCurrentLang(itemToTranslate, currentLang));
            });
        }

        [Theory]
        [InlineData(ItemHandlerExt.Json, null, null)]
        public async void TranslateNameToCurrentLangThrowTest(string json, string itemToTranslate, string currentLang)
        {
            await Main.StartStaTask(() =>
            {
                ComboBox comboBox = new ComboBox();
                ItemHandlerExt itemHandlerExt = new ItemHandlerExt(comboBox);
                itemHandlerExt.SetupItemHandler(json);

                Assert.Throws<NullReferenceException>(() =>
                {
                    itemHandlerExt.TranslateNameToCurrentLang(itemToTranslate, currentLang);
                });
            });
        }
    }
}