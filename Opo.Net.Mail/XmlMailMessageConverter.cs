using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Opo.ProjectBase;
using Opo.Net.Mime;
using System.Globalization;
using System.Text.RegularExpressions;
using System.IO;
using System.Xml.Linq;
using System.Xml;

namespace Opo.Net.Mail
{
    /// <summary>
    /// Converts XML data to MailMessage and vice versa
    /// </summary>
    public class XmlMailMessageConverter : IMailMessageConverter
    {
        /// <summary>
        /// Initializes a new instance of the XmlMailMessageConverter class.
        /// </summary>
        public XmlMailMessageConverter() { }

        /// <summary>
        /// Converts XML data to MailMessage
        /// </summary>
        /// <param name="data">An string containing the XML data (String or XDocument)</param>
        /// <returns>A new instance of the <see cref="MailMessage">MailMessage</see> class</returns>
        public IMailMessage ConvertFrom(object data)
        {
            IMailMessage mailMessage;
            if (data is String)
            {
                mailMessage = ConvertFrom(data as String);
            }
            else if (data is XDocument)
            {
                mailMessage = ConvertFrom(data as XDocument);
            }
            else
            {
                throw new ArgumentException("Input type " + data.GetType().ToString() + " is not supported by XmlMailMessageConverter");
            }
            return mailMessage;
        }

        /// <summary>
        /// Converts XML string to MailMessage
        /// </summary>
        /// <param name="xmlData">A String containing the XML data</param>
        /// <returns>A new instance of the MailMessage class</returns>
        /// <exception cref="XmlException">Throws a XmlException if the input string contains no valid XML data.</exception>
        public IMailMessage ConvertFrom(string xmlData)
        {
            XDocument xDocument = new XDocument();
            try
            {
                xDocument = XDocument.Parse(xmlData);
            }
            catch (XmlException)
            {
                throw;
            }
            return ConvertFrom(xDocument);
        }
        /// <summary>
        /// Converts XDocument to MailMessage
        /// </summary>
        /// <param name="xmlData">A XDocument containing the xml mail data</param>
        /// <returns>A new instance of the MailMessage class</returns>
        public IMailMessage ConvertFrom(XDocument xmlData)
        {
            IMailMessage mailMessage = new MailMessage();

            // headers
            var headers = from h in xmlData.Element("headers").Elements("header")
                          select new MailHeader(h.Attribute("name").Value, h.Value);
            mailMessage.Headers.AddRange(headers);

            // email addresses
            mailMessage.From = GetMailAddress(xmlData, "from");
            mailMessage.To = GetMailAddressCollection(xmlData, "to");
            mailMessage.Cc = GetMailAddressCollection(xmlData, "cc");
            mailMessage.Bcc = GetMailAddressCollection(xmlData, "bcc");
            mailMessage.ReplyTo = GetMailAddress(xmlData, "replyTo");

            // message-id, subject, date etc.
            mailMessage.MessageID = xmlData.Element("messageID").Value;
            mailMessage.ReferenceIDs = (from r in xmlData.Element("referenceIDs").Elements("messageID")
                                        select r.Value).ToList();
            mailMessage.Subject = xmlData.Element("subject").Value;
            mailMessage.Date = xmlData.Element("date").Value.ToDate();
            mailMessage.Priority = xmlData.Element("priority").Value.ToEnum<MailPriority>(MailPriority.Normal);

            // body
            var views = from v in xmlData.Element("body").Elements("view")
                        select new
                        {
                            ContentType = v.Attribute("contentType").Value,
                            Charset = v.Attribute("charset").Value,
                            TransferEncoding = v.Attribute("transferEncoding").Value,
                            Content = v.Value
                        };
            bool bodySet = false;
            foreach (var view in views)
            {
                if (bodySet)
                {
                    IAlternativeView alternativeView = new AlternativeView(view.Content, view.ContentType);
                    alternativeView.Charset = view.Charset;
                    alternativeView.TransferEncoding = view.TransferEncoding;
                    mailMessage.AlternativeViews.Add(alternativeView);
                }
                else
                {
                    mailMessage.Body = view.Content;
                    mailMessage.BodyType = view.ContentType.ToEnum<MailMessageBodyType>(MailMessageBodyType.PlainText);
                    bodySet = true;
                }
            }

            // attachments
            var attachments = from a in xmlData.Element("attachments").Elements("attachment")
                              let contentDisp = a.Element("contentDisposition")
                              select new
                              {
                                  FilePath = a.Attribute("filePath").Value,
                                  Name = a.Element("name").Value,
                                  ContentType = a.Element("contentType").Value,
                                  ContentDisposition = new
                                  {
                                      Type = contentDisp.Element("type").Value,
                                      FileName = contentDisp.Element("fileName").Value,
                                      CreationDate = contentDisp.Element("creationDate").Value,
                                      ModificationDate = contentDisp.Element("modificationDate").Value,
                                      ReadDate = contentDisp.Element("readDate").Value,
                                      Size = contentDisp.Element("size").Value
                                  },
                                  Size = a.Element("size").Value,
                                  TransferEncoding = a.Element("transferEncoding").Value,
                                  FileName = a.Element("fileName").Value
                              };
            foreach (var a in attachments)
            {
                IAttachment attachment = new Attachment(a.FilePath);
                attachment.Name = a.Name;
                attachment.ContentType = a.ContentType;
                attachment.ContentDisposition.DispositionType = a.ContentDisposition.Type;
                attachment.ContentDisposition.FileName = a.ContentDisposition.FileName;
                attachment.ContentDisposition.CreationDate = a.ContentDisposition.CreationDate.ToDate();
                attachment.ContentDisposition.ModificationDate = a.ContentDisposition.ModificationDate.ToDate();
                attachment.ContentDisposition.ReadDate = a.ContentDisposition.ReadDate.ToDate();
                attachment.ContentDisposition.Size = a.ContentDisposition.Size.ToLong();
                attachment.Size = a.Size.ToLong();
                attachment.TransferEncoding = a.TransferEncoding;
                mailMessage.Attachments.Add(attachment);
            }

            return mailMessage;
        }

