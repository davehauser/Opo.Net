using System.Text;

namespace Opo.Net.Mail
{
    public interface IAlternativeView
    {
        string Content { get; set; }
        string ContentType { get; set; }
        string Charset { get; set; }
        string TransferEncoding { get; set; }
    }
}
