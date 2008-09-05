using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Moq;

namespace Opo.Net.Mail
{
    [TestFixture(Description = "Tests for MailMessage")]
    public class MailMessageTests
    {
        private string _subject;
        private string _body;
        private Mock<IMailAddress> _to;
        private Mock<IMailAddress> _from;
        private MailMessageBodyType _type;

        [TestFixtureSetUp]
        public void Setup()
        {
            _subject = "Message Subject";
            _body = "<b>This is a HTML message body.</b>";
            _to = new Mock<IMailAddress>();
            _to.Expect(a => a.Address).Returns("to@example.org");
            _to.Expect(a => a.DisplayName).Returns("To Address");
            _from = new Mock<IMailAddress>();
            _from.Expect(a => a.Address).Returns("from@example.org");
            _from.Expect(a => a.DisplayName).Returns("From Address");
            _type = MailMessageBodyType.Html;
        }

        [Test]
        public void CanCreateMailMessage()
        {
            MailMessage mailMessage = new MailMessage();
            Assert.That(mailMessage.BodyType, Is.EqualTo(MailMessageBodyType.Default));

            mailMessage = new MailMessage(_from.Object, _to.Object, _subject, _body, _type);
            Assert.That(mailMessage.From.Address, Is.EqualTo("from@example.org"));
            Assert.That(mailMessage.From.DisplayName, Is.EqualTo("From Address"));
            Assert.That(mailMessage.To.First().Address, Is.EqualTo("to@example.org"));
            Assert.That(mailMessage.To.First().DisplayName, Is.EqualTo("To Address"));
            Assert.That(mailMessage.Subject, Is.EqualTo(_subject));
            Assert.That(mailMessage.Body, Is.EqualTo(_body));
            Assert.That(mailMessage.BodyType, Is.EqualTo(MailMessageBodyType.Html));
        }
    }
}
