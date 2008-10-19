using System;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Moq;

namespace Opo.Net.Mime
{
    [TestFixture(Description = "Tests for Opo.Net.Mime.MimeEntityCollection")]
    public class MimeEntityCollectionTests
    {
        private Mock<IMimeParser> _mimeParser = new Mock<IMimeParser>();

        [TestFixtureSetUp]
        public void Setup()
        {
            _mimeParser.Expect(m => m.ParseContentType(It.IsAny<string>())).Returns("text/plain");
        }

        [TestFixtureTearDown]
        public void Teardown()
        {

        }

        [Test]
        public void CanAddMimeEntityToCollection()
        {
            MimeEntityCollection mimeEntityCollection = new MimeEntityCollection();
            Assert.That(mimeEntityCollection.Count, Is.EqualTo(0));

            IMimeEntity mimeEntity = new TextMimeEntity(_mimeParser.Object, TestMimeMessage.textBody);
            mimeEntityCollection.Add(mimeEntity);
            Assert.That(mimeEntityCollection.Count, Is.EqualTo(1));

            mimeEntityCollection.Add(_mimeParser.Object, TestMimeMessage.mimeData);
            Assert.That(mimeEntityCollection.Count, Is.EqualTo(2));
        }
    }
}
