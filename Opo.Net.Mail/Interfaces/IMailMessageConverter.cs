using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Opo.Net.Mail
{
    public interface IMailMessageConverter
    {
        IMailMessage LoadFromFile(string filePath);
        string SaveToFile(IMailMessage message, string path);
        string SaveToFile(IMailMessage message, string path, string fileName);
    }
}
