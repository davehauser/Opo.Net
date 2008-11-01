using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;
using Opo.ProjectBase;

namespace Opo.Net.Mime
{
    /// <summary>
    /// MIME Parser whose parsing algorithm is based on Regular Expressions
    /// </summary>
    public class RegexMimeParser : IMimeParser
    {
        /// <summary>
        /// Parse all headers of the MIME message
        /// </summary>
        /// <param name="mimeData">A string containing the text data of a MIME message</param>
        /// <returns>A String array containing the headers of the MIME message</returns>
        public string[] ParseHeaders(string mimeData)
        {
            mimeData.Validate("mimeData");

            Regex r = new Regex(@"(\r\n\s*){2,}");
            string headerPart = r.Split(mimeData)[0];

            r = new Regex(@"(?<Header>.+:\s+[^\r\n]*(\r\n[\t ]+[^\r\n]+)*)");
            MatchCollection mc = r.Matches(headerPart);
            IList<string> headers = new List<string>();
            foreach (Match match in mc)
            {
                headers.Add(match.Groups["Header"].Value);
            }
            return headers.ToArray();
        }

        /// <summary>
        /// Extracts the value of a header from MIME data
        /// </summary>
        /// <param name="mimeData">A string containing the text data of a MIME message</param>
        /// <param name="headerName">A string containing the name of the header to parse</param>
        /// <returns>A String array containing the value of the specific header of the MIME message</returns>
        public string ParseHeaderValue(string mimeData, string headerName)
        {
            mimeData.Validate("mimeData");

            Regex r = new Regex(headerName + @":\s+(?<Header>[^\r\n]*(\r\n[\t\x20]+[^\r\n]+)*)");
            Match m = r.Match(mimeData);
            return m.Groups["Header"].Value.Trim();
        }

        /// <summary>
        /// Extracts the Content-Type from MIME data
        /// </summary>
        /// <param name="mimeData">A string containing the text data of a MIME message</param>
        /// <returns>A String array containing the Content-Type of the MIME message</returns>
        public string ParseContentType(string mimeData)
        {
            string ContentType = ParseHeaderValue(mimeData, "Content-Type");
            if (ContentType.Contains(Environment.NewLine))
            {
                ContentType = ContentType.Substring(0, ContentType.IndexOf(Environment.NewLine)).Trim().TrimEnd(';');
            }
            
            mimeData.Validate("mimeData");

            Regex r = new Regex(@"Content-Type:[\s]+(?<ContentType>.*)");
            Match m = r.Match(mimeData);
            return m.Groups["ContentType"].Value.Trim().Replace(";", "");
        }

        /// <summary>
        /// Extracts the charset from MIME data
        /// </summary>
        /// <param name="mimeData">A string containing the text data of a MIME message</param>
        /// <returns>A String array containing the charset of the MIME message</returns>
        public string ParseCharset(string mimeData)
        {
            mimeData.Validate("mimeData");

            Regex r = new Regex(@"[\s\t]+charset=\x22(?<Charset>.*)\x22");
            Match m = r.Match(mimeData);
            return m.Groups["Charset"].Value;
        }

        /// <summary>
        /// Extracts the boundary string from MIME data
        /// </summary>
        /// <param name="mimeData">A string containing the text data of a MIME message</param>
        /// <returns>A String array containing the boundary string of the MIME message</returns>
        public string ParseBoundary(string mimeData)
        {
            mimeData.Validate("mimeData");

            Regex r = new Regex(@"[\s\t]+boundary=\x22(?<Boundary>.*)\x22");
            Match m = r.Match(mimeData);
            return m.Groups["Boundary"].Value;
        }

        /// <summary>
        /// Extracts the Content part from MIME data
        /// </summary>
        /// <param name="mimeData">A string containing the text data of a MIME message</param>
        /// <returns>A String array containing the Content part of the MIME message</returns>
        public string ParseContent(string mimeData)
        {
            mimeData.Validate("mimeData");

            Regex r = new Regex(Environment.NewLine + @"\s*" + Environment.NewLine);
            Match m = r.Match(mimeData);
            return mimeData.Substring(m.Index + m.Length);
        }

    }
}
