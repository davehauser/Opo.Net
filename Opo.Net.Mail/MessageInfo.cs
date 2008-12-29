namespace Opo.Net.Mail
{
    /// <summary>
    /// Information about a mail message
    /// </summary>
    public class MessageInfo
    {
        /// <summary>
        /// Gets or sets the message number.
        /// </summary>
        /// <value>The message number.</value>
        public int MessageNumber { get; internal set; }
        /// <summary>
        /// Gets or sets the size.
        /// </summary>
        /// <value>The size.</value>
        public int Size { get; internal set; }
        /// <summary>
        /// Gets or sets the unique ID.
        /// </summary>
        /// <value>The unique ID.</value>
        public string UniqueID { get; internal set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageInfo"/> class.
        /// </summary>
        public MessageInfo() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageInfo"/> class.
        /// </summary>
        /// <param name="messageNumber">The message number.</param>
        /// <param name="size">The size of the message.</param>
        /// <param name="uniqueID">The unique ID of the message.</param>
        public MessageInfo(int messageNumber, int size, string uniqueID)
        {
            this.MessageNumber = messageNumber;
            this.Size = size;
            this.UniqueID = uniqueID;
        }
    }
}
