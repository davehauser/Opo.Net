using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Opo.Net;

namespace Opo.Net.Mime
{
    public interface IMimeEntity
    {
        string ContentType { get; set; }
        string TransferEncoding { get; set; }
        string ContentDisposition { get; set; }
        Dictionary<string,string> Headers { get; set; }
        string MimeData { get; set; }
        List<IMimeEntity> Entities { get; set; }

        bool HasEntities { get; }

        string GetHeaderValue(string headerName);
    }
}
