using System.Text;

namespace Opo.Net.Mail
{
    /// <summary>
    /// Represents an alternative view for a mail message body
    /// </summary>
    public interface IAlternativeView
    {
        /// <summary>
        /// Gets or sets the content of the view
        /// </summary>
        string Content { get; set; }
        /// <summary>
        /// Gets or sets the Content-Type (e.g. "text/html")
        /// </summary>
        string ContentType { get; set; }
        /// <summary>
        /// Gets or sets the charset (e.g. "iso-8859-1")
        /// </summary>
        string Charset { get; set; }
        /// <summary>
        /// Gets or sets the Content-Transfer-Encoding (e.g. "quoted-printable")
        /// </summary>
        string TransferEncoding { get; set; }
    }
}
