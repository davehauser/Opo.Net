using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Opo.Net.Mail
{
    /// <summary>
    /// Represents a POP3 client
    /// </summary>
    public interface IPop3Client : IMailClient, IRecieveMailClient, ISendMailClient, IDisposable
    {
        /// <summary>
        /// Gets the APOP timestamp.
        /// </summary>
        /// <value>The APOP timestamp.</value>
        string ApopTimestamp { get; }
        /// <summary>
        /// Gets or sets a value indicating whether to use SSL.
        /// </summary>
        /// <value><c>true</c> if SSL is used; otherwise, <c>false</c>.</value>
        bool AutoReconnect { get; set; }
        /// <summary>
        /// Gets the current state of the Pop3Client
        /// </summary>
        /// <value>The POP3 state.</value>
        Pop3SessionState State { get; }
        /// <summary>
        /// Gets or sets the timeout.
        /// </summary>
        /// <value>The timeout.</value>
        int ReadTimeout { get; set; }
        /// <summary>
        /// Gets mailbox info
        /// </summary>
        /// <value>The mailbox info</value>
        MailboxInfo Mailbox { get; }
        /// <summary>
        /// Gets the messages info
        /// </summary>
        /// <value>The messages info</value>
        MessageInfoCollection Messages { get; }

        /// <summary>
        /// Get the size of the message specified by messageNumber in bytes
        /// </summary>
        /// <param name="messageNumber">Message number on server. Notice: After deleting or downloading messages, the message numbers may change.</param>
        /// <returns>Size of specified message in bytes or -1, if the message was not found</returns>
        int GetMessageSize(int messageNumber);
        /// <summary>
        /// Gets the unique id (created by the mail server) for a message
        /// </summary>
        /// <param name="messageNumber">Message number on server. Notice: After deleting or downloading messages, the message numbers may change.</param>
        /// <returns>The unique id or an empty string, if the message was not found</returns>
        string GetUid(int messageNumber);
        /// <summary>
        /// Get a specific email message from the server
        /// </summary>
        /// <param name="messageNumber">Message number on server. Notice: After deleting or downloading messages, the message numbers may change.</param>
        /// <returns>A string containing the raw text of the message</returns>
        string GetMessage(int messageNumber);
        /// <summary>
        /// Get the headers of a specific email message
        /// </summary>
        /// <param name="messageNumber">Message number on server. Notice: After deleting or downloading messages, the message numbers may change.</param>
        /// <returns>A string containg the raw text of the message's headers</returns>
        string GetMessageHeaders(int messageNumber);
        /// <summary>
        /// Get the headers of a specific email message
        /// </summary>
        /// <param name="uid">Unique ID of the message</param>
        /// <returns>A string containg the raw text of the message's headers</returns>
        string GetMessageHeaders(string uid);

        /// <summary>
        /// Deletes a message from the mail server
        /// </summary>
        /// <param name="messageNumber">Message number on server. Notice: After deleting or downloading messages, the message numbers may change.</param>
        /// <returns>true if successfull</returns>
        bool DeleteMessage(int messageNumber);
    }
}
