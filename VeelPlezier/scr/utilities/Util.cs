using System;
using System.Linq;
using System.Windows.Controls;
using JetBrains.Annotations;
using VeelPlezier.scr.enums;

namespace VeelPlezier.scr.utilities
{
    internal static class Util
    {
        /// <summary>
        /// Returns the first <see cref="Label"/> with the given name
        /// </summary>
        /// <param name="panel">The <see cref="Panel"/> to grab the <see cref="Label"/> from</param>
        /// <param name="name">The <see cref="Label">Label's</see> name as a <see cref="string"/></param>
        /// <returns></returns>
        [NotNull]
        internal static Label GetLabelByNameFromPanel([NotNull] Panel panel, string name)
        {
            return panel.Children.OfType<Label>().First(label => label.Name.Equals(name));
        }

        /// <summary>
        /// Returns the <see cref="TranslationLanguage"/> based on it's name
        /// </summary>
        /// <param name="name">The <see cref="TranslationLanguage">Language's</see> name as a <see cref="string">String</see></param>
        /// <returns>The (nullable) relating <see cref="TranslationLanguage"/></returns>
        [CanBeNull]
        internal static TranslationLanguage LanguageValueOf([NotNull] string name)
        {
            switch (name.ToLower())
            {
                case "nl":
                case "dutch":
                    return TranslationLanguage.Dutch;
                case "en":
                case "english":
                    return TranslationLanguage.English;

                default:
                    throw new ArgumentOutOfRangeException(nameof(name));
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
        /// Parses the given String to a int
        /// Supports usage of both . and ,
        /// </summary>
        /// <param name="s">The <see cref="string"/> you want to be parsed</param>
        /// <returns>The string parsed to a <see cref="int"/></returns>
        internal static int ParseToInt(string s)
        {
            return ParseToInt(s, static _ => { });
        }

        /// <summary>
        /// Parses the given String to a int
        /// Supports usage of both . and ,
        /// </summary>
        /// <param name="s">The <see cref="string"/> you want to be parsed</param>
        /// <param name="onError">The <see cref="Action"/> that will run when something goes wrong</param>
        /// <returns>The string parsed to a <see cref="int"/></returns>
        internal static int ParseToInt(string s, [NotNull] Action<Exception> onError)
        {
            try
            {
                return int.Parse(
                    s.Replace('.', ',')
                );
            }
            catch (Exception e)
            {
                onError.Invoke(e);
            }

            return 0;
        }

        /// <summary>
        /// Parses the given String to a double
        /// Supports usage of both . and ,
        /// </summary>
        /// <param name="s">The <see cref="string"/> you want to be parsed</param>
        /// <returns>The string parsed to a <see cref="double"/></returns>
        internal static double ParseToDouble(string s)
        {
            return ParseToDouble(s, static _ => { });
        }

        /// <summary>
        /// Parses the given String to a double
        /// Supports usage of both . and ,
        /// </summary>
        /// <param name="s">The <see cref="string"/> you want to be parsed</param>
        /// <param name="onError">The <see cref="Action"/> that will run when something goes wrong</param>
        /// <returns>The string parsed to a <see cref="double"/></returns>
        internal static double ParseToDouble(string s, [NotNull] Action<Exception> onError)
        {
            try
            {
                return double.Parse(
                    s.Replace('.', ',')
                );
            }
            catch (Exception e)
            {
                onError.Invoke(e);
            }

            return 0;
        }
    }
}