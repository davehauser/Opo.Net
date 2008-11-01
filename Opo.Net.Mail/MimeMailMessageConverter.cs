using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Opo.ProjectBase;
using Opo.Net.Mime;
using System.Globalization;
using System.Text.RegularExpressions;
using System.IO;

namespace Opo.Net.Mail
{
    /// <summary>
    /// Converts MIME data to MailMessage and vice versa
    /// </summary>
    public class MimeMailMessageConverter : IMailMessageConverter
    {
        private IMimeParser _mimeParser;
        /// <summary>
        /// Gets or sets the IMimeParser that is used for conversion. Default value is an instance of RegexMimeParser
        /// </summary>
        public IMimeParser MimeParser
        {
            get
            {
                if (_mimeParser == null)
                    _mimeParser = new RegexMimeParser();
                return _mimeParser;
            }
            set
            {
                _mimeParser = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the MimeMailMessageConverter class.
        /// </summary>
        public MimeMailMessageConverter() { }
        /// <summary>
        /// Initializes a new instance of the MimeMailMessageConverter class.
        /// </summary>
        /// <param name="mimeParser">IMimeParser instance that is used for conversion</param>
        public MimeMailMessageConverter(IMimeParser mimeParser)
        {
            MimeParser = mimeParser;
        }

        public IMailMessage ConvertFrom(object data)
        {
            IMailMessage mailMessage;
            if (data is String)
            {
                mailMessage = ConvertFrom(data as String);
            }
            else if (data is IMimeEntity)
            {
                mailMessage = ConvertFrom(data as IMimeEntity);
            }
            else
            {
                throw new ArgumentException("Input type " + data.GetType().ToString() + " is not supported by MimeMailMessageConverter");
            }
            return mailMessage;
        }

        /// <summary>
        /// Converts MIME data to MailMessage
        /// </summary>
        /// <param name="data">A String containing the MIME data</param>
        /// <returns>A new instance of the MailMessage class</returns>
        public IMailMessage ConvertFrom(string mimeData)
        {
            string contentType = MimeParser.ParseContentType(mimeData);
            IMimeEntity mimeEntity = MimeEntityFactory.GetInstance(MimeParser, contentType);
            mimeEntity.SetMimeData(mimeData);
            return ConvertFrom(mimeEntity);
        }
        /// <summary>
        /// Converts MIME data to MailMessage
        /// </summary>
        /// <param name="mimeParser">IMimeParser that is used to parse the MIME data</param>
        /// <param name="mimeData">A String containing the MIME data</param>
        /// <returns>A new instance of the MailMessage class</returns>
        public IMailMessage ConvertFrom(IMimeEntity mimeEntity)
        {
            mimeEntity.Validate("mimeData");

            IMailMessage mailMessage = new MailMessage();

            // set Subject
            mailMessage.Subject = mimeEntity.GetHeaderValue("Subject");

            // set addresses
            string[] from = ParseAddresses(mimeEntity.GetHeaderValue("From"));
            try
            {
                mailMessage.From = MailAddress.Parse(from[0]);
            }
            catch (Exception) { }

            string[] to = mimeEntity.GetHeaderValue("To").Split(',');
            foreach (string address in to)
            {
                try
                {
                    mailMessage.To.Add(MailAddress.Parse(address.Trim()));
                }
                catch (Exception) { }
            }

            string[] cc = mimeEntity.GetHeaderValue("CC").Split(',');
            foreach (string address in cc)
            {
                try
                {
                    mailMessage.Cc.Add(MailAddress.Parse(address.Trim()));
                }
                catch (Exception) { }
            }

            string[] bcc = mimeEntity.GetHeaderValue("BCC").Split(',');
            foreach (string address in bcc)
            {
                try
                {
                    mailMessage.Bcc.Add(MailAddress.Parse(address.Trim()));
                }
                catch (Exception) { }
            }

            string[] replyTo = ParseAddresses(mimeEntity.GetHeaderValue("Reply-To"));
            try
            {
                mailMessage.ReplyTo = MailAddress.Parse(replyTo[0]);
            }
            catch (Exception) { }

            // set mail Date
            mailMessage.Date = MimeUtilities.ParseRfc2822Date(mimeEntity.GetHeaderValue("Date"));

            // set mail X-Priority
            int priority = -1;
            int.TryParse(mimeEntity.GetHeaderValue("X-Priority"), out priority);
            if (priority < 1)
                priority = 3;
            mailMessage.Priority = (MailPriority)priority;

            // set Reference-IDs
            string[] referenceIDs = mimeEntity.GetHeaderValue("References").Replace("> <", ",").Replace("<", "").Replace(">", "").Split(',');
            mailMessage.ReferenceIDs.AddRange(referenceIDs);

            mailMessage.Size = Encoding.UTF8.GetByteCount(mimeEntity.GetMimeData());

            if (mimeEntity is TextMimeEntity)
            {
                // set mail body
                mailMessage.Body = (mimeEntity as TextMimeEntity).GetContent();
                string textType = mimeEntity.GetHeaderValue("Content-Type");
                if (textType.Contains('/'))
                    textType = textType.Substring(textType.IndexOf('/'));
                if (textType.Contains(';'))
                    textType = textType.Substring(0, textType.IndexOf(';'));
                try
                {
                    mailMessage.BodyType = (MailMessageBodyType)Enum.Parse(typeof(MailMessageBodyType), textType, true);
                }
                catch (Exception) { }
            }
            else
            {
                if (mimeEntity.HasEntities)
                {
                    ProcessEntities(ref mailMessage, mimeEntity);
                }
            }
            return mailMessage;
        }
        private void ProcessEntities(ref IMailMessage mailMessage, IMimeEntity mimeEntity)
        {
            foreach (IMimeEntity entity in mimeEntity.Entities)
            {
                if (entity is TextMimeEntity)
                {
                    string content = (entity as TextMimeEntity).GetContent();
                    string contentType = entity.GetHeaderValue("Content-Type");
                    AlternativeView alternativeView = new AlternativeView(content, contentType);
                    alternativeView.Charset = MimeParser.ParseCharset(entity.GetMimeData());
                    alternativeView.TransferEncoding = entity.GetHeaderValue("Content-Transfer-Encoding");
                }
                else if (entity is AttachmentMimeEntity)
                {
                    string contentType = entity.GetHeaderValue("Content-Type");
                    Regex r = new Regex(@"filename=\x22(?<Name>.*?)\x22");
                    string name = r.Match(contentType).Groups["Name"].Value;
                    if (contentType.Contains(';'))
                        contentType = contentType.Substring(0, contentType.IndexOf(';'));
                    ContentDisposition contentDisposition = new ContentDisposition();
                    string contentDisp = entity.GetHeaderValue("Content-Disposition");
                    r = new Regex(@"filename=\x22(?<Filename>.*?)\x22");
                    string filename = r.Match(contentDisp).Groups["Filename"].Value;
                    contentDisposition.FileName = filename;

                    string contentTransferEncoding = entity.GetHeaderValue("Content-Transfer-Encoding");


                    IAttachment attachment = new Attachment(name, (entity as AttachmentMimeEntity).GetContent(), contentTransferEncoding);
                }
                else if (entity is MultipartMimeEntity)
                {
                    ProcessEntities(ref mailMessage, entity);
                }
            }
        }

        /// <summary>
        /// Converts an IMailMessage to MIME data
        /// </summary>
        /// <param name="mailMessage">IMailMessage to convert</param>
        /// <returns>A string containing the MIME data</returns>
        public object ConvertTo(IMailMessage mailMessage)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Loads an MIME data file to a IMailMessage
        /// </summary>
        /// <param name="filePath">Absolute path to the MIME data file</param>
        /// <returns>A new instance of the MailMessage class</returns>
        public IMailMessage LoadFromFile(string filePath)
        {
            if (!File.Exists(filePath))
                throw new IOException("File not found: " + filePath);

            StreamReader mimeReader = new StreamReader(File.OpenRead(filePath));
            return ConvertFrom(mimeReader.ReadToEnd());
        }

        /// <summary>
        /// Saves a IMailMessage to a MIME data file
        /// </summary>
        /// <param name="mailMessage">IMailMessage to convert</param>
        /// <param name="path">Absolute path where the MIME data file is saved</param>
        /// <returns>A String containing the generated filename of the MIME data file</returns>
        public string SaveToFile(IMailMessage message, string path)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Saves a IMailMessage to a MIME data file
        /// </summary>
        /// <param name="mailMessage">IMailMessage to convert</param>
        /// <param name="path">Absolute path where the MIME data file is saved</param>
        /// <param name="fileName">Filename for the MIME data file</param>
        /// <returns>A String containing the filename of the MIME data file</returns>
        public string SaveToFile(IMailMessage message, string path, string fileName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Checks if the addresses are encoded and returns an array of decoded addresses
        /// </summary>
        /// <param name="mimeData">A string containing the text data of a MIME message</param>
        /// <param name="type">A string containing the type of the addresses (e.g. "From", "CC")</param>
        /// <returns>A String array containing addresses of the MIME message</returns>
        public string[] ParseAddresses(string addressData)
        {
            addressData.Validate("addressData");

            string[] addresses = addressData.Split(',');
            for (int i = 0; i < addresses.Length; i++)
            {
                string revisedAddress = addresses[i].Trim();
                if (revisedAddress.StartsWith("=?"))
                {
                    // is quoted-printable?

                    int p = revisedAddress.ToLower().IndexOf("?q?");
                    if (p > -1)
                    {
                        revisedAddress = Mime.MimeEncoding.QuotedPrintable.Decode(revisedAddress.Substring(p + 3).Replace("?=", ""));
                    }
                }
                addresses[i] = revisedAddress;
            }
            return addresses;
        }
    }
}
