using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Opo.Net.Mail
{
    /// <summary>
    /// Collection of MailHeaders
    /// </summary>
    public class MailHeaderCollection : List<MailHeader>
    {
        /// <summary>
        /// Adds a new mail header to the collection
        /// </summary>
        /// <param name="name">Header name</param>
        /// <param name="value">Header value</param>
        public void Add(string name, string value)
        {
            this.Add(new MailHeader(name, value));
        }
        /// <summary>
        /// Removes a mail header from the collection
        /// </summary>
        /// <param name="name">Header name</param>
        public void Remove(string name)
        {
            this.RemoveAll(h => h.Name == name);
        }
        /// <summary>
        /// Gets the value of a header
        /// </summary>
        /// <param name="name">Header name</param>
        /// <returns>A String containing the value of the specific header</returns>
        public string GetValue(string name)
        {
            MailHeader header = this.FirstOrDefault(h => h.Name == name);
            return header != null ? header.Value : "";
        }
    }
}
