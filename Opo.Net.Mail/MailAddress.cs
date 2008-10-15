using System;
using System.Text.RegularExpressions;

namespace Opo.Net.Mail
{
    public class MailAddress : IMailAddress
    {
        /// <summary>
        /// Address part of the mail address, e.g. "email@sample.org"
        /// </summary>
        private string _address;
        
        /// <summary>
        /// Gets or sets the address part of the mail address, e.g. "email.sample.org"
        /// </summary>
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
        public string DisplayName { get; set; }
        
        /// <summary>
        /// Gets or sets the domain part of the address (the part after the &#64;)
        /// </summary>
        public string Domain
        {
            get
            {
                return GetDomain(this);
            }
            set
            {
                this._address = String.Format("{0}@{1}", GetAccountName(this), value);
            }
        }

        /// <summary>
        /// Gets or sets the account name of the address (the part before the &#64;)
        /// </summary>
        public string AccountName
        {
            get
            {
                return GetAccountName(this);
            }
            set
            {
                this._address = String.Format("{0}@{1}", value, GetDomain(this));
            }
        }

        public MailAddress(string address)
        {
            this.Address = address;
            this.DisplayName = "";
        }
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
            return String.Format(format, this.Address, this.DisplayName, this.AccountName, this.Domain);
        }

        #region Static methods
        public static MailAddress Parse(string mailAddress)
        {
            Regex r = new Regex(@"(?<Address>([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?))");
            Match m = r.Match(mailAddress);
            if (m.Groups["Address"].Length <= 0)
                throw new ArgumentException("mailAddress contains no valid email address.");
            MailAddress address = new MailAddress(m.Groups["Address"].Value);
            address.DisplayName = r.Replace(mailAddress, "").Replace("\"", "").Replace("<", "").Replace(">", "").Trim();
            return address;
        }
        public static string GetDomain(MailAddress mailAddress)
        {
            string a = mailAddress.Address;
            return a.Substring(a.IndexOf('@') + 1);
        }
        public static string GetAccountName(MailAddress mailAddress)
        {
            string a = mailAddress.Address;
            return a.Substring(0, a.IndexOf('@'));
        }
        #endregion
    }
}
