using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Opo.Net.Mail
{
    /// <summary>
    /// Pop3 connection has the following states
    /// </summary>
    public enum Pop3SessionState
    {
        undefined = 0,
        /// <summary>
        /// No connection to the server
        /// </summary>
        Disconnected = 1,
        /// <summary>
        /// Connected but not authenticated
        /// </summary>
        Authorization = 2,
        /// <summary>
        /// Connected and authenticated, ready for STAT, LIST, RETR, DELE etc.
        /// </summary>
        Transaction = 3,
        /// <summary>
        /// On QUIT command, messages marked for deletion are deleted
        /// </summary>
        Update = 4
    }
}
