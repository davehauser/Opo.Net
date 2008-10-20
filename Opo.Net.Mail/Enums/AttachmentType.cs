using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Opo.Net.Mail
{
    /// <summary>
    /// Specifies how the content of an attachment is stored
    /// </summary>
    public enum AttachmentType
    {
        /// <summary>
        /// The content of the attachment is stored as string
        /// </summary>
        String,
        /// <summary>
        /// The content of the attachment is storead as a reference to a file
        /// </summary>
        File,
        /// <summary>
        /// The content  is passed to the attachment as a stream
        /// </summary>
        Stream
    }
}
