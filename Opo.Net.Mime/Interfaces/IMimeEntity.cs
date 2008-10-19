﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Opo.Net;

namespace Opo.Net.Mime
{
    public interface IMimeEntity
    {
        /// <summary>
        /// Gets or sets the Content-Type
        /// </summary>
        string ContentType { get; set; }
        /// <summary>
        /// Gets or sets the Transfer-Encoding
        /// </summary>
        string TransferEncoding { get; set; }
        /// <summary>
        /// Gets or sets the Content-Disposition
        /// </summary>
        string ContentDisposition { get; set; }
        /// <summary>
        /// Collection of MIME headers
        /// </summary>
        IDictionary<string, string> Headers { get; set; }
        /// <summary>
        /// Gets or sets the MIME data
        /// </summary>
        string MimeData { get; set; }
        /// <summary>
        /// Collection of child MimeEntities
        /// </summary>
        MimeEntityCollection Entities { get; set; }
        /// <summary>
        /// Gets a value indicating whether there are any items in the Entities collection
        /// </summary>
        bool HasEntities { get; }

        /// <summary>
        /// Get the value of a MIME header
        /// </summary>
        /// <param name="headerName">The name of the header (e.g. "Content-Type")</param>
        /// <returns>A String containing the value of the header or an empty string if the header is not present</returns>
        string GetHeaderValue(string headerName);
    }
}