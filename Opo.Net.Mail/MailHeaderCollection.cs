using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Opo.Net.Mail
{
    public class MailHeaderCollection : List<MailHeader>
    {
        public void Add(string name, string value)
        {
            this.Add(new MailHeader(name, value));
        }
        public void Remove(string name)
        {
            this.RemoveAll(h => h.Name == name);
        }
        public string GetValue(string name)
        {
            return this.FirstOrDefault(h => h.Name == name).Value;
        }
    }
}
