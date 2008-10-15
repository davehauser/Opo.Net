using System;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Moq;

namespace Opo.Net.Mime
{
    [TestFixture]
    public class MimeEntityTests
    {
        private Mock<IMimeParser> _mimeParser = new Mock<IMimeParser>();
        
        [TestFixtureSetUp]
        public void Setup()
        {
            _mimeParser.Expect(m => m.ParseBoundary(It.IsAny<string>())).Returns(TestMimeMessage.boundaryLevel1);
            _mimeParser.Expect(m => m.ParseContentType(It.IsAny<string>())).Returns(TestMimeMessage.contentType);
            _mimeParser.Expect(m => m.ParseContent(It.IsAny<string>())).Returns(TestMimeMessage.content);
        }

        [TestFixtureTearDown]
        public void Teardown()
        {

        }

        [Test]
        public void CanCreateMimeEntity()
        {
            MimeEntityBase mimeEntityBase = new MimeEntityBase(_mimeParser.Object, TestMimeMessage.mimeData);
            Assert.That(mimeEntityBase.MimeData, Is.EqualTo(TestMimeMessage.mimeData));
            Assert.That(mimeEntityBase.ContentType, Is.EqualTo(TestMimeMessage.contentType));

            mimeEntityBase = new MimeEntityBase(_mimeParser.Object, TestMimeMessage.mimeData, TestMimeMessage.contentType);
            Assert.That(mimeEntityBase.ContentType, Is.EqualTo(TestMimeMessage.contentType));
            Assert.That(mimeEntityBase.MimeData, Is.EqualTo(TestMimeMessage.mimeData));
        }
    }
}
