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
    }
}
