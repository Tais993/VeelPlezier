using System;
using System.Windows.Controls;
using Xunit;

// ReSharper disable All

namespace VeelPlezierTest.Tests.ItemHandlerTests
{
    public sealed class GetItemByName
    {
        [Theory]
        [InlineData(ItemHandlerExt.Json, "friet")]
        [InlineData(ItemHandlerExt.Json, "Fries")]
        public async void GetItemByNameSuccessTest(string json, string itemToGet)
        {
            await Main.StartStaTask(() =>
            {
                ComboBox comboBox = new ComboBox();
                ItemHandlerExt itemHandlerExt = new ItemHandlerExt(comboBox);
                itemHandlerExt.SetupItemHandler(json);

                Assert.NotNull(itemHandlerExt.GetItemByName(itemToGet));
            });
        }

        [Theory]
        [InlineData(ItemHandlerExt.Json, "aaa")]
        [InlineData(ItemHandlerExt.Json, "12412E")]
        public async void GetItemByNameFailTest(string json, string itemToGet)
        {
            await Main.StartStaTask(() =>
            {
                ComboBox comboBox = new ComboBox();
                ItemHandlerExt itemHandlerExt = new ItemHandlerExt(comboBox);
                itemHandlerExt.SetupItemHandler(json);

                Assert.Null(itemHandlerExt.GetItemByName(itemToGet));
            });
        }

        [Theory]
        [InlineData(ItemHandlerExt.Json, null)]
        public async void GetItemByNameThrowTest(string json, string itemToGet)
        {
            await Main.StartStaTask(() =>
            {
                ComboBox comboBox = new ComboBox();
                ItemHandlerExt itemHandlerExt = new ItemHandlerExt(comboBox);
                itemHandlerExt.SetupItemHandler(json);

                Assert.Throws<NullReferenceException>(() => { itemHandlerExt.GetItemByName(itemToGet); });
            });
        }
    }
}