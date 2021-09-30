using System;

namespace VeelPlezier.enums
{
    internal sealed class TranslationLanguage
    {
        internal static readonly TranslationLanguage English = new TranslationLanguage("en", new Uri("..\\Resources\\StringResources.xaml", UriKind.Relative));
        internal static readonly TranslationLanguage Dutch = new TranslationLanguage("nl", new Uri("..\\Resources\\StringResources.nl-NL.xaml", UriKind.Relative));
        
        internal string LanguageShortCode { get; }
        internal Uri UriToResource { get; }
        
        private TranslationLanguage(string languageShortCode, Uri uriToResource)
        {
            LanguageShortCode = languageShortCode;
            UriToResource = uriToResource;
        }
    }
}