        private IMailAddress GetMailAddress(XDocument xmlData, string type)
        {
            IMailAddress mailAddress = null;
            XElement xmlAddress = xmlData.Element(type).Element("mailAddress");
            if (xmlAddress != null)
            {
                mailAddress = new MailAddress(xmlAddress.Element("address").Value);
                if (xmlAddress.Element("displayName") != null)
                {
                    mailAddress.DisplayName = xmlAddress.Element("displayName").Value;
                }
            }
            return mailAddress;
        }
        private MailAddressCollection GetMailAddressCollection(XDocument xmlData, string type)
        {
            MailAddressCollection mailAddresses = new MailAddressCollection();
            foreach (var mailAddress in xmlData.Element(type).Elements("mailAddress"))
            {
                mailAddresses.Add(new MailAddress(
                    mailAddress.Element("address").Value,
                    (mailAddress.Element("displayName") != null) ? mailAddress.Element("displayName").Value : ""
                ));
            }
            return mailAddresses;
        }

        /// <summary>
        /// Converts an IMailMessage to XML data
        /// </summary>
        /// <param name="mailMessage">IMailMessage to convert</param>
        /// <returns>A string containing the XML data</returns>
        public object ConvertTo(IMailMessage mailMessage)
        {
            // reference ids
            XElement xmlReferenceIDs = new XElement("referenceIDs");
            foreach (string referenceID in mailMessage.ReferenceIDs)
            {
                xmlReferenceIDs.Add(new XElement("messageID", referenceID));
            }

            // headers
            string[] doNotAddToHeaders = new string[] { "from", "to", "cc", "bcc", "reply-to", "subject", "date", "x-priority" };
            XElement xmlHeaders = new XElement("headers");
            foreach (MailHeader header in mailMessage.Headers)
            {
                if (!doNotAddToHeaders.Contains(header.Name.ToLower()))
                {
                    xmlHeaders.Add(new XElement("header", new XAttribute("name", header.Name), header.Value));
                }
            }

