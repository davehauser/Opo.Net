using System;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Moq;
using System.Text.RegularExpressions;
using Debug = System.Diagnostics.Debug;

namespace Opo.Net.Mime
{
    [TestFixture(Description = "Tests for Opo.Net.Mime.MultipartMimeEntity")]
    public class MultipartMimeEntityTests
    {
        IMimeParser _mimeParser = new RegexMimeParser();

        [TestFixtureSetUp]
        public void Setup()
        {

        }

        [TestFixtureTearDown]
        public void Teardown()
        {

        }

        [Test]
        public void CanCreateMultipartMimeEntityFromMimeData()
        {
            MultipartMimeEntity mimeEntity = new MultipartMimeEntity(_mimeParser, TestMimeMessage.mimeData);

            Assert.That(mimeEntity.Entities.Count, Is.EqualTo(2));
            Assert.That(mimeEntity.Entities[0], Is.TypeOf(typeof(MultipartMimeEntity)));
            Assert.That(mimeEntity.Entities[1], Is.TypeOf(typeof(AttachmentMimeEntity)));
            Assert.That(mimeEntity.Entities[0].Entities.Count, Is.EqualTo(2));
            Assert.That(mimeEntity.Entities[0].Entities[0].ContentType, Is.EqualTo("text/plain"));
            Assert.That(mimeEntity.Entities[0].Entities[1].ContentType, Is.EqualTo("text/html"));
            Regex r = new Regex(@"(\r\n\s*){3,}");
            string cleanedUpMimeData = r.Replace(TestMimeMessage.mimeData, "\r\n\r\n");
            Assert.That(mimeEntity.GetMimeData(), Is.EqualTo(cleanedUpMimeData));
            Assert.That(mimeEntity.GetHeaderValue("From"), Is.EqualTo(TestMimeMessage.from));
            Assert.That(mimeEntity.Boundary, Is.EqualTo(TestMimeMessage.boundaryLevel1));
        }

        [Test]
        public void CanSetMimeData()
        {
            MultipartMimeEntity mimeEntity = new MultipartMimeEntity(_mimeParser, TestMimeMessage.mimeData);
            Assert.That(mimeEntity.Entities.Count, Is.EqualTo(2));

            mimeEntity.SetMimeData(TestMimeMessage.mimeData);
            Assert.That(mimeEntity.Entities.Count, Is.EqualTo(2));
        }
    }
}
