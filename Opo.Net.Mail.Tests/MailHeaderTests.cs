using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace Opo.Net.Mail
{
    [TestFixture(Description = "Tests for MailHeader")]
    public class MailHeaderTests
    {
        [Test]
        public void CanCreateMailHeader()
        {
            MailHeader mailHeader = new MailHeader("name", "value");

            Assert.That(mailHeader.Name, Is.EqualTo("name"));
            Assert.That(mailHeader.Value, Is.EqualTo("value"));
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void CannotCreateMailHeaderWithEmptyName()
        {
            MailHeader mailHeader = new MailHeader("", "value");
        }

        [Test]
        public void CanCreateString()
        {
            MailHeader mailHeader = new MailHeader("name", "value");

            Assert.That(mailHeader.ToString(), Is.EqualTo("name: value"));
        }
    }
}
