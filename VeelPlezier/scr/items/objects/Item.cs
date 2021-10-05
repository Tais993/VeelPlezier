using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using VeelPlezier.scr.enums;

namespace VeelPlezier.scr.items.objects
{
    public sealed record Item(IEnumerable<Name> Names, string Price)
    {
        internal IEnumerable<Name> Names { get; } = Names;
        public string Price { get; } = Price;

        [CanBeNull]
        public string GetTranslationByKey([NotNull] string key)
        {
            return (from name in Names
                where name.Key.Equals(key)
                select name.Value).First();
        }

        [CanBeNull]
        public string GetTranslationByTranslationLanguage([NotNull] TranslationLanguage language)
        {
            return GetTranslationByKey(language.LanguageShortCode);
        }
    }
}