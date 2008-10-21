using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml.Linq;
using Opo.Net.Mime;

namespace Opo.Net.Mail
{
    /// <summary>
    /// Represents a mail message
    /// </summary>
    public class MailMessage : IMailMessage
    {
        /// <summary>
        /// Gets or sets the MessageID
        /// </summary>
        public string MessageID { get; set; }
        /// <summary>
        /// Gets or sets the ReferenceIDs. ReferenceIDs contains MessageIDs of related Messages which then can be used to group messages to conversations
        /// </summary>
        public List<string> ReferenceIDs { get; set; }
        /// <summary>
        /// Gets or sets a mail server specific unique ID
        /// </summary>
        public string UID { get; set; }
        /// <summary>
        /// Gets or set the Subject
        /// </summary>
        public string Subject { get; set; }
        /// <summary>
        /// Gets or sets the from address for this mail message
        /// </summary>
        public IMailAddress From { get; set; }
        /// <summary>
        /// Gets the address collection that contains the recipients of this e-mail message
        /// </summary>
        public MailAddressCollection To { get; set; }
        /// <summary>
        /// Gets or sets the address collection that contains the carbon copy (CC) recipients for this e-mail message
        /// </summary>
        public MailAddressCollection CC { get; set; }
        /// <summary>
        /// Gets or sets the address collection that contains the blind carbon copy (BCC) recipients for this e-mail message
        /// </summary>
        public MailAddressCollection Bcc { get; set; }
        /// <summary>
        /// Gets or sets the ReplyTo address for the mail message
        /// </summary>
        public MailAddress ReplyTo { get; set; }
        /// <summary>
        /// Gets or sets the message body
        /// </summary>
        public string Body { get; set; }
        /// <summary>
        /// Gets or sets the body type (e.g. html, text)
        /// </summary>
        public MailMessageBodyType BodyType { get; set; }
        /// <summary>
        /// Gets or sets the mail priority
        /// </summary>
        public MailPriority Priority { get; set; }
        /// <summary>
        /// Gets or sets a collection that contains headers of this mail message
        /// </summary>
        public MailHeaderCollection Headers { get; set; }
        /// <summary>
        /// Gets a value indicating whether the message contains attachments
        /// </summary>
        public bool HasAttachments { get { return (this.Attachments.Count > 0); } }
        /// <summary>
        /// Gets or sets the Date of the mail message
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// Gets or sets the size of the message in bytes (including attachments)
        /// </summary>
        public long Size { get; set; }
        /// <summary>
        /// Gets or sets a collection that contains the messages attachments
        /// </summary>
        public AttachmentCollection Attachments { get; set; }
        /// <summary>
        /// Gets or sets a collection that contains alternative views for the message body
        /// </summary>
        public AlternativeViewCollection AlternateViews { get; set; }

        /// <summary>
        /// Initializes a new instance of the MailMessage class
        /// </summary>
        public MailMessage()
        {
            ReferenceIDs = new List<string>();
            To = new MailAddressCollection();
            CC = new MailAddressCollection();
            Bcc = new MailAddressCollection();
            Headers = new MailHeaderCollection();
            Attachments = new AttachmentCollection();
            AlternateViews = new AlternativeViewCollection();
            BodyType = MailMessageBodyType.Html;
        }

        /// <summary>
        /// Initializes a new instance of the MailMessage class with some additional information
        /// </summary>
        /// <param name="from">IMailAddress representing the senders mail address</param>
        /// <param name="to">IMailAddress representing the recipients mail address</param>
        /// <param name="subject">Subject of the mail message</param>
        /// <param name="body">Body message body</param>
        /// <param name="type">Type of the message body</param>
        public MailMessage(IMailAddress from, IMailAddress to, string subject, string body, MailMessageBodyType type)
            : this()
        {
            From = from;
            To.Add(to);
            Subject = subject;
            Body = body;
            BodyType = type;
        }
    }
}