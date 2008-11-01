using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;

namespace Opo.Net.Mime
{
    public static class ContentTransferEncoding
    {
        public const string Unknown = "";
        public const string QuotedPrintable = "quoted-printable";
        public const string Base64 = "base64";
        public const string SevenBit = "7bit";
        public const string Binary = "binary";
    }
}
