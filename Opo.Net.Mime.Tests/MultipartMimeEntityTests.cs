using System;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Moq;

namespace Opo.Net.Mime
{
    [TestFixture(Description = "Tests for Opo.Net.Mime.MultipartMimeEntity")]
    public class MultipartMimeEntityTests
    {
        Mock<IMimeParser> _mimeParser = new Mock<IMimeParser>();

        [TestFixtureSetUp]
        public void Setup()
        {
            _mimeParser.Expect(m => m.ParseBoundary(TestMimeMessage.mimeData)).Returns(TestMimeMessage.boundaryLevel1);
            _mimeParser.Expect(m => m.ParseContentType(It.IsAny<string>())).Returns("text/plain");
        }

        [TestFixtureTearDown]
        public void Teardown()
        {

        }

        [Test]
        public void CanCreateMultipartMimeEntity()
        {
            System.Diagnostics.Debug.WriteLine(TestMimeMessage.mimeData);
            IMimeEntity mimeEntity = new MultipartMimeEntity(_mimeParser.Object, "no entities");
            Assert.That(mimeEntity.HasEntities, Is.False);

            mimeEntity = new MultipartMimeEntity(_mimeParser.Object, TestMimeMessage.mimeData);
            Assert.That(mimeEntity.Entities.Count, Is.EqualTo(2));
            Assert.That(mimeEntity.GetMimeData(), Is.EqualTo(TestMimeMessage.mimeData));
        }
    }
}
