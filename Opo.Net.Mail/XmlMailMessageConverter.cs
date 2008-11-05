using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;
using Opo.Net.Mime;
using System.Xml;

namespace Opo.Net.Mail
{
    /// <summary>
    /// Converts a Opo.Net.Mail.MailMessage to Xml and vice versa
    /// </summary>
    public class XmlMailMessageConverter : IMailMessageConverter
    {
        /// <summary>
        /// Converts XML data to MailMessage
        /// </summary>
        /// <param name="data">An XElement or a String containing the XML data</param>
        /// <returns>A new instance of the MailMessage class</returns>
        public IMailMessage ConvertFrom(object data)
        {
            XElement xmlMessage = data as XElement;
            if (xmlMessage == null)
            {
                if (data is string)
                    xmlMessage = XElement.Parse(data as string);
                else
                    return null;
            }

            IMailMessage message = new MailMessage();

            if (xmlMessage == null)
                return new MailMessage();
            message.MessageID = xmlMessage.Element("MessageID").Value;
            message.From = MailAddressFromXml(xmlMessage.Descendants("From").FirstOrDefault());
            message.To = MailAddressListFromXml(xmlMessage.Descendants("To").Elements().ToArray());
            message.Cc = MailAddressListFromXml(xmlMessage.Descendants("CC").Elements().ToArray());
            message.Bcc = MailAddressListFromXml(xmlMessage.Descendants("Bcc").Elements().ToArray());
            message.Subject = xmlMessage.Descendants("Subject").FirstOrDefault().Value;
            message.ReferenceIDs = ReferenceIDsFromXml(xmlMessage.Descendants("Conversation").Elements().ToArray());
            message.Date = DateTime.Parse(xmlMessage.Descendants("Date").FirstOrDefault().Value);
            message.Priority = ((MailPriority)Enum.Parse(typeof(MailPriority), xmlMessage.Descendants("Priority").FirstOrDefault().Value));
            message.Headers = HeadersFromXml(xmlMessage.Descendants("Headers").FirstOrDefault());
            // body & alternate views
            var views = from v in xmlMessage.Descendants("Body").Descendants("View")
                        select v;
            if (views.Count() == 0)
            {
                message.Body = "";
            }
            else
            {
                var htmlBody = views.Where(v => v.Attribute("ContentType").Value == MediaType.Text.Html);
                var textBody = views.Where(v => v.Attribute("ContentType").Value == MediaType.Text.Plain);
                var otherViews = views.Where(v => v.Attribute("ContentType").Value != MediaType.Text.Html && v.Attribute("ContentType").Value != MediaType.Text.Plain);
                if (htmlBody.Count() > 0)
                {
                    message.Body = htmlBody.First().Value;
                    if (textBody.Count() > 0)
                    {
                        message.AlternativeViews.Add(AlternativeView.LoadXmlAlternativeView(textBody.First()));
                    }
                }
                else if (textBody.Count() > 0)
                {
                    message.Body = textBody.First().Value;
                }
                else
                {
                    message.Body = otherViews.First().Value;
                    otherViews.First().Remove();
                }
                foreach (XElement v in otherViews)
                {
                    message.AlternativeViews.Add(AlternativeView.LoadXmlAlternativeView(v));
                }
            }
            return message;
        }

        /// <summary>
        /// Converts an IMailMessage to XML data
        /// </summary>
        /// <param name="mailMessage">IMailMessage to convert</param>
        /// <returns>An XElement representing the IMailMessage</returns>
        public object ConvertTo(IMailMessage mailMessage)
        {
            // conversation ids
            XElement xmlConversation = new XElement("Conversation");
            foreach (string referenceID in mailMessage.ReferenceIDs)
            {
                xmlConversation.Add(new XElement("MessageID", referenceID));
            }
            // headers
            XElement xmlHeaders = new XElement("Headers");
            foreach (MailHeader header in mailMessage.Headers)
            {
                xmlHeaders.Add(new XElement(header.Name, header.Value));
            }
            // alternative views
            XElement xmlAlternativeViews = new XElement("Body");
            foreach (AlternativeView view in mailMessage.AlternativeViews)
            {
                xmlAlternativeViews.Add(new XElement("View",
                    new XAttribute("ContentType", view.ContentType ?? ""),
                    new XAttribute("TransferEncoding", view.TransferEncoding ?? ""),
                    new XAttribute("Charset", (view.Charset ?? "utf8")),
                    "<![CDATA[" + view.Content.HtmlEncode() + "]]>")
                    );
            }

