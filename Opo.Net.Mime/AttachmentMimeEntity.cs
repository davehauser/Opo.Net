using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Opo.Net.Mime
{
    public class AttachmentMimeEntity : MimeEntityBase, IMimeEntity
    {
        public string ContentTransferEncoding { get; set; }

        public AttachmentMimeEntity(IMimeParser mimeParser, string mimeData)
            : base(mimeParser, mimeData)
        {
            ContentTransferEncoding = mimeParser.ParseHeader(MimeData, "Content-Transfer-Encoding");
        }
        public AttachmentMimeEntity(IMimeParser mimeParser, string mimeData, string contentType)
            : base(mimeParser, mimeData, contentType)
        {
            ContentTransferEncoding = mimeParser.ParseHeader(MimeData, "Content-Transfer-Encoding");
        }

        public Stream GetContent()
        {
            return MimeEncoding.Base64.Decode(_mimeParser.ParseContent(MimeData));
        }
    }
}
