using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Opo.Net.Mail
{
    /// <summary>
    /// Extension methods for IMailMessage
    /// </summary>
    public static class IMailMessageExtensionMethods
    {
        /// <summary>
        /// Converts data to MailMessage
        /// </summary>
        /// <param name="mailMessage">IMailMessage to convert</param>
        /// <param name="mailMessageConverter">IMailMessageConverter that is used to convert the message</param>
        /// <param name="data">Object that contains the mail data</param>
        public static void ConvertFrom(this IMailMessage mailMessage, IMailMessageConverter mailMessageConverter, object data)
        {
            mailMessage = mailMessageConverter.ConvertFrom(data);
        }
        /// <summary>
        /// Converts an IMailMessage to an other data type
        /// </summary>
        /// <param name="mailMessage">IMailMessage to load the converted data</param>
        /// <param name="mailMessageConverter">IMailMessageConverter that is used to convert the data to the IMailMessage</param>
        /// <returns>Object with the according data</returns>
        public static object ConvertTo(this IMailMessage mailMessage, IMailMessageConverter mailMessageConverter)
        {
            return mailMessageConverter.ConvertTo(mailMessage);
        }

        /// <summary>
        /// Loads mail data from a file to a new instance of the MailMessage class
        /// </summary>
        /// <param name="mailMessage">IMailMessage to load the data</param>
        /// <param name="mailMessageConverter">IMailMessageConverter that is used to convert the data to the IMailMessage</param>
        /// <param name="path">String specifying the path of the data file</param>
        public static void LoadFromFile(this IMailMessage mailMessage, IMailMessageConverter mailMessageConverter, string path)
        {
            mailMessage = mailMessageConverter.LoadFromFile(path);
        }
        /// <summary>
        /// Saves an IMailMessage to a mail data file
        /// </summary>
        /// <param name="mailMessage">IMailMessage to save to a file</param>
        /// <param name="mailMessageConverter">IMailMessageConverter that is used to convert the message</param>
        /// <param name="path">Path to the data file. The filename is generated automatically</param>
        /// <returns>Filename of the data file</returns>
        public static string SaveToFile(this IMailMessage mailMessage, IMailMessageConverter mailMessageConverter, string path)
        {
            return mailMessageConverter.SaveToFile(mailMessage, path);
        }
        /// <summary>
        /// Saves an IMailMessage to a mail data file
        /// </summary>
        /// <param name="mailMessage">IMailMessage to convert</param>
        /// <param name="mailMessageConverter">IMailMessageConverter that is used to convert the message</param>
        /// <param name="path">Path of the data file (without filename)</param>
        /// <param name="fileName">Name of the data file</param>
        /// <returns>Filename of the data file</returns>
        public static string SaveToFile(this IMailMessage mailMessage, IMailMessageConverter mailMessageConverter, string path, string fileName)
        {
            return mailMessageConverter.SaveToFile(mailMessage, path, fileName);
        }
    }
}
