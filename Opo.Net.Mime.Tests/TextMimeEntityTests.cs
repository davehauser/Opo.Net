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
            _mimeParser.Expect(m => m.ParseContent(TestMimeMessage.mimeData)).Returns(TestMimeMessage.content);
            _mimeParser.Expect(m => m.ParseContentType(TestMimeMessage.mimeData)).Returns(TestMimeMessage.contentType);
        }

        [TestFixtureTearDown]
        public void Teardown()
        {

        }

        [Test]
        public void CanCreateTextMimeEntity()
        {
            TextMimeEntity mimeEntity = new TextMimeEntity(_mimeParser.Object, TestMimeMessage.textPart);
            foreach (var item in mimeEntity.Headers)
            {
                System.Diagnostics.Debug.WriteLine(item.Key + ": " + item.Value);
            }
            System.Diagnostics.Debug.WriteLine("-----------------------------------");
            System.Diagnostics.Debug.WriteLine(mimeEntity.Content);
            Assert.That(mimeEntity.GetMimeData(), Is.EqualTo(TestMimeMessage.textPart));
            Assert.That(mimeEntity.ContentType, Is.EqualTo(TestMimeMessage.textBodyContentType));
            Assert.That(mimeEntity.ContentTransferEncoding, Is.EqualTo(TestMimeMessage.textBodyContentTransferEncoding));
        }

        [Test]
        public void CanGetContent()
        {
            TextMimeEntity mimeEntity = new TextMimeEntity(_mimeParser.Object, TestMimeMessage.mimeData);
            Assert.That(mimeEntity.GetContent(), Is.EqualTo(TestMimeMessage.content));
        }
    }
}
