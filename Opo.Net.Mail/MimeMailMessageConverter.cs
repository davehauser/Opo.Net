using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Opo.Net.Mail
{
    public class MimeMailMessageConverter : IMailMessageConverter
    {
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
