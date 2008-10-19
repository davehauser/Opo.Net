using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Opo.Net.Mail
{
    /// <summary>
    /// Collection of IMailAddresses
    /// </summary>
    public class MailAddressCollection : List<IMailAddress>
    {
        /// <summary>
        /// Initializes a new instance of the MailAddressCollection class
        /// </summary>
        public MailAddressCollection() { }

        /// <summary>
        /// Initializes a new instance of the MailAddressCollection class and fills it with the mail addresses of a given collection
        /// </summary>
        /// <param name="mailAddresses"></param>
        public MailAddressCollection(IEnumerable<IMailAddress> mailAddresses)
            : base(mailAddresses) { }

        /// <summary>
        /// Returns a string representing the MailAddressCollection
        /// </summary>
        /// <returns>A String containing the mail addresses separated by a semicolon</returns>
        public override string ToString()
        {
            return ToString(";");
        }
        
        /// <summary>
        /// Returns a string representing the MailAddressCollection
        /// </summary>
        /// <param name="separator">A String containing the custom separator for the mail addresses</param>
        /// <returns>A String containing the mail addresses separated by a custom separator</returns>
        public string ToString(string separator)
        {
            return String.Join(separator, this.Select(m => m.ToString()).ToArray());
        }
    }
}
