using System;
using System.Linq;

namespace Nucleotic.Common.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Parses the enum.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static T ParseEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        /// <summary>
        /// Replaces all.
        /// </summary>
        /// <param name="seed">The seed.</param>
        /// <param name="chars">The chars.</param>
        /// <param name="replacementCharacter">The replacement character.</param>
        /// <returns></returns>
        public static string ReplaceAll(this string seed, char[] chars, char replacementCharacter)
        {
            return chars.Aggregate(seed, (str, cItem) => str.Replace(cItem, replacementCharacter));
        }

        /// <summary>
        /// Determines whether this instance is numeric.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>
        ///   <c>true</c> if the specified input is numeric; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNumeric(this string input)
        {
            int test;
            return int.TryParse(input, out test);
        }
    }
}