using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;
using Opo.ProjectBase;

namespace Opo.Net.Mime
{
    public class RegexMimeParser : IMimeParser
    {
        public string ParseMessageID(string mimeData)
        {
            mimeData.Validate("mimeData");

            Regex r = new Regex(@"Message-ID:[\s]+\x3C(?<MessageID>.*)\x3E", RegexOptions.IgnoreCase);
            Match m = r.Match(mimeData);
            return m.Groups["MessageID"].Value;
        }

        public string ParseSubject(string mimeData)
        {
            mimeData.Validate("mimeData");

            Regex r = new Regex(@"Subject:[\s]+(?<Subject>(.*(\n[ \t]+.+)*)+)");
            Match m = r.Match(mimeData);
            return m.Groups["Subject"].Value.Trim();
        }

        public string ParseFrom(string mimeData)
        {
            mimeData.Validate("mimeData");

            string mailAddress;
            Regex r = new Regex(@"From:[\s]+(?<From>.*)", RegexOptions.IgnoreCase);
            Match m = r.Match(mimeData);
            if (m.Groups["From"].Length > 0)
            {
                mailAddress = m.Groups["From"].Value.Trim();
            }
            else
            {
                mailAddress = string.Empty;
            }
            return mailAddress;
        }
        public string[] ParseTo(string mimeData)
        {
            return ParseAddresses(mimeData, "To");
        }
        public string[] ParseCC(string mimeData)
        {
            return ParseAddresses(mimeData, "CC");
        }
        public string[] ParseBCC(string mimeData)
        {
            return ParseAddresses(mimeData, "BCC");
        }
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

        public string ParseDate(string mimeData)
        {
            mimeData.Validate("mimeData");

            DateTime messageDate = DateTime.MinValue;

            Regex r = new Regex(@"Date:[\s]+(?<Date>.*)");
            Match m = r.Match(mimeData);
            return m.Groups["Date"].Value.Trim();
        }

        public string ParseMimeVersion(string mimeData)
        {
            mimeData.Validate("mimeData");

            Regex r = new Regex(@"MIME-Version:[\s]+(?<Version>[\d\.]*)", RegexOptions.IgnoreCase);
            Match m = r.Match(mimeData);
            return m.Groups["Version"].Value;
        }

        public string ParsePriority(string mimeData)
        {
            mimeData.Validate("mimeData");

            Regex r = new Regex(@"X-Priority:[\s]+(?<Priority>\d)");
            Match m = r.Match(mimeData);
            return m.Groups["Priority"].Value;
        }

        public string ParseContentType(string mimeData)
        {
            mimeData.Validate("mimeData");

            Regex r = new Regex(@"Content-Type:[\s]+(?<ContentType>.*)");
            Match m = r.Match(mimeData);
            return m.Groups["ContentType"].Value.Trim().Replace(";", "");
        }

        public string ParseCharset(string mimeData)
        {
            mimeData.Validate("mimeData");

            Regex r = new Regex(@"[\s\t]+charset=\x22(?<Charset>.*)\x22");
            Match m = r.Match(mimeData);
            return m.Groups["Charset"].Value;
        }

        public string ParseBoundary(string mimeData)
        {
            mimeData.Validate("mimeData");

            Regex r = new Regex(@"[\s\t]+boundary=\x22(?<Boundary>.*)\x22");
            Match m = r.Match(mimeData);
            return m.Groups["Boundary"].Value;
        }

        public string ParseContent(string mimeData)
        {
            mimeData.Validate("mimeData");

            string boundary = ParseBoundary(mimeData);
            string content = Regex.Split(mimeData, "\r\n--" + boundary, RegexOptions.IgnoreCase)[1];
            System.Diagnostics.Debug.WriteLine(mimeData);
            return content.Trim();
        }

        public string ParseHeader(string mimeData, string headerName)
        {
            mimeData.Validate("mimeData");

            Regex r = new Regex(headerName + @":[\s]+(?<Header>.*)");
            Match m = r.Match(mimeData);
            return m.Groups["Header"].Value.Trim().TrimEnd(';');
        }
    }
}
