using System;
using System.Windows.Controls;
using VeelPlezierTest.Tests.tests.scr.items.ItemHandlerTests;
using Xunit;

// ReSharper disable All

namespace VeelPlezierTest.Tests.ItemHandlerTests
{
    public sealed class GetItemByName
    {
        [WpfTheory]
        [InlineData(ItemHandlerExt.Json, "friet")]
        [InlineData(ItemHandlerExt.Json, "Fries")]
        public void GetItemByNameSuccessTest(string json, string itemToGet)
        {
            ComboBox comboBox = new ComboBox();
            ItemHandlerExt itemHandlerExt = new ItemHandlerExt(comboBox);
            itemHandlerExt.SetupItemHandler(json);

            Assert.NotNull(itemHandlerExt.GetItemByName(itemToGet));
        }

        [WpfTheory]
        [InlineData(ItemHandlerExt.Json, "aaa")]
        [InlineData(ItemHandlerExt.Json, "12412E")]
        public void GetItemByNameFailTest(string json, string itemToGet)
        {
            ComboBox comboBox = new ComboBox();
            ItemHandlerExt itemHandlerExt = new ItemHandlerExt(comboBox);
            itemHandlerExt.SetupItemHandler(json);

            Assert.Null(itemHandlerExt.GetItemByName(itemToGet));
        }

        [WpfTheory]
        [InlineData(ItemHandlerExt.Json, null)]
        public void GetItemByNameThrowTest(string json, string itemToGet)
        {
            ComboBox comboBox = new ComboBox();
            ItemHandlerExt itemHandlerExt = new ItemHandlerExt(comboBox);
            itemHandlerExt.SetupItemHandler(json);

            Assert.Throws<NullReferenceException>(() => { itemHandlerExt.GetItemByName(itemToGet); });
        }
    }
}