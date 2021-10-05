using System;

namespace VeelPlezier.scr.enums
{
    public sealed class TranslationLanguage
    {
        public static readonly TranslationLanguage English = new("en",
            new Uri("..\\Resources\\StringResources.xaml", UriKind.Relative),
            new Uri("..\\Resources\\CalculatorResources.xaml", UriKind.Relative),
            new Uri("..\\Resources\\CurrencyConverterResources.xaml", UriKind.Relative),
            new Uri("..\\Resources\\ReceiptResources.xaml", UriKind.Relative));

        public static readonly TranslationLanguage Dutch = new("nl",
            new Uri("..\\Resources\\StringResources.nl-NL.xaml", UriKind.Relative),
            new Uri("..\\Resources\\CalculatorResources.nl-NL.xaml", UriKind.Relative),
            new Uri("..\\Resources\\CurrencyConverterResources.nl-NL.xaml", UriKind.Relative),
            new Uri("..\\Resources\\ReceiptResources.nl-NL.xaml", UriKind.Relative));


        public readonly string LanguageShortCode;
        public readonly Uri UriToResource;
        public readonly Uri UriToCalculatorResource;
        public readonly Uri UriToCurrencyConverterResource;
        public readonly Uri UriToReceiptResource;


        public TranslationLanguage(string languageShortCode, Uri uriToResource, Uri uriToCalculatorResource,
            Uri uriToCurrencyConverterResource, Uri uriToReceiptResource)
        {
            LanguageShortCode = languageShortCode;
            UriToResource = uriToResource;
            UriToCalculatorResource = uriToCalculatorResource;
            UriToCurrencyConverterResource = uriToCurrencyConverterResource;
            UriToReceiptResource = uriToReceiptResource;
        }
    }
}