            // build xml file
            XDocument xmlMailMessage = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XElement("MailMessage",
                    new XElement("MessageID", mailMessage.MessageID),
                    xmlConversation,
                    new XElement("From", MailAddressToXml(mailMessage.From)),
                    new XElement("To", MailAddressListToXml(mailMessage.To)),
                    new XElement("CC", MailAddressListToXml(mailMessage.Cc)),
                    new XElement("Bcc", MailAddressListToXml(mailMessage.Bcc)),
                    new XElement("Subject", mailMessage.Subject),
                    new XElement("Date", mailMessage.Date.ToString("r")),
                    new XElement("Priority", mailMessage.Priority.ToString()),
                    xmlHeaders,
                    xmlAlternativeViews
                    )
                );
            return xmlMailMessage;
        }

        /// <summary>
        /// Loads an XML file to a IMailMessage
        /// </summary>
        /// <param name="filePath">Absolute path to the xml file</param>
        /// <returns>A new instance of the MailMessage class</returns>
        public IMailMessage LoadFromFile(string filePath)
        {
            string mailPath = filePath.Substring(0, filePath.LastIndexOf('\\'));
            if (!File.Exists(filePath))
                throw new FileNotFoundException(String.Concat("[", filePath, "] not found."));
            XElement xmlMessage = XDocument.Load(filePath).Element("MailMessage");

            IMailMessage message = ConvertFrom(xmlMessage);

            // attachments
            var attachments = from v in xmlMessage.Descendants("Attachments").Descendants("Attachment")
                              select v;
            if (attachments.Count() > 0)
            {
                foreach (var attachment in attachments)
                {
                    Attachment a = new Attachment(Path.Combine(mailPath, attachment.Element("FileName").Value));
                    a.Name = attachment.Element("Name").Value;
                    a.ContentType = attachment.Element("ContentType").Value;
                    a.Size = int.Parse(attachment.Element("Size").Value);
                    a.TransferEncoding = attachment.Element("TransferEncoding").Value;
                    //var contentDisposition = attachment.Element("ContentDisposition");
                    //a.ContentDisposition.DispositionType = contentDisposition.Element("Type").Value;
                    //a.ContentDisposition.FileName = contentDisposition.Element("FileName").Value;
                    //a.ContentDisposition.CreationDate = DateTime.Parse(contentDisposition.Element("CreationDate").Value);
                    //a.ContentDisposition.ModificationDate = DateTime.Parse(contentDisposition.Element("ModificationDate").Value);
                    //a.ContentDisposition.ReadDate = DateTime.Parse(contentDisposition.Element("ReadDate").Value);
                    message.Attachments.Add(a);
                }
            }
            return message;
        }

        /// <summary>
        /// Saves a IMailMessage to a XML file
        /// </summary>
        /// <param name="mailMessage">IMailMessage to convert</param>
        /// <param name="path">Absolute path where the XML file is saved</param>
        /// <returns>A String containing the generated filename of the XML file</returns>
        public string SaveToFile(IMailMessage mailMessage, string path)
        {
            string fileName = String.Concat(mailMessage.MessageID, ".xml");
            return SaveToFile(mailMessage, path, fileName);
        }

