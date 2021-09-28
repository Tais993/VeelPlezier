using System;

namespace VeelPlezier
{
    internal class Util
    {
        internal static double ParseToDouble(string s)
        {
            try
            {
                return double.Parse(
                    s
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