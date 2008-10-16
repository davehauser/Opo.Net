using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Opo.Net.Mime
{
    public class TextMimeEntity : MimeEntityBase, IMimeEntity
    {
        public TextMimeEntity(IMimeParser mimeParser, string mimeData)
            : base(mimeParser, mimeData) { }
        public TextMimeEntity(IMimeParser mimeParser, string mimeData, string contentType)
            : base(mimeParser, mimeData, contentType) { }

        public string GetContent()
        {
            return _mimeParser.ParseContent(MimeData);
        }
    }
}
