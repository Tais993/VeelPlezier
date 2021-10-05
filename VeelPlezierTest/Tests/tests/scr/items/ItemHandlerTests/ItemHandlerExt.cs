using System.Windows.Controls;
using Newtonsoft.Json;
using VeelPlezier.scr.items;
using VeelPlezier.scr.items.objects;

namespace VeelPlezierTest.Tests.tests.scr.items.ItemHandlerTests
{
    internal sealed class ItemHandlerExt : ItemHandler
    {
        public const string Json = @"{
  
            ""ItemsArray"" : [
        {
            ""Names"" : [
            {""Key"" :  ""en"", ""Value"" :  ""Fries""},
            {""Key"" :  ""nl"", ""Value"" :  ""Friet""},
            {""Key"" : ""fake"", ""Value"" : ""fake""}
            ],
            ""Price"" : ""1.50""
        }
        ]
    }";

        public ItemHandlerExt(ComboBox itemsInStore) : base(itemsInStore)
        {
        }

        public void SetupItemHandler(string json)
        {
            Items = JsonConvert.DeserializeObject<Items>(json);
        }
    }
}