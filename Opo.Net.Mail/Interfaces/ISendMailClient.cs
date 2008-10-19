using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Opo.Net.Mail
{
    /// <summary>
    /// Represents a mail client for sending email messages
    /// </summary>
    public interface ISendMailClient
    {
        /// <summary>
        /// Gets or sets the Host of the mail server
        /// </summary>
        string Host {get; set;}
        /// <summary>
        /// Gets or sets the mail servers port
        /// </summary>
        int Port {get; set;}
        /// <summary>
        /// Gets or sets the username
        /// </summary>
        string Username { get; set; }
        /// <summary>
        /// Gets or sets the users password
        /// </summary>
        string Password { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether SSL is used
        /// </summary>
        bool UseSSL { get; set; }
    }
}
