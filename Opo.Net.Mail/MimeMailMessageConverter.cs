using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Opo.ProjectBase;
using Opo.Net.Mime;

namespace Opo.Net.Mail
{
    public class MimeMailMessageConverter : IMailMessageConverter
    {
        public IMailMessage ConvertFrom(object data)
        {
            string mimeData = data as string;
            data.Validate("data");

            IMailMessage mailMessage = new MailMessage();
            IMimeParser mimeParser = new RegexMimeParser();
            string contentType = mimeParser.ParseContentType(mimeData);
            IMimeEntity mimeEntity = new MimeEntityBase(mimeParser, contentType, mimeData);
            
            return mailMessage;
        }

        public object ConvertTo(IMailMessage mailMessage)
        {
            throw new NotImplementedException();
        }

        public IMailMessage LoadFromFile(string filePath)
        {
            throw new NotImplementedException();
        }

        public string SaveToFile(IMailMessage message, string path)
        {
            throw new NotImplementedException();
        }

        public string SaveToFile(IMailMessage message, string path, string fileName)
        {
            throw new NotImplementedException();
        }
    }
}
