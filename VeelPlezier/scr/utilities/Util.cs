using System;
using JetBrains.Annotations;
using VeelPlezier.enums;

namespace VeelPlezier
{
    internal static class Util
    {
        /// <summary>
        /// Returns the <see cref="Language"/> based on it's name
        /// </summary>
        /// <param name="name">The <see cref="Language">Language's</see> name as a <see cref="string">String</see></param>
        /// <returns>The (nullable) relating <see cref="Language"/></returns>
        [CanBeNull]
        internal static Language LanguageValueOf([NotNull] string name)
        {
            switch (name.ToLower())
            {
                case "nl":
                case "dutch":
                    return Language.Dutch;
                case "en":
                case "english":
                    return Language.English;
                
                default:
                    throw new ArgumentOutOfRangeException("Doesn't exist");
            }
        }

        
        /// <summary>
        /// Returns the ScreenType based on it's name
        /// </summary>
        /// <param name="name">The ScreenType's name as a <see cref="string">String</see></param>
        /// <returns>The relating <see cref="ScreenType">ScreenType</see></returns>
        internal static ScreenType ScreenTypeValueOf([NotNull] string name)
        {
            return (ScreenType) Enum.Parse(typeof(ScreenType), name);
        }
        
        /// <summary>
        /// Parses the given String to a double
        /// Supports usage of both . and ,
        /// </summary>
        /// <param name="s">The <see cref="string"/> you want to be parsed</param>
        /// <returns>The string parsed to a <see cref="double"/></returns>
        internal static double ParseToDouble(string s)
        {
            return ParseToDouble(s, exception => { });
        }
        
        internal static double ParseToDouble(string s, [NotNull] Action<Exception> action)
        {
            try
            {
                return double.Parse(
                    s.Replace('.', ',')
                );
            }
            catch (Exception e)
            {
                action.Invoke(e);
            }

            return 0;
        }
    }
}