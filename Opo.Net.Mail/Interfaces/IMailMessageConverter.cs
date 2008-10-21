using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Opo.Net.Mail
{
    /// <summary>
    /// Contains methods for converting IMailMessages from and to other data types
    /// </summary>
    public interface IMailMessageConverter
    {
        /// <summary>
        /// Converts data to MailMessage
        /// </summary>
        /// <param name="data">Object that contains the mail data</param>
        /// <returns>A new instance of the MailMessage class</returns>
        IMailMessage ConvertFrom(object data);
        /// <summary>
        /// Converts an IMailMessage to an other data type
        /// </summary>
        /// <param name="mailMessage">IMailMessage to convert</param>
        /// <returns>Object with the according data</returns>
        object ConvertTo(IMailMessage mailMessage);
        /// <summary>
        /// Loads mail data from a file to a new instance of the MailMessage class
        /// </summary>
        /// <param name="data">String specifying the path of the data file</param>
        /// <returns>A new instance of the MailMessage class</returns>
        IMailMessage LoadFromFile(string path);
        /// <summary>
        /// Saves an IMailMessage to a mail data file
        /// </summary>
        /// <param name="mailMessage">IMailMessage to save to a file</param>
        /// <param name="path">Path to the data file. The filename is generated automatically</param>
        /// <returns>Filename of the data file</returns>
        string SaveToFile(IMailMessage mailMessage, string path);
        /// <summary>
        /// Saves an IMailMessage to a mail data file
        /// </summary>
        /// <param name="mailMessage">IMailMessage to convert</param>
        /// <param name="path">Path of the data file (without filename)</param>
        /// <param name="fileName">Name of the data file</param>
        /// <returns>Filename of the data file</returns>
        string SaveToFile(IMailMessage mailMessage, string path, string fileName);
    }
}