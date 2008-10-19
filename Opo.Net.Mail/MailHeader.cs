using System;
using System.Collections.Generic;
using System.Linq;

namespace Opo.Net.Mail
{
    /// <summary>
    /// Represents a mail header
    /// </summary>
    public class MailHeader
    {
        /// <summary>
        /// Gets or sets the name of the header
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the value of the header
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Initializes a new instance of the MailHeader class and sets the name and the value
        /// </summary>
        /// <param name="name">Header name</param>
        /// <param name="value">Header value</param>
        /// <exception cref="ArgumentNullException">Throws ArgumentNullException if the name is null or empty</exception>
        public MailHeader(string name, string value)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException("[name] cannot be empty");
            Name = name;
            Value = value;
        }

        /// <summary>
        /// Returns a string representing the header
        /// </summary>
        /// <returns>A String in the form "header name: header value"</returns>
        public override string ToString()
        {
            return String.Concat(Name, ": ", Value);
        }
    }
}
