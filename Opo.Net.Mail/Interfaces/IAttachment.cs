using System.IO;
using Opo.Net.Mime;

namespace Opo.Net.Mail
{
    public interface IAttachment
    {
        long Size { get; set; }
        string ContentType { get; set; }
        string Name { get; set; }
        string TransferEncoding { get; set; }
        ContentDisposition ContentDisposition { get; set; }

        Stream GetContentStream();
    }
}
