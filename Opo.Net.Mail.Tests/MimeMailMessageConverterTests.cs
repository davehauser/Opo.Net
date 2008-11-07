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
        //Mock<IMimeParser> _mimeParser = new Mock<IMimeParser>();
        IMimeParser _mimeParser = new RegexMimeParser();

        [TestFixtureSetUp]
        public void Setup()
        {
            //_mimeParser.Expect(m => m.ParseHeaderValue(It.IsAny<string>(), "Subject")).Returns(TestMimeMessage.subject);
            //_mimeParser.Expect(m => m.ParseHeaderValue(It.IsAny<string>(), "From")).Returns(TestMimeMessage.from);
            //_mimeParser.Expect(m => m.ParseHeaderValue(It.IsAny<string>(), "To")).Returns(TestMimeMessage.toPlain);
            //_mimeParser.Expect(m => m.ParseHeaderValue(It.IsAny<string>(), "CC")).Returns(TestMimeMessage.cc);
            //_mimeParser.Expect(m => m.ParseHeaderValue(It.IsAny<string>(), "BCC")).Returns(TestMimeMessage.bcc);
            //_mimeParser.Expect(m => m.ParseHeaderValue(It.IsAny<string>(), "Message-ID")).Returns(TestMimeMessage.messageID);
            //_mimeParser.Expect(m => m.ParseHeaderValue(It.IsAny<string>(), "Date")).Returns(TestMimeMessage.messageDate);
            //_mimeParser.Expect(m => m.ParseHeaderValue(It.IsAny<string>(), "X-Priority")).Returns(TestMimeMessage.priority);
            //_mimeParser.Expect(m => m.ParseHeaderValue(It.IsAny<string>(), "References")).Returns(TestMimeMessage.references);
            //_mimeParser.Expect(m => m.ParseContent(It.IsAny<string>())).Returns(TestMimeMessage.content);
            //_mimeParser.Expect(m => m.ParseCharset(TestMimeMessage.textPart)).Returns(TestMimeMessage.textBodyCharset);
            //_mimeParser.Expect(m => m.ParseCharset(TestMimeMessage.htmlPart)).Returns(TestMimeMessage.htmlBodyCharset);
            //_mimeParser.Expect(m => m.ParseContentType(It.IsAny<string>())).Returns(TestMimeMessage.contentType);
            //_mimeParser.Expect(m => m.ParseContentType(TestMimeMessage.textPart)).Returns(TestMimeMessage.textBodyContentType);
            //_mimeParser.Expect(m => m.ParseContentType(TestMimeMessage.htmlPart)).Returns(TestMimeMessage.htmlBodyContentType);
            //_mimeParser.Expect(m => m.ParseContentType(TestMimeMessage.attachmentPart)).Returns(TestMimeMessage.attachmentContentType);
        }

        [TestFixtureTearDown]
        public void Teardown()
        {

        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void CannotConvertInt()
        {
            IMailMessageConverter mimeMailMessageConverter = new MimeMailMessageConverter(_mimeParser);
            IMailMessage mailMessage = mimeMailMessageConverter.ConvertFrom(123);
        }

        [Test]
        public void CanConvertMimeEntity()
        {
            IMailMessageConverter mimeMailMessageConverter = new MimeMailMessageConverter();
            IMailMessage mailMessage = mimeMailMessageConverter.ConvertFrom(MimeEntity.GetInstance(_mimeParser, TestMimeMessage.mimeData));
            Assert.That(mailMessage.MessageID, Is.EqualTo(TestMimeMessage.messageID));
            Assert.That(mailMessage.Subject, Is.EqualTo(TestMimeMessage.subject));
            Assert.That(mailMessage.From.ToString(), Is.EqualTo(TestMimeMessage.emailAdresses[0]));
            Assert.That(mailMessage.AlternativeViews.Count, Is.EqualTo(1));
            Assert.That(mailMessage.Body, Is.EqualTo(TestMimeMessage.textBodyDecoded));
            Assert.That(mailMessage.BodyType, Is.EqualTo(MailMessageBodyType.PlainText));

        }

        [Test]
        public void CanConvertMimeData()
        {
            MimeMailMessageConverter mimeMailMessageConverter = new MimeMailMessageConverter(_mimeParser);
            IMailMessage mailMessage = mimeMailMessageConverter.ConvertFrom(TestMimeMessage.mimeData);
            Assert.That(mailMessage.MessageID, Is.EqualTo(TestMimeMessage.messageID));
            Assert.That(mailMessage.Subject, Is.EqualTo(TestMimeMessage.subject));
            Assert.That(mailMessage.From.ToString(), Is.EqualTo(TestMimeMessage.emailAdresses[0]));
            Assert.That(mailMessage.AlternativeViews.Count, Is.EqualTo(1));
            Assert.That(mailMessage.Body, Is.EqualTo(TestMimeMessage.textBodyDecoded));
            Assert.That(mailMessage.BodyType, Is.EqualTo(MailMessageBodyType.PlainText));
        }
    }
}
