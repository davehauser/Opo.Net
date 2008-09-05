using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Opo.Net.Mime;
using System.IO;

namespace Opo.Net.Mail
{
    [TestFixture]
    public class AttachmentTests
    {
        private string _name;
        private string _content;
        private string _contentType;
        private string _transferEncoding;

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            _name = "Attachment Name";
            _content = "Attachment Content";
            _contentType = Mime.MediaType.Text.Plain;
            _transferEncoding = Mime.TransferEncoding.QuotedPrintable;
        }

        [Test]
        public void CanCreateAttachment()
        {
            Attachment attachment = new Attachment(_name, _content, _contentType, _transferEncoding);

            Assert.That(attachment.Name, Is.EqualTo(_name));
            Assert.That(attachment.ContentType, Is.EqualTo(_contentType));
            Assert.That(attachment.TransferEncoding, Is.EqualTo(_transferEncoding));
            Assert.That(attachment.Size, Is.EqualTo(_content.Length));
            Assert.That(attachment.ToString(), Is.EqualTo(_name));
            Assert.That(attachment.ContentDisposition, Is.Not.Null);
        }

        [Test]
        public void CanGetContentStream()
        {
            Attachment attachment = new Attachment(_name, _content, _contentType, _transferEncoding);

            using (StreamReader reader = new StreamReader(attachment.GetContentStream()))
            {
                Assert.That(reader.ReadToEnd(), Is.EqualTo(_content));
            }
        }
    }
}
