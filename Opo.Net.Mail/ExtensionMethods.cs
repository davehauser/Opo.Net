using System;
using System.Web;

namespace Opo.Net.Mail
{
    public static class ExtensionMethods
    {
        #region POP 3
        internal static bool IsOK(this string s)
        {
            return s.StartsWith("+OK");
        }
        internal static bool IsERR(this string s)
        {
            return s.StartsWith("-ERR");
        }
        #endregion

        #region MailMessage

        #endregion

        #region string
        public static string HtmlEncode(this string s)
        {
            return HttpUtility.HtmlEncode(s);
        }
        public static string HtmlDecode(this string s)
        {
            return HttpUtility.HtmlDecode(s);
        }
        public static bool IsNullOrEmpty(this string s)
        {
            return String.IsNullOrEmpty(s);
        }
        public static bool IsNotNullOrEmpty(this string s)
        {
            return !String.IsNullOrEmpty(s);
        }
        #endregion
    }
}
