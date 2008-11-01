using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Opo.ProjectBase;
using Opo.Net.Mime;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Opo.Net.Mail
{
    /// <summary>
    /// Converts MIME data to MailMessage and vice versa
    /// </summary>
    public class MimeMailMessageConverter : IMailMessageConverter
    {
        private IMimeParser _mimeParser;

        public MimeMailMessageConverter(IMimeParser mimeParser)
        {
            _mimeParser = mimeParser;
        }

        /// <summary>
        /// Converts MIME data to MailMessage
        /// </summary>
        /// <param name="data">A String containing the MIME data</param>
        /// <returns>A new instance of the MailMessage class</returns>
        public IMailMessage ConvertFrom(object data)
        {
            IMailMessage mailMessage;
            if (data is String)
            {
                IMimeParser mimeParser = new RegexMimeParser();
                mailMessage = ConvertFrom(data as String);
            }
            else
            {
                mailMessage = new MailMessage();
            }
            return mailMessage;
        }

        /// <summary>
        /// Converts MIME data to MailMessage
        /// </summary>
        /// <param name="mimeParser">IMimeParser that is used to parse the MIME data</param>
        /// <param name="mimeData">A String containing the MIME data</param>
        /// <returns>A new instance of the MailMessage class</returns>
        public IMailMessage ConvertFrom(string mimeData)
        {
            mimeData.Validate("mimeData");

            IMailMessage mailMessage = new MailMessage();
            string contentType = _mimeParser.ParseContentType(mimeData);
            IMimeEntity mimeEntity = MimeEntityFactory.GetInstance(_mimeParser, contentType);
            mimeEntity.SetMimeData(mimeData);
            
            // set Subject
            mailMessage.Subject = mimeEntity.GetHeaderValue("Subject");

            // set addresses
            mailMessage.From = MailAddress.Parse(mimeEntity.GetHeaderValue("From"));

            System.Diagnostics.Debug.WriteLine("*** " + mimeEntity.GetType().ToString());
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

            // set mail Date
            IFormatProvider provider = CultureInfo.InvariantCulture;
            mailMessage.Date = DateTime.ParseExact(mimeEntity.GetHeaderValue("Date"), "R", provider);
            mailMessage.ReplyTo = MailAddress.Parse(mimeEntity.GetHeaderValue("Reply-To"));

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
                    alternativeView.Charset = _mimeParser.ParseCharset(entity.GetMimeData());
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
            throw new NotImplementedException();
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
    }
}
