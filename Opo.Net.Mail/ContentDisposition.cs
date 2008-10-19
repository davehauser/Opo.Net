using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Opo.Net.Mail
{
    /// <summary>
    /// Represents a MIME Content-Disposition header
    /// </summary>
    public class ContentDisposition
    {
        private IDictionary<string, string> _parameters;
        
        /// <summary>
        /// Gets or sets the Disposition-Type
        /// </summary>
        public string DispositionType { get; set; }
        /// <summary>
        /// Gets or sets the filename
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// Gets or sets the Creation-Date
        /// </summary>
        public DateTime CreationDate { get; set; }
        /// <summary>
        /// Gets or sets the Modification-Date
        /// </summary>
        public DateTime ModificationDate { get; set; }
        /// <summary>
        /// Gets or sets the Read-Date
        /// </summary>
        public DateTime ReadDate { get; set; }
        /// <summary>
        /// Gets or sets the Size
        /// </summary>
        public long Size { get; set; }
        /// <summary>
        /// Gets or sets a collection of additional parameters
        /// </summary>
        public IDictionary<string, string> Parameters
        {
            get
            {
                if (_parameters == null)
                    _parameters = new Dictionary<string, string>();
                return _parameters;
            }
            set
            {
                _parameters = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the ContentDisposition class with the <see cref="DispositionType">DispositionType</see> set to "attachment"
        /// </summary>
        public ContentDisposition()
        {
            DispositionType = "attachment";
        }

        /// <summary>
        /// Initiazlizes a new instance of the ContentDisposition class with the specified information
        /// </summary>
        /// <param name="dispositionType">A string containing the Disposition-Type</param>
        /// <param name="fileName">A string containing the filename</param>
        /// <param name="creationDate">A DateTime containing the Creation-Date</param>
        /// <param name="size">A Int64 specifying the Size</param>
        public ContentDisposition(string dispositionType, string fileName, DateTime creationDate, long size)
            : this()
        {
            DispositionType = dispositionType;
            FileName = fileName;
            CreationDate = creationDate;
            Size = size;
        }
    }
}
