using System;

namespace VeelPlezier.scr.enums
{
    internal sealed class TranslationLanguage
    {
        internal static readonly TranslationLanguage English = new("en",
            new Uri("..\\Resources\\StringResources.xaml", UriKind.Relative),
            new Uri("..\\Resources\\CalculatorResources.xaml", UriKind.Relative),
            new Uri("..\\Resources\\ReceiptResources.xaml", UriKind.Relative));

        internal static readonly TranslationLanguage Dutch = new("nl",
            new Uri("..\\Resources\\StringResources.nl-NL.xaml", UriKind.Relative),
            new Uri("..\\Resources\\CalculatorResources.nl-NL.xaml", UriKind.Relative),
            new Uri("..\\Resources\\ReceiptResources.nl-NL.xaml", UriKind.Relative));

        private TranslationLanguage(string languageShortCode, Uri uriToResource, Uri uriToCalculatorResource, Uri uriToReceiptResource)
        {
            LanguageShortCode = languageShortCode;
            UriToResource = uriToResource;
            UriToCalculatorResource = uriToCalculatorResource;
            UriToReceiptResource = uriToReceiptResource;
        }

        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        // ReSharper disable once MemberCanBePrivate.Global
        internal string LanguageShortCode { get; }
        internal Uri UriToResource { get; }
        internal Uri UriToCalculatorResource { get; }

        internal Uri UriToReceiptResource { get; }
    }
}