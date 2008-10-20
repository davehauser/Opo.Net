namespace Opo.Net.Mail
{
    /// <summary>
    /// Represents a email address
    /// </summary>
    public interface IMailAddress
    {
        /// <summary>
        /// Gets or sets the address part of the mail address, e.g. "email@sample.org"
        /// </summary>
        string Address { get; set; }
        /// <summary>
        /// Gets or sets the name, that is displayed with the address, e.g "Sample Email Account"
        /// </summary>
        string DisplayName { get; set; }
        /// <summary>
        /// Gets or sets the account name of the address (the part before the @)
        /// </summary>
        string AccountName { get; set; }
        /// <summary>
        /// Gets or sets the host part of the address (the part after the @)
        /// </summary>
        string Host { get; set; }
    }
}
