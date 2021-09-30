using System.Diagnostics.CodeAnalysis;
using System.Linq;
using JetBrains.Annotations;

namespace VeelPlezier.objects
{
    [SuppressMessage("ReSharper", "MemberCanBeInternal")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public sealed class Item
    {
        public Name[] Names { get; set; }
        public string Price { get; set; }

        public Item(Name[] names, string price)
        {
            Names = names;
            Price = price;
        }
        
        public Item()
        {
        }
        
        [CanBeNull]
        [ContractAnnotation("key:null => null")]
        public string GetTranslationByKey(string key)
        {
            if (string.IsNullOrEmpty(key))
                key = "en";
            
            return (from name in Names 
                where name.Key.Equals(key) 
                select name.Value).FirstOrDefault();
        }
    }
}