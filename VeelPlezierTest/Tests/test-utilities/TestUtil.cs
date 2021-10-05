using VeelPlezier.scr.enums;
using VeelPlezier.scr.items.objects;

namespace VeelPlezierTest.Tests.test_utilities
{
    internal static class TestUtil
    {
        internal static Item GenerateItem(string price, string nameKey, string namesString)
        {
            Name[] names = new Name[1];

            names[0] = new Name(nameKey, namesString);

            return new Item(names, price);
        }

        internal static TranslationLanguage GetTranslationLanguageByCode(string languageCode)
        {
            switch (languageCode)
            {
                case "nl":
                    return TranslationLanguage.Dutch;
                case "en":
                    return TranslationLanguage.English;
                default:
                    return new TranslationLanguage(languageCode, null, null, null, null);
            }
        }
    }
}