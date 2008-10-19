using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace Opo.Net.Mail
{
    [TestFixture(Description = "Tests for Opo.Net.Mail.Pop3Client")]
    public class Pop3ClientTests
    {
        private string host = "example.org";
        private int port = 110;
        private string username = "Username";
        private string password = "Password";

        [Test]
        public void CanCreatePop3Client()
        {
            Pop3Client pop3Client = new Pop3Client(host, port, username, password);

            Assert.That(pop3Client.Host, Is.EqualTo(host));
            Assert.That(pop3Client.Port, Is.EqualTo(port));
            Assert.That(pop3Client.Username, Is.EqualTo(username));
            Assert.That(pop3Client.Password, Is.EqualTo(password));
        }

        [Test]
        public void CanCreateMailboxInfo()
        {
            Pop3Client.MailboxInfo mailboxInfo = new Pop3Client.MailboxInfo(1, 1);

            Assert.That(mailboxInfo.NumberOfMessages, Is.EqualTo(1));
            Assert.That(mailboxInfo.MailboxSize, Is.EqualTo(1));
        }

        [Test]
        public void CanCreateMessageInfo()
        {
            Pop3Client.MessageInfo messageInfo = new Pop3Client.MessageInfo(1, 1, "1");

            Assert.That(messageInfo.MessageNumber, Is.EqualTo(1));
            Assert.That(messageInfo.Size, Is.EqualTo(1));
            Assert.That(messageInfo.UniqueID, Is.EqualTo("1"));
        }

        [Test]
        public void CanCreateMessageInfoCollection()
        {
            Pop3Client.MessageInfoCollection messageInfoCollection = new Pop3Client.MessageInfoCollection();
            messageInfoCollection.Add(new Pop3Client.MessageInfo(1, 1, "1"));
            messageInfoCollection.Add(new Pop3Client.MessageInfo(2, 2, "2"));
            messageInfoCollection.Add(new Pop3Client.MessageInfo(3, 3, "3"));

            Assert.That(messageInfoCollection.Count, Is.EqualTo(3));
            Assert.That(messageInfoCollection.ElementAt<Pop3Client.MessageInfo>(0).MessageNumber, Is.EqualTo(1));
            Assert.That(messageInfoCollection.ElementAt<Pop3Client.MessageInfo>(1).MessageNumber, Is.EqualTo(2));
            Assert.That(messageInfoCollection.ElementAt<Pop3Client.MessageInfo>(2).MessageNumber, Is.EqualTo(3));
        }
    }
}
