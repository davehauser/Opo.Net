using System;
using System.Web;

namespace Opo.Net.Mail
{
    /// <summary>
    /// Extension methods for strings
    /// </summary>
    public static class StringExtensionMethods
    {
        /// <summary>
        /// Returns a value indicating whether a String starts with "+OK"
        /// </summary>
        /// <param name="s">String to test</param>
        /// <returns>True if s starts with "+OK"</returns>
        internal static bool IsOK(this string s)
        {
            return s.StartsWith("+OK");
        }
        /// <summary>
        /// Returns a value indicating whether a String starts with "-ERR"
        /// </summary>
        /// <param name="s">String to test</param>
        /// <returns>True if s starts with "-ERR"</returns>
        internal static bool IsERR(this string s)
        {
            return s.StartsWith("-ERR");
        }

        /// <summary>
        /// HTML-encodes a string and returns the encoded string
        /// </summary>
        /// <param name="s">String to encode</param>
        /// <returns>Encoded string</returns>
        public static string HtmlEncode(this string s)
        {
            return HttpUtility.HtmlEncode(s);
        }
        /// <summary>
        /// Decodes an HTML-encoded string and returns the decoded string
        /// </summary>
        /// <param name="s">HTML-encoded string</param>
        /// <returns>Decoded string</returns>
        public static string HtmlDecode(this string s)
        {
            return HttpUtility.HtmlDecode(s);
        }
        /// <summary>
        /// Indicates whether the specified String object is a null reference (Nothing in Visual Basic) or an Empty string
        /// </summary>
        /// <param name="s">A string reference</param>
        /// <returns>true if the value parameter is nullNothingnullptra null reference (Nothing in Visual Basic) or an empty string (""); otherwise, false</returns>
        public static bool IsNullOrEmpty(this string s)
        {
            return String.IsNullOrEmpty(s);
        }
        /// <summary>
        /// Indicates whether the specified String object is not a null reference (Nothing in Visual Basic) or an Empty string
        /// </summary>
        /// <param name="s">A string reference</param>
        /// <returns>true if the value parameter is not a null reference (Nothing in Visual Basic) or an empty string (""); otherwise, false</returns>
        public static bool IsNotNullOrEmpty(this string s)
        {
            return !String.IsNullOrEmpty(s);
        }

        /// <summary>
        /// Tries to convert a string to a date. Returns DateTime.MinValue if the conversion fails.
        /// </summary>
        /// <param name="s">String to convert</param>
        /// <returns></returns>
        public static DateTime ToDate(this string s)
        {
            DateTime date = DateTime.MinValue;
            DateTime.TryParse(s, out date);
            return date;
        }
        /// <summary>
        /// Parses string to Enum.
        /// </summary>
        /// <typeparam name="T">Enumeration type</typeparam>
        /// <param name="s">String to parse</param>
        /// <param name="defaultValue">The default value if parsing fails.</param>
        /// <returns></returns>
        public static T ToEnum<T>(this string s, T defaultValue)
        {
            T parsedEnum;
            try
            {
                parsedEnum = (T)Enum.Parse(typeof(T), s);
            }
            catch (ArgumentException)
            {
                parsedEnum = defaultValue;
            }
            return parsedEnum;
        }
        public static int ToInt(this string s)
        {
            int i = -1;
            Int32.TryParse(s, out i);
            return i;
        }
        public static long ToLong(this string s)
        {
            long l = -1;
            Int64.TryParse(s, out l);
            return l;
        }
    }
}