            // body
            XElement xmlBody = new XElement("body");
            xmlBody.Add(new XElement("view",
                new XAttribute("contentType", mailMessage.BodyType),
                mailMessage.Body));
            foreach (IAlternativeView view in mailMessage.AlternativeViews)
            {
                xmlBody.Add(new XElement("view",
                    new XAttribute("contentType", view.ContentType),
                    new XAttribute("charset", view.Charset),
                    new XAttribute("transferEncoding", view.TransferEncoding),
                    view.Content));
            }

            // attachments
            XElement xmlAttachments = new XElement("attachments");
            foreach (IAttachment attachment in mailMessage.Attachments)
            {
                xmlAttachments.Add(new XElement("attachment",
                    new XElement("name", attachment.Name),
                    new XElement("contentType", attachment.ContentType),
                    new XElement("contentDisposition",
                        new XElement("type", attachment.ContentDisposition.DispositionType),
                        new XElement("fileName", attachment.ContentDisposition.FileName),
                        new XElement("creationDate", attachment.ContentDisposition.CreationDate),
                        new XElement("modificationDate", attachment.ContentDisposition.ModificationDate),
                        new XElement("readDate", attachment.ContentDisposition.ReadDate),
                        new XElement("size", attachment.ContentDisposition.Size)
                    ),
                    new XElement("size", attachment.Size),
                    new XElement("transferEncoding", attachment.TransferEncoding),
                    new XElement("fileName")
                ));
            }


            // build xml message
            XElement xmlMessage = new XElement("mailMessage",
                new XElement("messageID", mailMessage.MessageID),
                xmlReferenceIDs,
                new XElement("from", GetXmlMailAddress(mailMessage.From)),
                new XElement("to", GetXmlMailAddresses(mailMessage.To)),
                new XElement("cc", GetXmlMailAddresses(mailMessage.Cc)),
                new XElement("bcc", GetXmlMailAddresses(mailMessage.Bcc)),
                new XElement("replyTo", GetXmlMailAddress(mailMessage.ReplyTo)),
                new XElement("subject", mailMessage.Subject),
                new XElement("date", mailMessage.Date),
                new XElement("priority", mailMessage.Priority),
                xmlHeaders,
                xmlBody,
                xmlAttachments
            );

            return new XDocument(new XDeclaration("1.0", "utf-8", "yes"), xmlMessage);
        }

        private XElement GetXmlMailAddress(IMailAddress mailAddress)
        {
            XElement xmlAddress = null;
            if (mailAddress != null)
            {
                xmlAddress = new XElement("mailAddress");
                xmlAddress.Add(
                    new XElement("address", mailAddress.Address),
                    new XElement("displayName", mailAddress.DisplayName)
                );
            }
            return xmlAddress;
        }
        private XElement[] GetXmlMailAddresses(MailAddressCollection mailAddressCollection)
        {
            List<XElement> xmlAddresses = new List<XElement>();
            foreach (IMailAddress mailAddress in mailAddressCollection)
            {
                xmlAddresses.Add(GetXmlMailAddress(mailAddress));
            }
            return xmlAddresses.ToArray();
        }

        /// <summary>
        /// Loads an XML file to a IMailMessage
        /// </summary>
        /// <param name="filePath">Absolute path to the XML file</param>
        /// <returns>A new instance of the MailMessage class</returns>
        public IMailMessage LoadFromFile(string filePath)
        {
            if (!File.Exists(filePath))
                throw new IOException("File not found: " + filePath);

            StreamReader mimeReader = new StreamReader(File.OpenRead(filePath));
            return ConvertFrom(mimeReader.ReadToEnd());
        }

        /// <summary>
        /// Saves a IMailMessage to a XML file
        /// </summary>
        /// <param name="mailMessage">IMailMessage to convert</param>
        /// <param name="path">Absolute path where the MIME file is saved</param>
        /// <returns>A String containing the generated filename of the XML file</returns>
        public string SaveToFile(IMailMessage message, string path)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Saves a IMailMessage to a XML file
        /// </summary>
        /// <param name="mailMessage">IMailMessage to convert</param>
        /// <param name="path">Absolute path where the XML file is saved</param>
        /// <param name="fileName">Filename for the XML file</param>
        /// <returns>A String containing the filename of the XML file</returns>
        public string SaveToFile(IMailMessage message, string path, string fileName)
        {
            throw new NotImplementedException();
        }
    }
}
