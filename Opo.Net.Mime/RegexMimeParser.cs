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
        /// Extracts the Message-ID from MIME data
        /// </summary>
        /// <param name="mimeData">A string containing the text data of a MIME message</param>
        /// <returns>A String containing the Message-ID of the MIME message</returns>
        public string ParseMessageID(string mimeData)
        {
            return ParseHeader(mimeData, "Message-ID").Replace("<", "").Replace(">", "");
        }

        /// <summary>
        /// Extracts the Subject from MIME data
        /// </summary>
        /// <param name="mimeData">A string containing the text data of a MIME message</param>
        /// <returns>A String containing the Subject of the MIME message</returns>
        public string ParseSubject(string mimeData)
        {
            return ParseHeader(mimeData, "Subject");
        }

        /// <summary>
        /// Extracts the From address from MIME data
        /// </summary>
        /// <param name="mimeData">A string containing the text data of a MIME message</param>
        /// <returns>A String containing the From address of the MIME message</returns>
        public string ParseFrom(string mimeData)
        {
            return ParseHeader(mimeData, "From");
        }
        /// <summary>
        /// Extracts the To addresses from MIME data
        /// </summary>
        /// <param name="mimeData">A string containing the text data of a MIME message</param>
        /// <returns>A String array containing the To addresses of the MIME message</returns>
        public string[] ParseTo(string mimeData)
        {
            return ParseAddresses(mimeData, "To");
        }
        /// <summary>
        /// Extracts the CC addresses from MIME data
        /// </summary>
        /// <param name="mimeData">A string containing the text data of a MIME message</param>
        /// <returns>A String array containing the CC addresses of the MIME message</returns>
        public string[] ParseCC(string mimeData)
        {
            return ParseAddresses(mimeData, "CC");
        }
        /// <summary>
        /// Extracts the BCC addresses from MIME data
        /// </summary>
        /// <param name="mimeData">A string containing the text data of a MIME message</param>
        /// <returns>A String array containing the BCC addresses of the MIME message</returns>
        public string[] ParseBCC(string mimeData)
        {
            return ParseAddresses(mimeData, "BCC");
        }
        /// <summary>
        /// Extracts addresses from MIME data
        /// </summary>
        /// <param name="mimeData">A string containing the text data of a MIME message</param>
        /// <param name="type">A string containing the type of the addresses (e.g. "From", "CC")</param>
        /// <returns>A String array containing addresses of the MIME message</returns>
        private string[] ParseAddresses(string mimeData, string type)
        {
            mimeData.Validate("mimeData");

            string[] addresses = new string[] { };
            Regex r = new Regex(type + @":[\s]+(?<Addresses>(.*(\n[ \t]+.+)*)+)", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            Match m = r.Match(mimeData);
            if (m.Groups["Addresses"].Length > 0)
            {
                addresses = m.Groups["Addresses"].Value.Split(',');
                for (int i = 0; i < addresses.Length; i++)
                {
                    string revisedAddress = addresses[i].Trim();
                    if (revisedAddress.StartsWith("=?"))
                    {
                        // is quoted-printable?
                        int p = revisedAddress.ToLower().IndexOf("?q?");
                        if (p > -1)
                        {
                            revisedAddress = Mime.MimeEncoding.QuotedPrintable.Decode(revisedAddress.Substring(p + 3).Replace("?=", ""));
                        }
                    }
                    addresses[i] = revisedAddress;
                }
            }
            return addresses;
        }

        /// <summary>
        /// Extracts the Date from MIME data
        /// </summary>
        /// <param name="mimeData">A string containing the text data of a MIME message</param>
        /// <returns>A String array containing the date of the MIME message</returns>
        public string ParseDate(string mimeData)
        {
            return ParseHeader(mimeData, "Date");
        }

        /// <summary>
        /// Extracts the MIME-Version from MIME data
        /// </summary>
        /// <param name="mimeData">A string containing the text data of a MIME message</param>
        /// <returns>A String array containing the MIME-Version of the MIME message</returns>
        public string ParseMimeVersion(string mimeData)
        {
            mimeData.Validate("mimeData");

            Regex r = new Regex(@"MIME-Version:[\s]+(?<Version>[\d\.]*)", RegexOptions.IgnoreCase);
            Match m = r.Match(mimeData);
            return m.Groups["Version"].Value;
        }

        /// <summary>
        /// Extracts the X-Priority from MIME data
        /// </summary>
        /// <param name="mimeData">A string containing the text data of a MIME message</param>
        /// <returns>A String array the X-Priority of the MIME message</returns>
        public string ParsePriority(string mimeData)
        {
            return ParseHeader(mimeData, "X-Priority");
        }

        /// <summary>
        /// Extracts the Content-Type from MIME data
        /// </summary>
        /// <param name="mimeData">A string containing the text data of a MIME message</param>
        /// <returns>A String array containing the Content-Type of the MIME message</returns>
        public string ParseContentType(string mimeData)
        {
            string ContentType = ParseHeader(mimeData, "Content-Type");
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

        /// <summary>
        /// Extracts the value of a header from MIME data
        /// </summary>
        /// <param name="mimeData">A string containing the text data of a MIME message</param>
        /// <param name="headerName">A string containing the name of the header to parse</param>
        /// <returns>A String array containing the value of the specific header of the MIME message</returns>
        public string ParseHeader(string mimeData, string headerName)
        {
            mimeData.Validate("mimeData");

            Regex r = new Regex(headerName + @":\s+(?<Header>.+\;\s+.+|.+)");
            Match m = r.Match(mimeData);
            return m.Groups["Header"].Value.Trim();
        }
    }
}
