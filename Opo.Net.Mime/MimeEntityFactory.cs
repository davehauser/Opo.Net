using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Opo.Net.Mime
{
    public static class MimeEntityFactory
    {
        public static IMimeEntity GetInstance(IMimeParser mimeParser, string contentType)
        {
            IMimeEntity mimeEntity;
            string mainType = (contentType.Contains("/")) ? contentType.Substring(0, contentType.IndexOf("/")) : contentType;
            switch (mainType)
            {
                case "text":
                    mimeEntity = new TextMimeEntity(mimeParser, contentType);
                    break;
                default:
                    mimeEntity = new AttachmentMimeEntity(mimeParser, contentType);
                    break;
            }
            return mimeEntity;
        }
    }
}
