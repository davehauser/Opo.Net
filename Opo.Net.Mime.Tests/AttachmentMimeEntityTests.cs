using System;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Moq;
using System.IO;

namespace Opo.Net.Mime
{
    [TestFixture]
    public class AttachmentMimeEntityTests
    {
        Mock<IMimeParser> _mimeParser = new Mock<IMimeParser>();

        [TestFixtureSetUp]
        public void Setup()
        {
            _mimeParser.Expect(m => m.ParseHeader(TestMimeMessage.mimeData, "Content-Transfer-Encoding")).Returns(TestMimeMessage.attachmentContentTransferEncoding);
            _mimeParser.Expect(m => m.ParseContent(TestMimeMessage.mimeData)).Returns(TestMimeMessage.content);
            _mimeParser.Expect(m => m.ParseContentType(TestMimeMessage.mimeData)).Returns(TestMimeMessage.contentType);
        }

        [TestFixtureTearDown]
        public void Teardown()
        {

        }

        [Test]
        public void CanCreateAttachmentMimeEntity()
        {
            AttachmentMimeEntity mimeEntity = new AttachmentMimeEntity(_mimeParser.Object, TestMimeMessage.mimeData);
            Assert.That(mimeEntity.ContentType, Is.EqualTo(TestMimeMessage.contentType));
            Assert.That(mimeEntity.ContentTransferEncoding, Is.EqualTo(TestMimeMessage.attachmentContentTransferEncoding));

            mimeEntity = new AttachmentMimeEntity(_mimeParser.Object, TestMimeMessage.mimeData, TestMimeMessage.contentType);
            Assert.That(mimeEntity.ContentType, Is.EqualTo(TestMimeMessage.contentType));
            Assert.That(mimeEntity.MimeData, Is.EqualTo(TestMimeMessage.mimeData));
        }

        [Test]
        public void CanGetContentStream()
        {
            AttachmentMimeEntity mimeEntity = new AttachmentMimeEntity(_mimeParser.Object, TestMimeMessage.contentType, TestMimeMessage.mimeData);
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            using (MemoryStream expected = new MemoryStream(encoding.GetBytes(TestMimeMessage.content)))
            {
                System.Diagnostics.Debug.WriteLine("");
                Assert.That(mimeEntity.GetContent().Length, Is.EqualTo(expected.Length));
            }
        }
    }
}
