using System;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;

namespace Opo.Net.Mime
{
    /// <summary>
    /// Methods for encoding and decoding MIME content
    /// </summary>
    public static class MimeEncoding
    {
        /// <summary>
        /// Encode and decode data with base64 encoding
        /// </summary>
        public static class Base64
        {
            /// <summary>
            /// Encode data with base64 encoding
            /// </summary>
            /// <param name="obj">Object to encode</param>
            /// <returns>Base64 encoded string</returns>
            public static string Encode(object obj)
            {
                byte[] byteObj;
                if (obj is String)
                {
                    byteObj = Encoding.UTF8.GetBytes((obj as String));
                }
                else
                {
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        BinaryFormatter binaryFormatter = new BinaryFormatter();
                        binaryFormatter.Serialize(memoryStream, obj);
                        byteObj = memoryStream.GetBuffer();
                    }
                }
                System.Diagnostics.Debug.WriteLine(Convert.ToBase64String(byteObj, Base64FormattingOptions.InsertLineBreaks));
                return Convert.ToBase64String(byteObj, Base64FormattingOptions.InsertLineBreaks);
            }

            /// <summary>
            /// Decode base64 encoded content
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="s">Base64 encoded string</param>
            /// <returns>Decoded object of type T</returns>
            public static T Decode<T>(string s)
            {
                object o = new BinaryFormatter().Deserialize(Decode(s));
                return (T)o;
            }

            /// <summary>
            /// Decode base64 encoded content
            /// </summary>
            /// <param name="s">Base64 encoded string</param>
            /// <returns>Decoded object</returns>
            public static Stream Decode(string s)
            {
                MemoryStream stream;
                try
                {
                    stream = new MemoryStream(Convert.FromBase64String(s));
                }
                catch (Exception)
                {
                    stream = new MemoryStream(Encoding.UTF8.GetBytes(s));
                }
                return stream;
            }
        }

        /// <summary>
        /// Encode and decode data with quoted-printable
        /// </summary>
        public static class QuotedPrintable
        {
            /// <summary>
            /// Decode quoted-printable encoded content
            /// </summary>
            /// <param name="s">Quoted-printable encoded string</param>
            /// <returns>Decoded string</returns>
            public static string Decode(string s)
            {
                string decodedText = s;
                decodedText = decodedText.Replace("=" + Environment.NewLine, "");
                Regex hexRegex = new Regex(@"(\=([0-9A-F][0-9A-F]))", RegexOptions.IgnoreCase);
                decodedText = hexRegex.Replace(decodedText, new MatchEvaluator(HexDecodeMatchEvaluator));
                return decodedText;
            }
            private static string HexDecodeMatchEvaluator(Match m)
            {
                int dec = Convert.ToInt32(m.Groups[2].Value, 16);
                char character = Convert.ToChar(dec);
                return character.ToString();
            }

            /// <summary>
            /// Encode string with quoted-printable encoding
            /// </summary>
            /// <param name="s">String to encode</param>
            /// <returns>Quoted-printable encoded string</returns>
            public static string Encode(string s)
            {
                string encodedText;
                Regex r = new Regex(@"[^\r\n\x20-\x7E]|\x3D|([\ \t](?=[\r\n]))");
                encodedText = r.Replace(s, new MatchEvaluator(HexEncodeMatchEvaluator));
                encodedText = LimitLineLength(encodedText, 75);
                return encodedText;
            }
            private static string HexEncodeMatchEvaluator(Match m)
            {
                char character = m.Value[0];
                int dec = (int)character;
                return "=" + dec.ToString("X");
            }
            /// <summary>
            /// Limits the line length of a string and inserts linebreaks after the given maximum line length
            /// </summary>
            /// <param name="s">A String which is to be processed</param>
            /// <param name="maxLineLength">An Int32 declaring the maximum line length</param>
            /// <returns></returns>
            private static string LimitLineLength(string s, int maxLineLength)
            {
                if (s.Length < maxLineLength)
                    return s;

                StringBuilder formattedText = new StringBuilder();
                while (true)
                {
                    if (s.Length < maxLineLength)
                    {
                        formattedText.Append(s);
                        break;
                    }
                    else
                    {
                        int splitPosition = s.LastIndexOf('=', maxLineLength);
                        if (splitPosition < 0 || splitPosition < maxLineLength - 2)
                            splitPosition = maxLineLength;
                        formattedText.AppendLine(s.Substring(0, splitPosition) + "=");
                        s = s.Remove(0, splitPosition);
                    }
                }
                return formattedText.ToString();
            }
        }
    }
}