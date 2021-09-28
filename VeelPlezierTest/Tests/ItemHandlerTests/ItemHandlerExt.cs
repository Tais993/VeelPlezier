using System.Windows.Controls;
using Newtonsoft.Json;
using VeelPlezier;
using VeelPlezier.objects;

namespace VeelPlezierTest.Tests.ItemHandlerTests
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