using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Opo.Net.Mail
{
    public delegate void Pop3EventHandler(object source, Pop3Client.Pop3EventArgs e);

    public class Pop3Client : IRecieveMailClient, IDisposable
    {
        #region Fields
        private TcpClient client;
        private Stream stream;
        private StreamReader reader;
        private string lastResponse;
        private const string CRLF = "\r\n";
        private int readTimeout = 6000;
        private MessageInfoCollection messages = new MessageInfoCollection();
        private List<int> deletedMessageNumbers = new List<int>();
        private List<int> downloadedMessageNumbers = new List<int>();
        #endregion

        #region Properties
        /// <summary>
        /// Gets the timestamp the server returned on connecting (only if server supports APOP)
        /// </summary>
        public string ApopTimestamp { get; private set; }
        /// <summary>
        /// Gets or sets the pop3 mail server host (ip or domain name)
        /// </summary>
        /// <example>
        /// pop.example.com, 208.77.188.166
        /// </example>
        public string Host { get; set; }
        /// <summary>
        /// Gets or sets the port to use for connecting to the pop3 mail server
        /// </summary>
        public int Port { get; set; }
        /// <summary>
        /// Gets or sets the username for authentication
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// Gets or sets the password for authentication
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// Gets or sets if SSL is used for the connection
        /// </summary>
        public bool UseSSL { get; set; }
        /// <summary>
        /// Gets or sets if the pop3 client should auto reconnect to the pop3 mail server when loosing connection. Default is false.
        /// </summary>
        public bool AutoReconnect { get; set; }
        /// <summary>
        /// Gets the current session state of the pop3 client. Default is true.
        /// </summary>
        public Pop3SessionState State { get; private set; }
        /// <summary>
        /// Gets or sets the read timeout in milliseconds. Default is 6000.
        /// </summary>
        public int ReadTimeout
        {
            get
            {
                return readTimeout;
            }
            set
            {
                readTimeout = value;
                if (stream != null && stream.CanTimeout)
                    stream.ReadTimeout = readTimeout;
            }
        }
        /// <summary>
        /// Gets Number of messages and the size of the mailbox
        /// </summary>
        public MailboxInfo Mailbox { get; private set; }
        /// <summary>
        /// Gets messages info (message number, size, unique id)
        /// </summary>
        public MessageInfoCollection Messages
        {
            get { return this.messages; }
        }
        /// <summary>
        /// Gets or sets a value indicating, wheter messages should be deleted from the server after downloading. Default is false.
        /// </summary>
        public bool DeleteMessagesAfterDownload { get; set; }
        #endregion

        #region De-/Constructor
        public Pop3Client(string host, int port)
        {
            this.Host = host;
            this.Port = port;
            this.ApopTimestamp = "";
            this.UseSSL = false;
            this.AutoReconnect = true;
            this.State = Pop3SessionState.Disconnected;
            this.DeleteMessagesAfterDownload = false;
            this.Mailbox = new MailboxInfo(-1, -1);
            this.messages = new MessageInfoCollection();
        }
        public Pop3Client(string host, int port, string username, string password)
            : this(host, port)
        {
            this.Username = username;
            this.Password = password;
        }
        ~Pop3Client()
        {
            if (client != null)
                client.Close();
            if (stream != null)
                stream.Dispose();
            if (reader != null)
                reader.Dispose();
        }
        #endregion

        #region Dis-/Connect
        /// <summary>
        /// Establish connection to the pop3 mail server
        /// </summary>
        /// <returns>Response from the server</returns>
        public string Connect()
        {
            string response;
            try
            {
                // connect to the client
                client = new TcpClient(this.Host, this.Port);
                try
                {
                    if (this.UseSSL)
                    {
                        // get ssl stream
                        stream = new SslStream(client.GetStream(), false);

                        // authenticate
                        ((SslStream)stream).AuthenticateAsClient(this.Host);
                    }
                    else
                    {
                        stream = client.GetStream();
                    }
                    stream.ReadTimeout = readTimeout;
                }
                catch (Exception e)
                {
                    if (this.UseSSL)
                        throw new Pop3Exception(String.Concat("Host found but SSL authentication failed. May your mail server does not support SSL.", Environment.NewLine, e.Message), e.InnerException);
                    else
                        throw new Pop3Exception(String.Concat("Connecting to the server failed. May your mail server requires SSL.", Environment.NewLine, e.Message), e.InnerException);
                }
                reader = new StreamReader(stream, Encoding.ASCII);

                // get first response form server
                lastResponse = reader.ReadLine();
                if (lastResponse.StartsWith("+OK"))
                {
                    // try to extract apop timestamp
                    Regex r = new Regex(@"(?<TimeStamp>\x3C[^\x3C\x3E]+\x3E)");
                    Match m = r.Match(lastResponse);
                    this.ApopTimestamp = m.Groups["TimeStamp"].Value;
                    // update session state
                    SetSessionState(Pop3SessionState.Authorization);
                }
                response = lastResponse;
            }
            catch (Pop3Exception e)
            {
                throw new Pop3Exception(String.Concat("An error occured while connecting to ", this.Host, ":", this.Port, (this.UseSSL ? "using SSL " : ""), Environment.NewLine, "Error: ", e.Message), e.InnerException);
            }
            return response;
        }
        /// <summar>
        /// Disconnect from mail server
        /// </summar>
        public void Disconnect()
        {
            if (client.Client.Connected)
                client.Client.Disconnect(true);
            client.Close();
        }
        /// <summary>
        /// Releases resources used by the Opo.Net.Mail.Pop3Client
        /// </summary>
        public void Dispose()
        {
            Disconnect();
            if (stream != null)
                stream.Dispose();
            if (reader != null)
                reader.Dispose();
            SetSessionState(Pop3SessionState.Disconnected);
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Logs in with username and password given to the constructor
        /// </summary>
        /// <returns>true: if successfully logged in</returns>
        public bool Login()
        {
            return Login(this.Username, this.Password);
        }
        /// <summary>
        /// Logs in with username and password given to the constructor
        /// </summary>
        /// <param name="useApop">true if APOP (password is encrypted) login should be used, if APOP is not supported user/pass will be used</param>
        /// <returns>true: if successfully logged in</returns>
        public bool Login(bool useApop)
        {
            return Login(this.Username, this.Password, useApop);
        }
        /// <summary>
        /// Logs in with username and password
        /// </summary>
        /// <param name="username">Username for the mailbox</param>
        /// <param name="password">Password for the mailbox</param>
        /// <returns>true: if successfully logged in</returns>
        public bool Login(string username, string password)
        {
            return Login(username, password, false);
        }
        /// <summary>
        /// Logs in with username and password
        /// </summary>
        /// <param name="username">Username for the mailbox</param>
        /// <param name="password">Password for the mailbox</param>
        /// <param name="useApop">true if APOP (password is encrypted) login should be used, if APOP is not supported user/pass will be used</param>
        /// <returns>true: if successfully logged in</returns>
        public bool Login(string username, string password, bool useApop)
        {
            if (!EnsureSessionState("pass"))
            {
                Connect();
            }
            bool loggedin = false;
            if (useApop && this.ApopTimestamp.IsNotNullOrEmpty())
            {
                // use apop login
                if (APOP(username, password).IsOK())
                {
                    loggedin = true;
                }
            }
            else
            {
                // use user/pass login
                if (USER(username).IsOK())
                {
                    if (PASS(password).IsOK())
                    {
                        loggedin = true;
                    }
                }
            }
            if (loggedin)
            {
                UpdateMailboxStats();
                UpdateMessagesInfo();
            }
            return loggedin;
        }
        /// <summary>
        /// Logout and disconnect from mail server (deletes mail messages marked for deletion and, if DeleteMessagesAfterDownload is set to true, downloaded messages)
        /// </summary>
        public bool Logout()
        {
            if (!DeleteMessagesAfterDownload)
            {
                RSET();
                foreach (int messageNumber in this.deletedMessageNumbers)
                {
                    DELE(messageNumber);
                }
            }
            else
            {
                foreach (int messageNumber in this.downloadedMessageNumbers)
                {
                    DELE(messageNumber);
                }
            }
            return QUIT().IsOK();
        }

        /// <summary>
        /// Get the size of the message specified by messageNumber in bytes
        /// </summary>
        /// <param name="messageNumber">Message number on server. Notice: After deleting or downloading messages, the message numbers may change.</param>
        /// <returns>Size of specified message in bytes or -1, if the message was not found</returns>
        public int GetMessageSize(int messageNumber)
        {
            int messageSize = -1;
            bool success = SendCommand(String.Format("LIST {0}", messageNumber));
            string response;
            if (success && (response = GetSingleLineResponse()).IsOK())
            {
                string[] values = response.Split(' ');
                int.TryParse(values[2], out messageSize);
            }
            return messageSize;
        }
        /// <summary>
        /// Gets the unique id (created by the mail server) for a message
        /// </summary>
        /// <param name="messageNumber">Message number on server. Notice: After deleting or downloading messages, the message numbers may change.</param>
        /// <returns>The unique id or an empty string, if the message was not found</returns>
        public string GetUid(int messageNumber)
        {
            string uniqueID = "";
            MessageInfo mi = this.messages.FirstOrDefault(m => m.MessageNumber == messageNumber);
            if (mi != null)
            {
                uniqueID = mi.UniqueID;
            }
            else
            {
                bool success = SendCommand(String.Format("UIDL {0}", messageNumber));
                string response;
                if (success && (response = GetSingleLineResponse()).IsOK())
                {
                    string[] values = response.Split(' ');
                    uniqueID = values[2];
                }
            }
            return uniqueID;
        }
        /// <summary>
        /// Get a specific email message from the server
        /// </summary>
        /// <param name="messageNumber">Message number on server. Notice: After deleting or downloading messages, the message numbers may change.</param>
        /// <returns>A string containing the raw text of the message</returns>
        public string GetMessage(int messageNumber)
        {
            return RETR(messageNumber);
        }
        /// <summary>
        /// Get a specific email message from the server
        /// </summary>
        /// <param name="uid">Unique ID of the message</param>
        /// <returns>A string containing the raw text of the message</returns>
        public string GetMessage(string uid)
        {
            int messageNumber = this.messages.GetMessageNumber(uid);
            return GetMessage(messageNumber);
        }
        /// <summary>
        /// Get the headers of a specific email message
        /// </summary>
        /// <param name="messageNumber">Message number on server. Notice: After deleting or downloading messages, the message numbers may change.</param>
        /// <returns>A string containg the raw text of the message's headers</returns>
        public string GetMessageHeaders(int messageNumber)
        {
            return TOP(messageNumber);
        }
        /// <summary>
        /// Get the headers of a specific email message
        /// </summary>
        /// <param name="uid">Unique ID of the message</param>
        /// <returns>A string containg the raw text of the message's headers</returns>
        public string GetMessageHeaders(string uid)
        {
            int messageNumber = this.messages.GetMessageNumber(uid);
            return GetMessageHeaders(messageNumber);
        }

        /// <summary>
        /// Deletes a message from the mail server
        /// </summary>
        /// <param name="messageNumber">Message number on server. Notice: After deleting or downloading messages, the message numbers may change.</param>
        /// <returns>true if successfull</returns>
        public bool DeleteMessage(int messageNumber)
        {
            if (DELE(messageNumber).IsOK())
            {
                deletedMessageNumbers.Add(messageNumber);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Deletes a message from the mail server
        /// </summary>
        /// <param name="uid">Unique ID of the message</param>
        /// <returns>true if successfull</returns>
        public bool DeleteMessage(string uid)
        {
            int messageNumber = this.messages.GetMessageNumber(uid);
            return DeleteMessage(messageNumber);
        }

        #region Standard POP3 Commands
        public string USER(string username)
        {
            bool success = SendCommand(String.Format("USER {0}", username));
            return success ? GetSingleLineResponse() : "";
        }

        public string PASS(string password)
        {
            // send PASS command
            bool success = SendCommand(String.Format("PASS {0}", password));
            string response = success ? GetSingleLineResponse() : "";

            // update session state
            if (response.IsOK())
                SetSessionState(Pop3SessionState.Transaction);
            else if (response.IsERR())
                SetSessionState(Pop3SessionState.Authorization);
            else
                SetSessionState(Pop3SessionState.Disconnected);

            // return server response
            return response;
        }

        public string STAT()
        {
            bool success = SendCommand("STAT");
            return success ? GetSingleLineResponse() : "";
        }

        public string LIST()
        {
            bool success = SendCommand("LIST");
            return success ? GetMultiLineResponse() : "";
        }
        public string LIST(int messageNumber)
        {
            bool success = SendCommand(String.Format("LIST {0}", messageNumber));
            return success ? GetSingleLineResponse() : "";
        }

        public string RETR(int messageNumber)
        {
            bool success = SendCommand(String.Format("RETR {0}", messageNumber));
            return success ? GetMultiLineResponse() : "";
        }

        public string DELE(int messageNumber)
        {
            bool success = SendCommand(String.Format("DELE {0}", messageNumber));
            return success ? GetSingleLineResponse() : "";
        }

        public string RSET()
        {
            bool success = SendCommand("RSET");
            return success ? GetSingleLineResponse() : "";
        }

        public string QUIT()
        {
            // update session state
            SetSessionState(Pop3SessionState.Update);

            // send QUIT command
            bool success = SendCommand("QUIT");
            string response = success ? GetSingleLineResponse() : "";

            // update session state
            if (response.IsOK())
                SetSessionState(Pop3SessionState.Disconnected);
            else
                SetSessionState(Pop3SessionState.undefined);

            // return server response
            return response;
        }

        public string APOP(string username, string password)
        {
            // calculate md5 has
            MD5 md5Hasher = MD5.Create();
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(this.ApopTimestamp + password));
            StringBuilder digest = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                digest.Append(data[i].ToString("x2"));
            }

            // send apop command
            bool success = SendCommand(String.Format("APOP {0} {1}", username, digest));
            string response = success ? GetSingleLineResponse() : "";

            // update session state
            if (response.IsOK())
                SetSessionState(Pop3SessionState.Transaction);
            else if (response.IsERR())
                SetSessionState(Pop3SessionState.Authorization);
            else
                SetSessionState(Pop3SessionState.Disconnected);

            // return server response
            return response;
        }

        public string TOP(int messageNumber)
        {
            bool success = SendCommand(String.Format("TOP {0}", messageNumber));
            return success ? GetMultiLineResponse() : "";
        }
        public string TOP(int messageNumber, int numberOfLines)
        {
            bool success = SendCommand(String.Format("TOP {0} {1}", messageNumber, numberOfLines));
            return success ? GetMultiLineResponse() : "";
        }

        public string UIDL()
        {
            bool success = SendCommand("UIDL");
            return success ? GetMultiLineResponse() : "";
        }
        public string UIDL(int messageNumber)
        {
            bool success = SendCommand(String.Format("UIDL {0}", messageNumber));
            return success ? GetSingleLineResponse() : "";
        }

        public string NOOP()
        {
            bool success = SendCommand("NOOP");
            return success ? GetSingleLineResponse() : "";
        }
        #endregion

        #endregion

        #region Events

        // state change
        public event Pop3EventHandler SessionStateChanging;
        private void OnSessionStateChanging(Pop3EventArgs e)
        {
            if (SessionStateChanging != null)
                SessionStateChanging(this, e);
        }
        public event Pop3EventHandler SessionStateChanged;
        private void OnSessionStateChanged(Pop3EventArgs e)
        {
            if (SessionStateChanged != null)
                SessionStateChanged(this, e);
        }

        // pop3 event arguments
        public class Pop3EventArgs : EventArgs
        {
            public Pop3SessionState SessionState { get; set; }

            public Pop3EventArgs(Pop3SessionState currentState)
            {
                this.SessionState = currentState;
            }
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Send a command to the mail server
        /// </summary>
        /// <param name="command">Pop3 command and parameters if necessary e.g. RETR 1</param>
        /// <returns>true: command was successfully sent</returns>
        private bool SendCommand(string command)
        {
            bool success = EnsureSessionState(command);
            if (success)
            {
                try
                {
                    byte[] buffer = Encoding.ASCII.GetBytes(command + CRLF);
                    stream.Write(buffer, 0, buffer.Length);
                }
                catch (Exception)
                {
                    success = false;
                }
            }
            return success;
        }

        /// <summary>
        /// Reads a single line from the mail server response
        /// </summary>
        /// <returns>The response of the server (1 line)</returns>
        private string GetSingleLineResponse()
        {
            string response = reader.ReadLine();
            if (response == null)
                throw new Pop3Exception("Pop3 mail server has not responded. May the connection timed out.");
            return response;
        }
        /// <summary>
        /// Reads a whole multiline response from the mail server
        /// </summary>
        /// <returns>The response of the server</returns>
        private string GetMultiLineResponse()
        {
            StringBuilder response = new StringBuilder();
            lastResponse = reader.ReadLine();
            response.AppendLine(lastResponse);
            if (lastResponse.IsOK())
            {
                while (ProcessMultiLineResponse(out lastResponse))
                {
                    response.AppendLine(lastResponse);
                }
                response.AppendLine(lastResponse);
            }
            return response.ToString();
        }
        /// <summary>
        /// Processes one line of the server response at a time and returns true until the end of the response is reached
        /// </summary>
        /// <param name="response">Last line of the response</param>
        /// <returns>false: The end of the response is reached</returns>
        private bool ProcessMultiLineResponse(out string response)
        {
            response = reader.ReadLine();
            if (response == null)
                throw new Pop3Exception("Pop3 mail server has not responded. May the connection timed out.");
            if (response.StartsWith("."))
            {
                if (response == ".")
                    // multiline responses always end with a single dot in a line
                    return false;
                // if a line starts with a dot it is doubled, the second dot must be removed now
                response = response.Substring(1);
            }
            return true;
        }

        /// <summary>
        /// Sets the session state
        /// </summary>
        /// <param name="state">Pop3 session state to set</param>
        private void SetSessionState(Pop3SessionState state)
        {
            OnSessionStateChanging(new Pop3EventArgs(this.State));
            this.State = state;
            OnSessionStateChanged(new Pop3EventArgs(this.State));
        }
        /// <summary>
        /// Make sure the current pop3 session state is appropriate for the requested command
        /// </summary>
        /// <param name="command">Command which will be sent to the server</param>
        /// <returns>true: The current session state is ok for the requested command</returns>
        private bool EnsureSessionState(string command)
        {
            switch (command.Substring(0, 4).ToLower())
            {
                case "user":
                case "pass":
                case "apop":
                    return (this.State == Pop3SessionState.Authorization);
                case "quit":
                    //TODO: Check if this is the right state
                    return (this.State == Pop3SessionState.Update);
                default:
                    return (this.State == Pop3SessionState.Transaction);
            }
        }
        /// <summary>
        /// Gets mailbox stats from server and updates the Mailbox property
        /// </summary>
        /// <returns>true: if successful</returns>
        private bool UpdateMailboxStats()
        {
            string response;
            bool success = SendCommand("STAT");
            if (success && (response = GetSingleLineResponse()).IsOK())
            {
                int numberOfMessages = -1;
                int mailboxSize = -1;
                string[] values = response.Split(' ');
                int.TryParse(values[1], out numberOfMessages);
                int.TryParse(values[2], out mailboxSize);
                this.Mailbox = new MailboxInfo(numberOfMessages, mailboxSize);
                return true;
            }
            this.Mailbox = new MailboxInfo(-1, -1);
            return false;
        }
        /// <summary>
        /// Gets messages stats from server and updates the messages property
        /// </summary>
        /// <returns>true: if successful</returns>
        public bool UpdateMessagesInfo()
        {
            bool messagesUpdated = false;
            MessageInfoCollection messagesInfo = new MessageInfoCollection();
            string response;
            // get message numbers and message sizes
            bool success = SendCommand("LIST");
            if (success && GetSingleLineResponse().IsOK())
            {
                while (ProcessMultiLineResponse(out response))
                {
                    int messageNumber = -1;
                    int messageSize = -1;
                    string[] values = response.Split(' ');
                    int.TryParse(values[0], out messageNumber);
                    int.TryParse(values[1], out messageSize);
                    messagesInfo.Add(new MessageInfo(messageNumber, messageSize, ""));
                }
                foreach (MessageInfo mi in messagesInfo)
                {
                    mi.UniqueID = GetUid(mi.MessageNumber);
                }
                this.messages = messagesInfo;
                messagesUpdated = true;
            }
            else
            {
                this.messages = messagesInfo;
            }
            return messagesUpdated;
        }
        #endregion

        #region Exceptions
        /// <summary>
        /// Pop3Exception is the default exception which is raised on any error in the Pop3Client
        /// </summary>
        public class Pop3Exception : Exception
        {
            public Pop3Exception() { }
            public Pop3Exception(string message) : base(message) { }
            public Pop3Exception(string message, Exception innerException) : base(message, innerException) { }
        }
        #endregion

        #region Enumerations
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
        #endregion

        #region Additional Types
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
        #endregion
    }

}
