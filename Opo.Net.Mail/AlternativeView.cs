using System.Collections.Generic;
using System.Xml.Linq;

namespace Opo.Net.Mail
{
    /// <summary>
    /// Represents an alternative view for a mail message body
    /// </summary>
    public class AlternativeView : IAlternativeView
    {
        /// <summary>
        /// Gets or sets the content of the view
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// Gets or sets the Content-Type (e.g. "text/html")
        /// </summary>
        public string ContentType { get; set; }
        /// <summary>
        /// Gets or sets the charset (e.g. "iso-8859-1")
        /// </summary>
        public string Charset { get; set; }
        /// <summary>
        /// Gets or sets the Content-Transfer-Encoding (e.g. "quoted-printable")
        /// </summary>
        public string TransferEncoding { get; set; }

        /// <summary>
        /// Initializes a new instance of the AlernativeView class
        /// </summary>
        /// <param name="content">A String representing the content of the view</param>
        /// <param name="contentType">The Content-Type (e.g. "text/html")</param>
        public AlternativeView(string content, string contentType)
        {
            Content = content;
            ContentType = contentType;
        }
        /// <summary>
        /// Initializes a new instance of the AlernativeView class
        /// </summary>
        /// <param name="content">A String representing the content of the view</param>
        /// <param name="contentType">The Content-Type (e.g. "text/html")</param>
        /// <param name="charset">The charset of the attachment (e.g. "iso-8859-1")</param>
        /// <param name="transferEncoding">The Content-Transfer-Encoding (e.g. "quoted-printable")</param>
        public AlternativeView(string content, string contentType, string charset, string transferEncoding)
        {
            Content = content;
            ContentType = contentType;
            Charset = charset;
            TransferEncoding = transferEncoding;
        }

        /// <summary>
        /// Loads XML data to an alternative view
        /// </summary>
        /// <param name="alternativeViewXml">XElement representing the alternative view</param>
        /// <returns>A new instance of the AlternativeView class representing the XML data</returns>
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
