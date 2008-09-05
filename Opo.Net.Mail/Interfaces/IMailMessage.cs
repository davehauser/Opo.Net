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
        string MessageID { get; set; }
        List<string> ReferenceIDs { get; set; }
        string UID { get; set; }
        string Subject { get; set; }
        IMailAddress From { get; set; }
        MailAddressCollection To { get; set; }
        MailAddressCollection CC { get; set; }
        MailAddressCollection Bcc { get; set; }
        string Body { get; set; }
        MailPriority Priority { get; set; }
        MailHeaderCollection Headers { get; set; }
        MailAddress ReplyTo { get; set; }
        MailMessageBodyType BodyType { get; set; }
        bool HasAttachments { get ; }

        DateTime Date { get; set; }
        int Size { get; set; }
        AttachmentCollection Attachments { get; set; }
        AlternativeViewCollection AlternateViews { get; set; }
    }
}