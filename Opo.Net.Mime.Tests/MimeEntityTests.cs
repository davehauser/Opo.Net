using System;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Moq;

namespace Opo.Net.Mime
{
    [TestFixture(Description = "Tests for Opo.Net.Mime.MimeEntity")]
    public class MimeEntityTests
    {
        private Mock<IMimeParser> _mimeParser = new Mock<IMimeParser>();

        [TestFixtureSetUp]
        public void Setup()
        {
            _mimeParser.Expect(m => m.ParseContentType(It.IsAny<string>())).Returns("text/plain");
            _mimeParser.Expect(m => m.ParseBoundary(TestMimeMessage.mimeData)).Returns(TestMimeMessage.boundaryLevel1);
            _mimeParser.Expect(m => m.ParseContentType(TestMimeMessage.mimeData)).Returns("multipart/mixed");
            _mimeParser.Expect(m => m.ParseBoundary(It.Is<string>(s => s != TestMimeMessage.mimeData))).Returns(TestMimeMessage.boundaryLevel2);
            _mimeParser.Expect(m => m.ParseContentType(TestMimeMessage.multipartAlternativePart)).Returns("multipart/alternative");
            _mimeParser.Expect(m => m.ParseContentType(TestMimeMessage.textPart)).Returns("text/plain");
            _mimeParser.Expect(m => m.ParseContentType(TestMimeMessage.htmlPart)).Returns("text/html");
            _mimeParser.Expect(m => m.ParseContentType(TestMimeMessage.attachmentPart)).Returns("image/gif");
        }

        [TestFixtureTearDown]
        public void Teardown()
        {

        }

        [Test]
        public void CanGetInstanceByMimeData()
        {
            Assert.That(MimeEntity.GetInstance(_mimeParser.Object, TestMimeMessage.mimeData), Is.TypeOf(typeof(MultipartMimeEntity)));
            Assert.That(MimeEntity.GetInstance(_mimeParser.Object, TestMimeMessage.multipartAlternativePart), Is.TypeOf(typeof(MultipartMimeEntity)));
            Assert.That(MimeEntity.GetInstance(_mimeParser.Object, TestMimeMessage.textPart), Is.TypeOf(typeof(TextMimeEntity)));
            Assert.That(MimeEntity.GetInstance(_mimeParser.Object, TestMimeMessage.htmlPart), Is.TypeOf(typeof(TextMimeEntity)));
            Assert.That(MimeEntity.GetInstance(_mimeParser.Object, TestMimeMessage.attachmentPart), Is.TypeOf(typeof(AttachmentMimeEntity)));
        }
    }
}
