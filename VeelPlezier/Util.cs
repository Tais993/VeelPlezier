using System;
using JetBrains.Annotations;
using VeelPlezier.enums;

namespace VeelPlezier
{
    internal static class Util
    {
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
            try
            {
                return double.Parse(
                    s.Replace('.', ',')
                );
            }
            catch (Exception)
            {
                return 0;
                // TODO: error handling
            }
        }
    }
}