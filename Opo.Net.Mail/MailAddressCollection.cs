using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Opo.Net.Mail
{
    public class MailAddressCollection : List<IMailAddress>
    {
        public MailAddressCollection() { }

        public MailAddressCollection(IEnumerable<IMailAddress> mailAddresses)
            : base(mailAddresses) { }

        public override string ToString()
        {
            return ToString(";");
        }
        
        public string ToString(string separator)
        {
            return String.Join(separator, this.Select(m => m.ToString()).ToArray());
        }
    }
}
