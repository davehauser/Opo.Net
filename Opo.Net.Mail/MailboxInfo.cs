namespace Opo.Net.Mail
{
    /// <summary>
    /// Information about a mailbox
    /// </summary>
    public class MailboxInfo
    {
        /// <summary>
        /// Gets or sets the number of messages.
        /// </summary>
        /// <value>The number of messages.</value>
        public int NumberOfMessages { get; internal set; }
        /// <summary>
        /// Gets or sets the size of the mailbox.
        /// </summary>
        /// <value>The size of the mailbox.</value>
        public int MailboxSize { get; internal set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MailboxInfo"/> class.
        /// </summary>
        /// <param name="numberOfMessages">The number of messages.</param>
        /// <param name="mailboxSize">Size of the mailbox.</param>
        public MailboxInfo(int numberOfMessages, int mailboxSize)
        {
            this.NumberOfMessages = numberOfMessages;
            this.MailboxSize = mailboxSize;
        }
    }
}
