using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Opo.Net.Mail
{
    /// <summary>
    /// Represents a mail client for recieving email messages
    /// </summary>
    public interface IRecieveMailClient
    {
        /// <summary>
        /// Gets or sets the mail servers host
        /// </summary>
        string Host { get; set; }
        /// <summary>
        /// Gets or sets the port of the mail server
        /// </summary>
        int Port { get; set; }
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
        /// <summary>
        /// Gets or sets a value indicating whether messages should be deleted on the mail server after download
        /// </summary>
        bool DeleteMessagesAfterDownload { get; set; }

        /// <summary>
        /// Connects to the mail server
        /// </summary>
        /// <returns>A String containing the mail servers response</returns>
        string Connect();
        /// <summary>
        /// Disconnects from the mail server
        /// </summary>
        void Disconnect();
        /// <summary>
        /// Logs a user in
        /// </summary>
        /// <param name="username">Username for the login</param>
        /// <param name="password">Users password</param>
        /// <returns>A value indicating whether login was successful</returns>
        bool Login(string username, string password);
        /// <summary>
        /// Logs a user out
        /// </summary>
        /// <returns>A value indicating whether logout was successful</returns>
        bool Logout();
        /// <summary>
        /// Downloads a message from the mail server
        /// </summary>
        /// <param name="id">ID of the mail message</param>
        /// <returns>A string containing the message data</returns>
        string GetMessage(string id);
        /// <summary>
        /// Deletes a message from the server
        /// </summary>
        /// <param name="id">ID ot the mail message to be deleted</param>
        /// <returns>A value indicating whether the message was deleted</returns>
        bool DeleteMessage(string id);
    }
}