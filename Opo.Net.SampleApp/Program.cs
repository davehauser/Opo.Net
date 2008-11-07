using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Opo.Net.Mail;
using Opo.Net.Mime;

namespace Opo.Net.TestProject
{
    class Program
    {
        static void Main(string[] args)
        {
            Pop3Client pop3 = new Pop3Client("pop.opo.li", 110, "test@opo.li", "test");
            pop3.Connect();
            pop3.Login();
            string mimeData = pop3.GetMessage(1);
            Console.WriteLine(mimeData);
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("-------------------------------------------");
            pop3.Logout();
            pop3.Disconnect();
            IMailMessageConverter converter = new MimeMailMessageConverter(new RegexMimeParser());
            IMailMessage message = converter.ConvertFrom(mimeData);
            Console.WriteLine(message.Subject);
            Console.WriteLine("-------------------------------------------");
            Console.WriteLine(message.Body);
            Console.ReadLine();
        }
    }
}
