using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Opo.Net.Mail;
using Opo.Net.Mime;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Linq;

namespace Opo.Net.TestProject
{
    class Program
    {
        static void Main(string[] args)
        {
            Pop3Client pop3 = new Pop3Client("pop.opo.li", 110, "test@opo.li", "test");
            pop3.Connect();
            pop3.Login();
            //IList<string> messages = new List<string>();
            //XmlMailMessageConverter xmlConverter = new XmlMailMessageConverter();
            //for (int i = 1; i <= pop3.Mailbox.NumberOfMessages; i++)
            //{
            //    string message = pop3.GetMessageHeaders(i);
            //    IMailMessageConverter converter = new MimeMailMessageConverter(new RegexMimeParser());
            //    IMailMessage mailMessage = converter.ConvertFrom(message);
            //    //Console.WriteLine("Subject:  " + mailMessage.Subject);
            //    //Console.WriteLine("From:     " + mailMessage.From.ToString());
            //    //Console.WriteLine("To:       " + mailMessage.To.ToString());
            //    //Console.WriteLine("-------------------------------------------");
            //    //Console.WriteLine(mailMessage.Body);
            //    //Console.WriteLine("-------------------------------------------");
            //    //Console.WriteLine();
            //    //Console.WriteLine();
            //    XDocument xml = (XDocument)xmlConverter.ConvertTo(mailMessage);
            //    string xmlMessage = xml.ToString();
            //    Console.WriteLine(xmlMessage);
            //}
            pop3.Logout();
            Console.WriteLine("First Logout");
            pop3.Login();
            pop3.Logout();
            Console.WriteLine("Second Logout");
            pop3.Disconnect();

            Console.ReadLine();
        }
    }
}
