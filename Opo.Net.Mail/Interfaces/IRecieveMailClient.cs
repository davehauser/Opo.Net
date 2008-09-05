using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Opo.Net.Mail
{
    public interface IRecieveMailClient
    {
        // Properties
        string Host { get; set; }
        int Port { get; set; }
        string Username { get; set; }
        string Password { get; set; }
        bool UseSSL { get; set; }
        bool DeleteMessagesAfterDownload { get; set; }

        // Methods
        string Connect();
        void Disconnect();
        bool Login(string username, string password);
        bool Logout();
        string GetMessage(string id);
        bool DeleteMessage(string id);
    }
}