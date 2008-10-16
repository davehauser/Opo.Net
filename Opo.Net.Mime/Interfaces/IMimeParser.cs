using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Opo.Net.Mime
{
    public interface IMimeParser
    {
        string ParseFrom(string mimeData);
        string[] ParseTo(string mimeData);
        string[] ParseCC(string mimeData);
        string[] ParseBCC(string mimeData);
        string ParseSubject(string mimeData);
        string ParseDate(string mimeData);
        string ParseMimeVersion(string mimeData);
        string ParseContentType(string mimeData);
        string ParseBoundary(string mimeData);
        string ParsePriority(string mimeData);
        string ParseContent(string mimeData);
        string ParseHeader(string mimeData, string headerName); 
    }
}
