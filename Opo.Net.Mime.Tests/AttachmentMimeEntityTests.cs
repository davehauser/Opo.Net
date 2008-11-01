using System;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Moq;
using System.IO;

namespace Opo.Net.Mime
{
    [TestFixture(Description = "Tests for Opo.Net.Mime.AttachmentMimeEntity")]
    public class AttachmentMimeEntityTests
    {
        Mock<IMimeParser> _mimeParser = new Mock<IMimeParser>();

        [TestFixtureSetUp]
        public void Setup()
        {
            _mimeParser.Expect(m => m.ParseHeaderValue(TestMimeMessage.mimeData, "Content-Transfer-Encoding")).Returns(TestMimeMessage.attachmentContentTransferEncoding);
            _mimeParser.Expect(m => m.ParseContent(TestMimeMessage.mimeData)).Returns(TestMimeMessage.attachmentContent);
            _mimeParser.Expect(m => m.ParseContentType(TestMimeMessage.mimeData)).Returns(TestMimeMessage.attachmentContentType);
        }

        [TestFixtureTearDown]
        public void Teardown()
        {

        }

        [Test]
        public void CanCreateAttachmentMimeEntity()
        {
            IMimeEntity mimeEntity = new AttachmentMimeEntity(_mimeParser.Object, TestMimeMessage.attachmentPart);
            Assert.That(mimeEntity.ContentType, Is.EqualTo(TestMimeMessage.attachmentContentType));

            mimeEntity = new AttachmentMimeEntity(_mimeParser.Object, TestMimeMessage.attachmentPart);
            Assert.That(mimeEntity.ContentType, Is.EqualTo(TestMimeMessage.attachmentContentType));
            Assert.That(mimeEntity.ContentTransferEncoding, Is.EqualTo(TestMimeMessage.attachmentContentTransferEncoding));
            Assert.That(mimeEntity.GetMimeData(), Is.EqualTo(TestMimeMessage.attachmentPart));
        }

        [Test]
        public void CanGetContentStream()
        {
            AttachmentMimeEntity mimeEntity = new AttachmentMimeEntity(_mimeParser.Object, TestMimeMessage.contentType);
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            using (MemoryStream expected = new MemoryStream(encoding.GetBytes(TestMimeMessage.content)))
            {
                Assert.That(mimeEntity.GetContent().Length, Is.EqualTo(expected.Length));
            }
        }
    }
}
