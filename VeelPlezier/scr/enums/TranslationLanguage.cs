using System;

namespace VeelPlezier.scr.enums
{
    public sealed class TranslationLanguage
    {
        public static readonly TranslationLanguage English = new("en",
            new Uri("..\\Resources\\StringResources.xaml", UriKind.Relative),
            new Uri("..\\Resources\\CalculatorResources.xaml", UriKind.Relative),
            new Uri("..\\Resources\\ReceiptResources.xaml", UriKind.Relative));

        public static readonly TranslationLanguage Dutch = new("nl",
            new Uri("..\\Resources\\StringResources.nl-NL.xaml", UriKind.Relative),
            new Uri("..\\Resources\\CalculatorResources.nl-NL.xaml", UriKind.Relative),
            new Uri("..\\Resources\\ReceiptResources.nl-NL.xaml", UriKind.Relative));

        public TranslationLanguage(string languageShortCode, Uri uriToResource, Uri uriToCalculatorResource,
            Uri uriToReceiptResource)
        {
            LanguageShortCode = languageShortCode;
            UriToResource = uriToResource;
            UriToCalculatorResource = uriToCalculatorResource;
            UriToReceiptResource = uriToReceiptResource;
        }

        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        // ReSharper disable once MemberCanBePrivate.Global
        public string LanguageShortCode { get; }
        public Uri UriToResource { get; }
        public Uri UriToCalculatorResource { get; }

        public Uri UriToReceiptResource { get; }
    }
}