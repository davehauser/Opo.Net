using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Opo.Net.Mail
{
    public interface ISendMailClient
    {
        string Host {get; set;}
        int Port {get; set;}
        string Username { get; set; }
        string Password { get; set; }
        bool UseSSL { get; set; }
    }
}
