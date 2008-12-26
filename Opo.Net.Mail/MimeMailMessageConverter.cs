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

        /// <summary>
        /// Converts MIME data to MailMessage
        /// </summary>
        /// <param name="data">An object containing the MIME data (currently String or <see cref="IMimeEntity">IMimeEntity</see>)</param>
        /// <returns>A new instance of the <see cref="MailMessage">MailMessage</see> class</returns>
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
            IMimeEntity mimeEntity = MimeEntity.GetInstance(MimeParser, mimeData);
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

            if (_mimeParser == null)
                _mimeParser = mimeEntity.MimeParser;
            IMailMessage mailMessage = new MailMessage();

            // set Subject
            mailMessage.Subject = mimeEntity.GetHeaderValue("Subject");

            // set addresses
            string[] from = ParseAddresses(mimeEntity.GetHeaderValue("From"));
            try
            {
                mailMessage.From = MailAddress.Parse(from[0]);
            }
            catch (Exception)
            {
                mailMessage.From = null;
            }

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

            // set MessageID and Reference-IDs
            mailMessage.MessageID = mimeEntity.GetHeaderValue("Message-ID").Replace("<", "").Replace(">", "");
            string[] referenceIDs = mimeEntity.GetHeaderValue("References").Replace("> <", ",").Replace("<", "").Replace(">", "").Split(',');
            mailMessage.ReferenceIDs.AddRange(referenceIDs);

            // set Size
            mailMessage.Size = Encoding.UTF8.GetByteCount(mimeEntity.GetMimeData());

            // set Headers
            foreach (var header in mimeEntity.Headers)
            {
                mailMessage.Headers.Add(header.Key, header.Value);
            }

            if (mimeEntity is TextMimeEntity)
            {
                // set mail body
                mailMessage.Body = (mimeEntity as TextMimeEntity).GetContent();
                string textType = mimeEntity.ContentType;
                if (textType.Contains('/'))
                    textType = textType.Substring(textType.IndexOf('/'));
                if (textType.Contains(';'))
                    textType = textType.Substring(0, textType.IndexOf(';'));
                try
                {
                    mailMessage.BodyType = (MailMessageBodyType)Enum.Parse(typeof(MailMessageBodyType), textType, true);
                }
                catch (Exception)
                {
                    mailMessage.BodyType = MailMessageBodyType.PlainText;
                }
            }
            else if (mimeEntity is AttachmentMimeEntity)
            {
                // TODO: create attachment
            }

            if (mimeEntity.HasEntities)
            {
                ProcessEntities(ref mailMessage, mimeEntity);
            }
            return mailMessage;
        }
        /// <summary>
        /// Converts the Entities collection of a IMimeEntity to Body, AlternativeViews and Attachments and adds them to the MailMessage
        /// </summary>
        /// <param name="mailMessage">IMailMessage instance to add Body, AlternativeViews and Attachments</param>
        /// <param name="mimeEntity">IMimeEntity which's Entities collection is processed</param>
        private void ProcessEntities(ref IMailMessage mailMessage, IMimeEntity mimeEntity)
        {
            int textMimeEntityCounter = 0;
            foreach (IMimeEntity entity in mimeEntity.Entities)
            {
                if (entity is TextMimeEntity)
                {
                    textMimeEntityCounter++;
                    string content = (entity as TextMimeEntity).GetContent();
                    string contentType = entity.GetHeaderValue("Content-Type");
                    string charset = (entity as TextMimeEntity).Charset;
                    if (textMimeEntityCounter == 1)
                    {
                        if (entity.ContentTransferEncoding == ContentTransferEncoding.QuotedPrintable)
                        {
                            mailMessage.Body = MimeEncoding.QuotedPrintable.Decode(content);
                        }
                        else
                        {
                            mailMessage.Body = content;
                        }
                        switch (contentType.Substring(5))
                        {
                            case "html":
                                mailMessage.BodyType = MailMessageBodyType.Html;
                                break;
                            default:
                                mailMessage.BodyType = MailMessageBodyType.PlainText;
                                break;
                        }
                    }
                    else
                    {
                        AlternativeView alternativeView = new AlternativeView(content, contentType);
                        alternativeView.Charset = charset;
                        alternativeView.TransferEncoding = entity.GetHeaderValue("Content-Transfer-Encoding");
                        mailMessage.AlternativeViews.Add(alternativeView);
                    }
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

                if (entity.HasEntities)
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
            StringBuilder mimeData = new StringBuilder();

            // add headers
            foreach (MailHeader header in mailMessage.Headers)
            {
                mimeData.AppendLine(header.Name + ": " + header.Value);
            }
            mimeData.AppendLine();

            // message type
            if (mailMessage.HasAttachments)
            {
                string boundary = GenerateBoundary();
                mimeData.AppendLine("Content-Type: " + MediaType.Multipart.Mixed);
                mimeData.AppendLine("\tboundary=\"---=_Part_" + boundary + "\"");
            }
            else if (mailMessage.AlternativeViews.Count > 0)
            {
                string boundary = GenerateBoundary();
                mimeData.AppendLine("Content-Type: " + MediaType.Multipart.Alternative);
                mimeData.AppendLine("\tboundary=\"" + boundary + "\"");
                mimeData.AppendLine();
                mimeData.AppendLine("--" + boundary);
                switch (mailMessage.BodyType)
                {
                    case MailMessageBodyType.Html:
                        mimeData.AppendLine("Content-Type: text/html");
                        break;
                    case MailMessageBodyType.PlainText:
                    default:
                        mimeData.AppendLine("Content-Type: text/plain");
                        break;
                }

            }
            else
            {
                mimeData.AppendLine(mailMessage.Body);
            }

            return mimeData;
        }

        private string ConvertAlternativeViews(AlternativeViewCollection alternativeViews, string boundary)
        {
            StringBuilder mimeData = new StringBuilder();
            foreach (AlternativeView alternativeView in alternativeViews)
            {
                mimeData.AppendLine("--" + boundary);
                mimeData.AppendLine("Content-Type: " + alternativeView.ContentType + ";");
                mimeData.AppendLine("\tcharset=\"" + alternativeView.Charset + "\"");
                mimeData.AppendLine("Content-Transfer-Encoding: " + alternativeView.TransferEncoding);
                mimeData.AppendLine();
                mimeData.AppendLine(alternativeView.Content);
                mimeData.AppendLine();
            }
            return mimeData.ToString();
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

        private string GenerateBoundary()
        {
            string boundary = "---=_Part_";
            boundary += Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Replace("/", "_").Replace("+", ".").Substring(0, 22);
            return boundary;
        }
    }
}
