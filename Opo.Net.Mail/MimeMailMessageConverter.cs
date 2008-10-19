using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Opo.ProjectBase;
using Opo.Net.Mime;

namespace Opo.Net.Mail
{
    /// <summary>
    /// Converts MIME data to MailMessage and vice versa
    /// </summary>
    public class MimeMailMessageConverter : IMailMessageConverter
    {
        /// <summary>
        /// Converts MIME data to MailMessage
        /// </summary>
        /// <param name="data">A String containing the MIME data</param>
        /// <returns>A new instance of the MailMessage class</returns>
        public IMailMessage ConvertFrom(object data)
        {
            string mimeData = data as string;
            mimeData.Validate("data");

            IMailMessage mailMessage = new MailMessage();
            IMimeParser mimeParser = new RegexMimeParser();
            string contentType = mimeParser.ParseContentType(mimeData);
            IMimeEntity mimeEntity = MimeEntityFactory.GetInstance(mimeParser, contentType);
            mimeEntity.MimeData = mimeData;
            
            return mailMessage;
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
