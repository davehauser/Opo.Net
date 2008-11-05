using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml.Linq;
using Opo.Net.Mime;

namespace Opo.Net.Mail
{
    public interface IMailMessage
    {
        /// <summary>
        /// Gets or sets the MessageID
        /// </summary>
        string MessageID { get; set; }
        /// <summary>
        /// Gets or sets the ReferenceIDs. ReferenceIDs contains MessageIDs of related Messages which then can be used to group messages to conversations
        /// </summary>
        List<string> ReferenceIDs { get; set; }
        /// <summary>
        /// Gets or set the Subject
        /// </summary>
        string Subject { get; set; }
        /// <summary>
        /// Gets or sets the from address for this mail message
        /// </summary>
        IMailAddress From { get; set; }
        /// <summary>
        /// Gets the address collection that contains the recipients of this e-mail message
        /// </summary>
        MailAddressCollection To { get; set; }
        /// <summary>
        /// Gets or sets the address collection that contains the carbon copy (CC) recipients for this e-mail message
        /// </summary>
        MailAddressCollection Cc { get; set; }
        /// <summary>
        /// Gets or sets the address collection that contains the blind carbon copy (BCC) recipients for this e-mail message
        /// </summary>
        MailAddressCollection Bcc { get; set; }
        /// <summary>
        /// Gets or sets the ReplyTo address for the mail message
        /// </summary>
        IMailAddress ReplyTo { get; set; }
        /// <summary>
        /// Gets or sets the message body
        /// </summary>
        string Body { get; set; }
        /// <summary>
        /// Gets or sets the body type (e.g. html, text)
        /// </summary>
        MailMessageBodyType BodyType { get; set; }
        /// <summary>
        /// Gets or sets the mail priority
        /// </summary>
        MailPriority Priority { get; set; }
        /// <summary>
        /// Gets or sets a collection that contains headers of this mail message
        /// </summary>
        MailHeaderCollection Headers { get; set; }
        /// <summary>
        /// Gets a value indicating whether the message contains attachments
        /// </summary>
        bool HasAttachments { get; }
        /// <summary>
        /// Gets or sets the Date of the mail message
        /// </summary>
        DateTime Date { get; set; }
        /// <summary>
        /// Gets or sets the size of the message in bytes (including attachments)
        /// </summary>
        long Size { get; set; }
        /// <summary>
        /// Gets or sets a collection that contains the messages attachments
        /// </summary>
        AttachmentCollection Attachments { get; set; }
        /// <summary>
        /// Gets or sets a collection that contains alternative views for the message body
        /// </summary>
        AlternativeViewCollection AlternativeViews { get; set; }
    }
}