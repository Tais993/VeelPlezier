using System.Windows.Controls;
using Xunit;

namespace VeelPlezierTest.Tests.ItemHandlerTests
{
    public sealed class ReloadItemsInDisplay
    {
        [Theory]
        [InlineData(ItemHandlerExt.Json, "en")]
        [InlineData(ItemHandlerExt.Json, "nl")]
        [InlineData(ItemHandlerExt.Json, "fake")]
        public async void ReloadItemsInDisplaySuccessTest(string json, string lang)
        {
            await Main.StartStaTask(() =>
            {
                ComboBox comboBox = new ComboBox();
                ItemHandlerExt itemHandlerExt = new ItemHandlerExt(comboBox);
                itemHandlerExt.SetupItemHandler(json);
                
                Assert.False(itemHandlerExt.ReloadItemsInDisplay("en"));
                
                Assert.True(itemHandlerExt.ReloadItemsInDisplay(lang));
            });    
        }
    }
}