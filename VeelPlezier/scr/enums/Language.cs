namespace VeelPlezier.enums
{
    public class Language
    {
        public static readonly Language English = new Language("en");
        public static readonly Language Dutch = new Language("nl");
        
        private string LanguageShortCode { get; }
        
        private Language(string languageShortCode)
        {
            this.LanguageShortCode = languageShortCode;
        }
    }
}