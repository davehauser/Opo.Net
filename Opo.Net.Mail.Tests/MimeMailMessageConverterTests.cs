﻿using System;
using System.Collections.Generic;
using Opo.Net.Mime;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Moq;

namespace Opo.Net.Mail
{
    [TestFixture(Description = "Tests for Opo.Net.Mail.MimeMailConverter")]
    public class MimeMailMessageConverterTests
    {
        Mock<IMimeParser> _mimeParser = new Mock<IMimeParser>();

        [TestFixtureSetUp]
        public void Setup()
        {
            _mimeParser.Expect(m => m.ParseHeaderValue(It.IsAny<string>(), "Subject")).Returns(TestMimeMessage.subject);
            _mimeParser.Expect(m => m.ParseHeaderValue(It.IsAny<string>(), "From")).Returns(TestMimeMessage.from);
            _mimeParser.Expect(m => m.ParseHeaderValue(It.IsAny<string>(), "Message-ID")).Returns(TestMimeMessage.messageID);
            _mimeParser.Expect(m => m.ParseHeaderValue(It.IsAny<string>(), "Date")).Returns(TestMimeMessage.messageDate);
            _mimeParser.Expect(m => m.ParseHeaderValue(It.IsAny<string>(), "X-Priority")).Returns(TestMimeMessage.priority);
            _mimeParser.Expect(m => m.ParseHeaderValue(It.IsAny<string>(), "References")).Returns(TestMimeMessage.references);
            _mimeParser.Expect(m => m.ParseContent(It.IsAny<string>())).Returns(TestMimeMessage.content);
            _mimeParser.Expect(m => m.ParseCharset(TestMimeMessage.textPart)).Returns(TestMimeMessage.textBodyCharset);
            _mimeParser.Expect(m => m.ParseCharset(TestMimeMessage.htmlPart)).Returns(TestMimeMessage.htmlBodyCharset);
            _mimeParser.Expect(m => m.ParseContentType(It.IsAny<string>())).Returns(TestMimeMessage.contentType);
            _mimeParser.Expect(m => m.ParseContentType(TestMimeMessage.textPart)).Returns(TestMimeMessage.textBodyContentType);
            _mimeParser.Expect(m => m.ParseContentType(TestMimeMessage.htmlPart)).Returns(TestMimeMessage.htmlBodyContentType);
            _mimeParser.Expect(m => m.ParseContentType(TestMimeMessage.attachmentPart)).Returns(TestMimeMessage.attachmentContentType);
        }

        [TestFixtureTearDown]
        public void Teardown()
        {

        }

        [Test]
        public void CanConvertMimeDataToMailMessage()
        {
            MimeMailMessageConverter mimeMailMessageConverter = new MimeMailMessageConverter(_mimeParser.Object);
            IMailMessage mailMessage = mimeMailMessageConverter.ConvertFrom(TestMimeMessage.mimeData);

            Assert.That(mailMessage.Subject, Is.EqualTo(TestMimeMessage.subject));
            Assert.That(mailMessage.From, Is.EqualTo(MailAddress.Parse(TestMimeMessage.emailAdresses[0])));
            IList<string> addresses = new List<string>(TestMimeMessage.emailAdresses);
            MailAddressCollection mailAddressCollection = new MailAddressCollection();
            foreach (string address in addresses)
            {
                mailAddressCollection.Add(MailAddress.Parse(address));
            }
            Assert.That(mailMessage.To, Is.EqualTo(mailAddressCollection));
            Assert.That(mailMessage.Cc, Is.EqualTo(mailAddressCollection));
        }
    }
}
