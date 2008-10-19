using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Opo.Net.Mail
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMailMessageConverter
    {
        IMailMessage ConvertFrom(object data);
        object ConvertTo(IMailMessage mailMessage);
        IMailMessage LoadFromFile(string path);
        string SaveToFile(IMailMessage mailMessage, string path);
        string SaveToFile(IMailMessage mailMessage, string path, string fileName);
    }
}
