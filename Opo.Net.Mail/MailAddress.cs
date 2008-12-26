using System;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.Xml;

namespace Opo.Net.Mail
{
    [XmlRoot("mailAddress")]
    public class MailAddress : IMailAddress
    {
        private string _address;

        /// <summary>
        /// Gets or sets the address part of the mail address, e.g. "email@sample.org"
        /// </summary>
        [XmlElement("address")]
        public string Address
        {
            get { return _address; }
            set
            {
                string tempValue = value.Trim();
                Regex r = new Regex(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
                if (r.IsMatch(tempValue))
                    this._address = tempValue;
                else
                    throw new ArgumentException("\"" + value + "\" is not a valid email address.");
            }
        }

        /// <summary>
        /// Gets or sets the name, that is displayed with the address, e.g "Sample Email Account"
        /// </summary>
        [XmlElement("displayName")]
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the host part of the address (the part after the @)
        /// </summary>
        [XmlIgnore]
        public string Host
        {
            get
            {
                return GetHost(this);
            }
            set
            {
                this._address = String.Format("{0}@{1}", GetAccountName(this), value);
            }
        }

        /// <summary>
        /// Gets or sets the account name of the address (the part before the @)
        /// </summary>
        [XmlIgnore]
        public string AccountName
        {
            get
            {
                return GetAccountName(this);
            }
            set
            {
                this._address = String.Format("{0}@{1}", value, GetHost(this));
            }
        }

        /// <summary>
        /// Initializes a new instance of the MailAddress class
        /// </summary>
        /// <param name="address">A String containing a valid email address</param>
        public MailAddress(string address)
        {
            this.Address = address;
            this.DisplayName = "";
        }
        /// <summary>
        /// Initializes a new instance of the MailAddress class
        /// </summary>
        /// <param name="address">A String containing a valid email address</param>
        /// <param name="displayName">A String containing the display name</param>
        public MailAddress(string address, string displayName)
        {
            this.Address = address;
            this.DisplayName = displayName;
        }
        public override string ToString()
        {
            return (!String.IsNullOrEmpty(DisplayName)) ? String.Concat("\"", DisplayName, "\" <", _address, ">") : _address;
        }
        /// <summary>
        /// Returns a formatted mail address. Use {0} or {address} for the Address part and {1} or {displayname} for the DisplayName part.
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        public string ToString(string format)
        {
            format = format.Replace("{address", "{0")
                           .Replace("{displayname", "{1")
                           .Replace("{accountname", "{2")
                           .Replace("{domain", "{3");
            return String.Format(format, this.Address, this.DisplayName, this.AccountName, this.Host);
        }

        public override bool Equals(object obj)
        {
            IMailAddress mailAddress = obj as IMailAddress;
            if (mailAddress != null)
            {
                return (Address == mailAddress.Address && DisplayName == mailAddress.DisplayName);
            }
            else
            {
                return false;
            }
        }

        #region Static methods
        /// <summary>
        /// Parses a email address string
        /// </summary>
        /// <param name="mailAddress">A string containing a valid email address</param>
        /// <returns>A new instance of an IMailAddress implementation</returns>
        /// <exception cref="ArgumentException">Throws a ArgumentException if the email address is not valid</exception>
        public static IMailAddress Parse(string mailAddress)
        {
            Regex r = new Regex(@"(?<Address>([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?))");
            Match m = r.Match(mailAddress);
            if (m.Groups["Address"].Length <= 0)
                throw new ArgumentException("mailAddress contains no valid email address.");
            MailAddress address = new MailAddress(m.Groups["Address"].Value);
            address.DisplayName = r.Replace(mailAddress, "").Replace("\"", "").Replace("<", "").Replace(">", "").Trim();
            return address;
        }
        /// <summary>
        /// Gets the host part of a mail address
        /// </summary>
        /// <param name="mailAddress">Instance of the MailAddress class</param>
        /// <returns>A String containing the host part of the mail address</returns>
        public static string GetHost(MailAddress mailAddress)
        {
            string a = mailAddress.Address;
            return a.Substring(a.IndexOf('@') + 1);
        }
        /// <summary>
        /// Gets the account name part of a mail address
        /// </summary>
        /// <param name="mailAddress">Instance of the MailAddress class</param>
        /// <returns>A String containing the account name part of the mail address</returns>
        public static string GetAccountName(MailAddress mailAddress)
        {
            string a = mailAddress.Address;
            return a.Substring(0, a.IndexOf('@'));
        }
        #endregion
    }
}
