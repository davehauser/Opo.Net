using System;
using System.Collections.Generic;
using System.Linq;

namespace Opo.Net.Mail
{
    public class MailHeader
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public MailHeader(string name, string value)
        {
            if (name.Equals(String.Empty))
                throw new ArgumentException("[name] cannot be empty");
            Name = name;
            Value = value;
        }

        public override string ToString()
        {
            return String.Concat(Name, ": ", Value);
        }
    }
}
