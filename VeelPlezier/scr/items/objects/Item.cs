using System.Diagnostics.CodeAnalysis;
using System.Linq;
using JetBrains.Annotations;
using VeelPlezier.scr.enums;

namespace VeelPlezier.scr.items.objects
{
    [SuppressMessage("ReSharper", "MemberCanBeInternal")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    internal sealed record Item(Name[] Names, string Price)
    {
        internal Name[] Names { get; } = Names;
        internal string Price { get; } = Price;

        [CanBeNull]
        internal string GetTranslationByKey(string key)
        {
            if (string.IsNullOrEmpty(key))
                key = "en";

            return (from name in Names
                where name.Key.Equals(key)
                select name.Value).First();
        }

        [CanBeNull]
        internal string GetTranslationByTranslationLanguage([NotNull] TranslationLanguage language)
        {
            return GetTranslationByKey(language.LanguageShortCode);
        }
    }
}