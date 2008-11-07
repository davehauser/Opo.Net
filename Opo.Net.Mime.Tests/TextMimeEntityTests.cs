using System;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Moq;

namespace Opo.Net.Mime
{
    [TestFixture(Description = "Tests for Opo.Net.TextMimeEntity")]
    public class TextMimeEntityTests
    {
        private Mock<IMimeParser> _mimeParser = new Mock<IMimeParser>();

        [TestFixtureSetUp]
        public void Setup()
        {
            _mimeParser.Expect(m => m.ParseContent(TestMimeMessage.textPart)).Returns(TestMimeMessage.content);
            _mimeParser.Expect(m => m.ParseContentType(TestMimeMessage.textPart)).Returns(TestMimeMessage.contentType);
            _mimeParser.Expect(m => m.ParseCharset(TestMimeMessage.textPart)).Returns(TestMimeMessage.textBodyCharset);
        }

        [TestFixtureTearDown]
        public void Teardown()
        {

        }

        [Test]
        public void CanCreateTextMimeEntity()
        {
            TextMimeEntity mimeEntity = new TextMimeEntity(_mimeParser.Object, TestMimeMessage.textPart);
            Assert.That(mimeEntity.GetMimeData(), Is.EqualTo(TestMimeMessage.textPart.Trim()));
            Assert.That(mimeEntity.ContentType, Is.EqualTo(TestMimeMessage.textBodyContentType));
            Assert.That(mimeEntity.ContentTransferEncoding, Is.EqualTo(TestMimeMessage.textBodyContentTransferEncoding));
            Assert.That(mimeEntity.Charset, Is.EqualTo(TestMimeMessage.textBodyCharset));
        }

        [Test]
        public void CanGetContent()
        {
            TextMimeEntity mimeEntity = new TextMimeEntity(_mimeParser.Object, TestMimeMessage.mimeData);
            Assert.That(mimeEntity.GetContent(), Is.EqualTo(TestMimeMessage.content));
        }
    }
}
