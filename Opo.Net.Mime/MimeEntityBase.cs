using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Opo.Net.Mime
{
    public class MimeEntityBase : IMimeEntity
    {
        protected IMimeParser _mimeParser;
        
        public string ContentType { get; set; }
        public string TransferEncoding { get; set; }
        public string ContentDisposition { get; set; }
        public Dictionary<string, string> Headers { get; set; }
        public string MimeData { get; set; }
        public List<IMimeEntity> Entities { get; set; }
        public bool HasEntities { get { return Entities.Count > 0; } }

        public MimeEntityBase(IMimeParser mimeParser, string mimeData)
        {
            _mimeParser = mimeParser;
            MimeData = mimeData;
            ContentType = mimeParser.ParseContentType(mimeData);
        }
        public MimeEntityBase(IMimeParser mimeParser, string mimeData, string contentType)
        {
            _mimeParser = mimeParser;
            MimeData = mimeData;
            ContentType = contentType;
        }

        public string GetHeaderValue(string headerName)
        {
            return Headers.FirstOrDefault(h => h.Key == headerName).Value;
        }
    }
}
