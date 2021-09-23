using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace VeelPlezier.objects
{
    [SuppressMessage("ReSharper", "MemberCanBeInternal")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public sealed class Item
    {
        public Name[] Names { get; set; }
        public string Price { get; set; }
        
        public string GetByKey(string key)
        {
            return (from name in Names 
                where name.Key.Equals(key) 
                select name.Value).FirstOrDefault();
        }
    }
}