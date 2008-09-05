using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml.Linq;
using Opo.Net.Mime;

namespace Opo.Net.Mail
{
    public class MailMessage : IMailMessage
    {
        public string MessageID { get; set; }
        public List<string> ReferenceIDs { get; set; }
        public string UID { get; set; }
        public string Subject { get; set; }
        public IMailAddress From { get; set; }
        public MailAddressCollection To { get; set; }
        public MailAddressCollection CC { get; set; }
        public MailAddressCollection Bcc { get; set; }
        public string Body { get; set; }
        public MailPriority Priority { get; set; }
        public MailHeaderCollection Headers { get; set; }
        public MailAddress ReplyTo { get; set; }
        public MailMessageBodyType BodyType { get; set; }
        public bool HasAttachments { get { return (this.Attachments.Count > 0); } }

        public DateTime Date { get; set; }
        public int Size { get; set; }
        public AttachmentCollection Attachments { get; set; }
        public AlternativeViewCollection AlternateViews { get; set; }

        public MailMessage()
        {
            ReferenceIDs = new List<string>();
            To = new MailAddressCollection();
            CC = new MailAddressCollection();
            Bcc = new MailAddressCollection();
            Headers = new MailHeaderCollection();
            Attachments = new AttachmentCollection();
            AlternateViews = new AlternativeViewCollection();
            BodyType = MailMessageBodyType.Default;
        }

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