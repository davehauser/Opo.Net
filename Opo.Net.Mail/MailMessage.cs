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
        private MailHeaderCollection _headers;

        /// <summary>
        /// Gets or sets the MessageID
        /// </summary>
        public string MessageID
        {
            get
            {
                return Headers.GetValue("Message-ID");
            }
            set
            {
                Headers.Add("Message-ID", value);
            }
        }
        /// <summary>
        /// Gets or sets the ReferenceIDs. ReferenceIDs contains MessageIDs of related Messages which then can be used to group messages to conversations
        /// </summary>
        public List<string> ReferenceIDs
        {
            get
            {
                string references = Headers.GetValue("References");
                if (!String.IsNullOrEmpty(references))
                {
                    return (from r in references.Split(',')
                            select r.Trim()).ToList();
                }
                else
                {
                    return new List<string>();
                }
            }
            set
            {
                if (value.Count > 0)
                {
                    Headers.Add("References", String.Join(",", value.ToArray()));
                }
            }
        }

        /// <summary>
        /// Gets or set the Subject
        /// </summary>
        public string Subject
        {
            get
            {
                return Headers.GetValue("Subject");
            }
            set
            {
                Headers.Add("Subject", value);
            }
        }
        /// <summary>
        /// Gets or sets the from address for this mail message
        /// </summary>
        public IMailAddress From
        {
            get
            {
                IMailAddress mailAddress;
                try
                {
                    mailAddress = MailAddress.Parse(Headers.GetValue("From"));
                }
                catch (Exception)
                {
                    mailAddress = null;
                }
                return mailAddress;
            }
            set
            {
                Headers.Add("From", value.ToString());
            }
        }
        /// <summary>
        /// Gets the address collection that contains the recipients of this e-mail message
        /// </summary>
        public MailAddressCollection To
        {
            get
            {
                return MailAddressCollection.Parse(Headers.GetValue("To"));
            }
            set
            {
                Headers.Add("To", value.ToString());
            }
        }
        /// <summary>
        /// Gets or sets the address collection that contains the carbon copy (CC) recipients for this e-mail message
        /// </summary>
        public MailAddressCollection Cc
        {
            get
            {
                return MailAddressCollection.Parse(Headers.GetValue("Cc"));
            }
            set
            {
                Headers.Add("Cc", value.ToString());
            }
        }
        /// <summary>
        /// Gets or sets the address collection that contains the blind carbon copy (BCC) recipients for this e-mail message
        /// </summary>
        public MailAddressCollection Bcc
        {
            get
            {
                return MailAddressCollection.Parse(Headers.GetValue("Bcc"));
            }
            set
            {
                Headers.Add("Bcc", value.ToString());
            }
        }
        /// <summary>
        /// Gets or sets the ReplyTo address for the mail message
        /// </summary>
        public IMailAddress ReplyTo
        {
            get
            {
                IMailAddress mailAddress;
                try
                {
                    mailAddress = MailAddress.Parse(Headers.GetValue("Reply-To"));
                }
                catch (Exception)
                {
                    mailAddress = null;
                }
                return mailAddress;
            }
            set
            {
                Headers.Add("Reply-To", value.ToString());
            }
        }
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
        public MailPriority Priority
        {
            get
            {
                MailPriority priority;
                try
                {
                    priority = (MailPriority)Enum.Parse(typeof(MailPriority), Headers.GetValue("X-Priority"));
                }
                catch (Exception)
                {
                    priority = MailPriority.Normal;
                }
                return priority;
            }
            set
            {
                Headers.Add("X-Priority", value.ToString());
            }
        }
        /// <summary>
        /// Gets or sets a collection that contains headers of this mail message
        /// </summary>
        public MailHeaderCollection Headers
        {
            get
            {
                if (_headers == null)
                    _headers = new MailHeaderCollection();
                return _headers;
            }
            set
            {
                _headers = value;
            }
        }
        /// <summary>
        /// Gets a value indicating whether the message contains attachments
        /// </summary>
        public bool HasAttachments { get { return (this.Attachments.Count > 0); } }
        /// <summary>
        /// Gets or sets the Date of the mail message
        /// </summary>
        public DateTime Date
        {
            get
            {
                DateTime dateTime = DateTime.MinValue;
                string date = Headers.GetValue("Date");
                if (!String.IsNullOrEmpty(date))
                {
                    dateTime = MimeUtilities.ParseRfc2822Date(date);
                }
                return dateTime;
            }
            set
            {
                Headers.Add("Date", value.ToRfc2822Date());
            }
        }
        /// <summary>
        /// Gets or sets the size of the message in bytes (including attachments)
        /// </summary>
        public long Size
        {
            get
            {
                long size = 0;
                Int64.TryParse(Headers.GetValue("Size"), out size);
                return size;
            }
            set
            {
                Headers.Add("Size", value.ToString());
            }
        }
        /// <summary>
        /// Gets or sets a collection that contains the messages attachments
        /// </summary>
        public AttachmentCollection Attachments { get; set; }
        /// <summary>
        /// Gets or sets a collection that contains alternative views for the message body
        /// </summary>
        public AlternativeViewCollection AlternativeViews { get; set; }

        /// <summary>
        /// Initializes a new instance of the MailMessage class
        /// </summary>
        public MailMessage()
        {
            ReferenceIDs = new List<string>();
            To = new MailAddressCollection();
            Cc = new MailAddressCollection();
            Bcc = new MailAddressCollection();
            //Headers = new MailHeaderCollection();
            Attachments = new AttachmentCollection();
            AlternativeViews = new AlternativeViewCollection();
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