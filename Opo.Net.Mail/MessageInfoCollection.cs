using System.Collections.Generic;
using System.Linq;

namespace Opo.Net.Mail
{
    /// <summary>
    /// Collection of message information
    /// </summary>
    public class MessageInfoCollection : List<MessageInfo>
    {
        /// <summary>
        /// Gets the unique id from the message specified by the message number
        /// </summary>
        /// <param name="messageNumber">Message number</param>
        /// <returns>Unique id of the message or an emtpy string if the message was not found</returns>
        public string GetUniqueID(int messageNumber)
        {
            MessageInfo mi = this.First(m => m.MessageNumber == messageNumber);
            if (mi != null)
                return mi.UniqueID;
            return "";
        }
        /// <summary>
        /// Gets the message number from the message specified by the unique id
        /// </summary>
        /// <param name="uid">Unique id of the message</param>
        /// <returns>Message number or -1 if the message was not found</returns>
        public int GetMessageNumber(string uid)
        {
            MessageInfo mi = this.First(m => m.UniqueID == uid);
            if (mi != null)
                return mi.MessageNumber;
            return -1;
        }
    }
}
