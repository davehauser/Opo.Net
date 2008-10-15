using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Opo.Net.Mail
{
    public static class IMailMessageExtensionMethods
    {
        public static void ConvertFrom(this IMailMessage mailMessage, IMailMessageConverter mailMessageConverter, object data)
        {
            mailMessage = mailMessageConverter.ConvertFrom(data);
        }
        public static object ConvertTo(this IMailMessage mailMessage, IMailMessageConverter mailMessageConverter)
        {
            return mailMessageConverter.ConvertTo(mailMessage);
        }
        
        public static string SaveToFile(this IMailMessage mailMessage, IMailMessageConverter mailMessageConverter, string path)
        {
            return mailMessageConverter.SaveToFile(mailMessage, path);
        }
        public static string SaveToFile(this IMailMessage mailMessage, IMailMessageConverter mailMessageConverter, string path, string fileName)
        {
            return mailMessageConverter.SaveToFile(mailMessage, path, fileName);
        }
        public static void LoadFromFile(this IMailMessage mailMessage, IMailMessageConverter mailMessageConverter, string path)
        {
            mailMessage = mailMessageConverter.LoadFromFile(path);
        }
    }
}