        /// <summary>
        /// Saves a IMailMessage to a XML file
        /// </summary>
        /// <param name="mailMessage">IMailMessage to convert</param>
        /// <param name="path">Absolute path where the XML file is saved</param>
        /// <param name="fileName">Filename for the XML file</param>
        /// <returns>A String containing the filename of the XML file</returns>
        public string SaveToFile(IMailMessage mailMessage, string path, string fileName)
        {
            XDocument xmlMailMessage = ConvertTo(mailMessage) as XDocument;

            if (xmlMailMessage != null)
            {
                // attachments
                XElement xmlAttachments = new XElement("Attachments");
                foreach (Attachment attachment in mailMessage.Attachments)
                {

                    xmlAttachments.Add(new XElement("Attachment",
                        new XElement("Name", attachment.Name),
                        new XElement("ContentType", attachment.ContentType ?? ""),
                        new XElement("ContentDisposition",
                            new XElement("Type", attachment.ContentDisposition.DispositionType ?? ""),
                            new XElement("FileName", attachment.ContentDisposition.FileName ?? ""),
                            new XElement("CreationDate", attachment.ContentDisposition.CreationDate),
                            new XElement("ModificationDate", attachment.ContentDisposition.ModificationDate),
                            new XElement("ReadDate", attachment.ContentDisposition.ReadDate),
                            new XElement("Size", attachment.Size)
                            ),
                        new XElement("Size", attachment.Size),
                        new XElement("TransferEncoding", attachment.TransferEncoding),
                        new XElement("FileName", Path.GetFileName(attachment.SaveToFile(path, String.Concat(Path.GetFileNameWithoutExtension(fileName), "_", attachment.ContentDisposition.FileName))))
                        )
                    );
                }
                xmlMailMessage.Add(xmlAttachments);

                xmlMailMessage.Save(Path.Combine(path, fileName));
                return fileName;
            }
            else
            {
                return String.Empty;
            }
        }

        #region helper methods
        private static XElement MailAddressToXml(IMailAddress mailAddress)
        {
            XElement xmlAddress = new XElement("MailAddress",
                new XElement("Address", mailAddress.Address));
            if (mailAddress.DisplayName.IsNotNullOrEmpty())
                xmlAddress.Add(new XElement("DisplayName", mailAddress.DisplayName));
            return xmlAddress;
        }
        private static MailAddress MailAddressFromXml(XElement xmlAddress)
        {
            var address = xmlAddress.Descendants("address").First();
            var displayName = xmlAddress.Descendants("displayName");
            MailAddress a = new MailAddress(address.Value);
            if (displayName.Count() > 0)
                a.DisplayName = displayName.First().Value;
            return a;
        }
        private static XElement[] MailAddressListToXml(IList<IMailAddress> mailAddressList)
        {
            List<XElement> mailAddresses = new List<XElement>();
            for (int i = 0; i < mailAddressList.Count; i++)
            {
                XElement xmlAddress = new XElement("mailAddress",
                    new XElement("address", mailAddressList[i].Address));
                if (mailAddressList[i].DisplayName.IsNotNullOrEmpty())
                    xmlAddress.Add(new XElement("displayName", mailAddressList[i].DisplayName));
                mailAddresses.Add(xmlAddress);
            }
            return mailAddresses.ToArray();
        }
        private static MailAddressCollection MailAddressListFromXml(IEnumerable<XElement> xmlAddressList)
        {
            MailAddressCollection addressCollection = new MailAddressCollection();
            foreach (XElement xmlAddress in xmlAddressList)
            {
                addressCollection.Add(MailAddressFromXml(xmlAddress));
            }
            return addressCollection;
        }
        private static MailHeaderCollection HeadersFromXml(XElement headersElement)
        {
            MailHeaderCollection headersCollection = new MailHeaderCollection();
            if (headersElement != null)
            {
                foreach (XElement header in headersElement.DescendantNodes())
                {
                    headersCollection.Add(header.Name.ToString(), header.Value);
                }
            }
            return headersCollection;
        }
        private static List<string> ReferenceIDsFromXml(IEnumerable<XElement> xmlReferenceIDs)
        {
            List<string> referenceIDs = new List<string>();
            foreach (XElement referenceID in xmlReferenceIDs)
            {
                referenceIDs.Add(referenceID.Value);
            }
            return referenceIDs;
        }
        #endregion
    }
}
