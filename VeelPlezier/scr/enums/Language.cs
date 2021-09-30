using System;

namespace VeelPlezier.enums
{
    internal sealed class Language
    {
        internal static readonly Language English = new Language("en", new Uri("..\\Resources\\StringResources.xaml", UriKind.Relative));
        internal static readonly Language Dutch = new Language("nl", new Uri("..\\Resources\\StringResources.nl-NL.xaml", UriKind.Relative));
        
        internal string LanguageShortCode { get; }
        internal Uri UriToResource { get; }
        
        private Language(string languageShortCode, Uri uriToResource)
        {
            this.LanguageShortCode = languageShortCode;
            this.UriToResource = uriToResource;
        }
    }
}