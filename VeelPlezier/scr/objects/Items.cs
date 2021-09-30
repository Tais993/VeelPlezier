using System.Diagnostics.CodeAnalysis;

namespace VeelPlezier.objects
{
    [SuppressMessage("ReSharper", "MemberCanBeInternal")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public sealed class Items
    {
        public Item[] ItemsArray { get; set; }
    }
}