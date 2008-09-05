using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace Opo.Net.Mail
{
    [TestFixture(Description = "Tests for MailAddressCollection")]
    public class MailAddressCollectionTests
    {
        IMailAddress[] mailAddresses;
        
        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            mailAddresses = new IMailAddress[3];
            for (int i = 0; i < 3; i++)
            {
                mailAddresses[i] = new MailAddress("email" + i.ToString() + "@example.org", i % 2 == 0 ? "Email " + i.ToString() : "");
            }
        }

        [Test]
        public void CanCreateMailAddressCollection()
        {
            MailAddressCollection mailAddressCollection = new MailAddressCollection();
            foreach (var mailAddress in mailAddresses)
            {
                mailAddressCollection.Add(mailAddress);
            }

            Assert.That(mailAddressCollection.Count, Is.EqualTo(3));
        }

        [Test]
        public void CanCreateMailAddressCollectionFromArray()
        {
            MailAddressCollection mailAddressCollection = new MailAddressCollection(mailAddresses);

            Assert.That(mailAddressCollection.Count, Is.EqualTo(3));
            for (int i = 0; i < 3; i++)
            {
                Assert.That(mailAddressCollection.ElementAt(i), Is.EqualTo(mailAddresses[i]));
            }
        }

        [Test]
        public void CanCreateStringFromAddressesWithDefaultSeparator()
        {
            MailAddressCollection mailAddressCollection = new MailAddressCollection(mailAddresses);

            string separator = ";";
            string expected = "\"Email 0\" <email0@example.org>" + separator + "email1@example.org" + separator + "\"Email 2\" <email2@example.org>";

            Assert.That(mailAddressCollection.ToString(), Is.EqualTo(expected));
        }

        [Test]
        public void CanCreateStringFromAddressesWithCustomSeparator()
        {
            MailAddressCollection mailAddressCollection = new MailAddressCollection(mailAddresses);

            string separator = ", ";
            string expected = "\"Email 0\" <email0@example.org>" + separator + "email1@example.org" + separator + "\"Email 2\" <email2@example.org>";

            Assert.That(mailAddressCollection.ToString(separator), Is.EqualTo(expected));
        }
    }
}