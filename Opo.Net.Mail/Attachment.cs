using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Opo.Net.Mime;

namespace Opo.Net.Mail
{
    public class Attachment : IAttachment
    {
        private string _content;

        public string Name { get; set; }
        public string ContentType { get; set; }
        public long Size { get; set; }
        public string TransferEncoding { get; set; }
        public ContentDisposition ContentDisposition { get; set; }

        public Attachment(string name, string content, string contentType, string transferEncoding)
        {
            Name = name;
            _content = content;
            ContentType = contentType;
            TransferEncoding = transferEncoding;

            Size = Encoding.UTF8.GetByteCount(_content);

            ContentDisposition = new ContentDisposition();
            ContentDisposition.DispositionType = "attachment";
            ContentDisposition.Size = Size;
            ContentDisposition.FileName = name;
        }

        public Stream GetContentStream()
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(_content));
        }
    }
}
