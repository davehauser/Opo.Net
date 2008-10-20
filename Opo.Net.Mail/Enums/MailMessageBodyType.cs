using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Opo.Net.Mail
{
    /// <summary>
    /// Specifies the priority of a mail message
    /// </summary>
    public enum MailMessageBodyType
    {
        /// <summary>
        /// Type of the body content is HTML
        /// </summary>
        Html,
        /// <summary>
        /// Type of the body content is plain text
        /// </summary>
        PlainText
    }
}
