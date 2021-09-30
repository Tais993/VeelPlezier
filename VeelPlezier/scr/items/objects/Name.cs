using System.Diagnostics.CodeAnalysis;

namespace VeelPlezier.objects
{
    [SuppressMessage("ReSharper", "MemberCanBeInternal")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public sealed class Name
    {
        public string Key { get; set; }
        public string Value { get; set; }
        
        public Name(string key, string value)
        {
            this.Key = key;
            this.Value = value;
        }

        public Name()
        {
        }
    }
}