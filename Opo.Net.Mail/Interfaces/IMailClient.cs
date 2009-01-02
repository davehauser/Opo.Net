namespace Opo.Net.Mail
{
    public interface IMailClient
    {
        /// <summary>
        /// Gets or sets the Host of the mail server
        /// </summary>
        string Host { get; set; }
        /// <summary>
        /// Gets or sets the mail servers port
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
    }
}
