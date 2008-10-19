using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Opo.Net.Mime
{
    /// <summary>
    /// Base class for IMimeEntity classes
    /// </summary>
    public abstract class MimeEntityBase : IMimeEntity
    {
        protected IMimeParser _mimeParser;
        private MimeEntityCollection _entities;

        /// <summary>
        /// Gets or sets the Content-Type
        /// </summary>
        public string ContentType { get; set; }
        /// <summary>
        /// Gets or sets the Transfer-Encoding
        /// </summary>
        public string TransferEncoding { get; set; }
        /// <summary>
        /// Gets or sets the Content-Disposition
        /// </summary>
        public string ContentDisposition { get; set; }
        /// <summary>
        /// Collection of MIME headers
        /// </summary>
        public IDictionary<string, string> Headers { get; set; }
        /// <summary>
        /// Gets or sets the MIME data
        /// </summary>
        public abstract string MimeData { get; set; }
        /// <summary>
        /// Collection of child MimeEntities
        /// </summary>
        public MimeEntityCollection Entities
        {
            get
            {
                if (_entities == null)
                    _entities = new MimeEntityCollection();
                return _entities;
            }
            set
            {
                _entities = value;
            }
        }
        /// <summary>
        /// Gets a value indicating whether there are any items in the Entities collection
        /// </summary>
        public bool HasEntities { get { return false; } }

        /// <summary>
        /// Initializes a new instance of the MimeEntityBase class
        /// </summary>
        /// <param name="mimeParser">An IMimeParser instance which is used for parsing the MIME data</param>
        /// <param name="mimeData">A string containing the MIME data for the MIME entity</param>
        public MimeEntityBase(IMimeParser mimeParser, string mimeData)
        {
            _mimeParser = mimeParser;
            MimeData = mimeData;
            ContentType = mimeParser.ParseContentType(mimeData);
        }

        /// <summary>
        /// Get the value of a MIME header
        /// </summary>
        /// <param name="headerName">The name of the header (e.g. "Content-Type")</param>
        /// <returns>A String containing the value of the header or an empty string if the header is not present</returns>
        public string GetHeaderValue(string headerName)
        {
            return Headers.FirstOrDefault(h => h.Key == headerName).Value;
        }
    }
}
