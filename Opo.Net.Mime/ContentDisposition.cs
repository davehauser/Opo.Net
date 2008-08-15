using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Opo.Net.Mime
{
    public class ContentDisposition
    {
        public string DispositionType { get; set; }
        public string FileName { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }
        public DateTime ReadDate { get; set; }
        public long Size { get; set; }
        public IDictionary<string, string> Parameters { get; set; }

        public ContentDisposition()
        {
            Parameters = new Dictionary<string, string>();
        }

        public ContentDisposition(string dispositionType, string fileName, DateTime creationDate, DateTime modificationDate, DateTime readDate, long size)
            : this()
        {
            DispositionType = dispositionType;
            FileName = fileName;
            CreationDate = creationDate;
            ModificationDate = modificationDate;
            ReadDate = readDate;
            Size = size;
        }
    }
}
