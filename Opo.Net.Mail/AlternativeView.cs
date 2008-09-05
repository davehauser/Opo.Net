using System.Collections.Generic;
using System.Xml.Linq;

namespace Opo.Net.Mail
{
    public class AlternativeView : IAlternativeView
    {
        public string Content { get; set; }
        public string ContentType { get; set; }
        public string Charset { get; set; }
        public string TransferEncoding { get; set; }

        public AlternativeView(string content, string contentType)
        {
            Content = content;
            ContentType = contentType;
        }
        public AlternativeView(string content, string contentType, string charset, string transferEncoding)
        {
            Content = content;
            ContentType = contentType;
            Charset = charset;
            TransferEncoding = transferEncoding;
        }

        public static AlternativeView LoadXmlAlternativeView(XElement alternativeViewXml)
        {
            string content = alternativeViewXml.Value;
            string contentType = (alternativeViewXml.Attribute("ContentType") ?? new XAttribute("ContentType", "")).Value;
            string charset = (alternativeViewXml.Attribute("Charset") ?? new XAttribute("Charset", "")).Value;
            string transferEncoding = (alternativeViewXml.Attribute("TransferEncoding") ?? new XAttribute("TransferEncoding", "")).Value;

            AlternativeView v = new AlternativeView(content, contentType, charset, transferEncoding);
            return v;
        }
    }
}
