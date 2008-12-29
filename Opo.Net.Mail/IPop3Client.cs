using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Opo.Net.Mail
{
    public interface IPop3Client : IRecieveMailClient, ISendMailClient
    {
        string ApopTimestamp { get; }
        bool UseSSL { get; set; }
        bool AutoReconnect { get; set; }
        Pop3SessionState State { get; }
    }
}
