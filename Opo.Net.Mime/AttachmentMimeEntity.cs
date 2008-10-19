using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Opo.Net.Mime
{
    public class AttachmentMimeEntity : MimeEntityBase, IMimeEntity
    {
        /// <summary>
        /// Gets or sets the Content-Transfer-Encoding of the attachment
        /// </summary>
        public string ContentTransferEncoding { get; set; }
        /// <summary>
        /// Gets or sets the MIME data of the attachment
        /// </summary>
        public override string MimeData { get; set; }

        /// <summary>
        /// Initializes a new instance of the AttachmentMimeEntity class
        /// </summary>
        /// <param name="mimeParser">An IMimeParser instance which is used for parsing the MIME data</param>
        /// <param name="mimeData">A string containing the MIME data of the attachment</param>
        public AttachmentMimeEntity(IMimeParser mimeParser, string mimeData)
            : base(mimeParser, mimeData)
        {
            ContentTransferEncoding = mimeParser.ParseHeader(MimeData, "Content-Transfer-Encoding");
        }

        /// <summary>
        /// Returns the decoded content of the attachment as a Stream
        /// </summary>
        /// <returns>A Stream containing the decoded attachment</returns>
        public Stream GetContent()
        {
            return MimeEncoding.Base64.Decode(_mimeParser.ParseContent(MimeData));
        }
    }
}
