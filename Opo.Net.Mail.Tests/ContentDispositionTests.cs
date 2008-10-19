using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace Opo.Net.Mail
{
    [TestFixture(Description = "Tests for Opo.Net.Mime.ContentDisposition")]
    public class ContentDispositionTests
    {
        private string _dispositionType;
        private string _fileName;
        private DateTime _creationDate;
        private DateTime _modificationDate;
        private DateTime _readDate;
        private long _size;

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            _dispositionType = "attachment";
            _fileName = "file.ext";
            _creationDate = new DateTime(2001, 1, 1);
            _modificationDate = new DateTime(2002, 2, 2);
            _readDate = new DateTime(2003, 3, 3);
            _size = 123456;
        }

        [Test]
        public void CanCreateContentDisposition()
        {
            ContentDisposition contentDisposition = new ContentDisposition(_dispositionType, _fileName, _creationDate, _size);

            Assert.That(contentDisposition.DispositionType, Is.EqualTo(_dispositionType));
            Assert.That(contentDisposition.FileName, Is.EqualTo(_fileName));
            Assert.That(contentDisposition.CreationDate, Is.EqualTo(_creationDate));
            Assert.That(contentDisposition.ModificationDate, Is.EqualTo(_modificationDate));
            Assert.That(contentDisposition.ReadDate, Is.EqualTo(_readDate));
        }

        [Test]
        public void CanSetAndGetParameters()
        {
            ContentDisposition contentDisposition = new ContentDisposition();
            contentDisposition.Parameters.Add("Parameter", "Value");

            Assert.That(contentDisposition.Parameters.Count, Is.EqualTo(1));
            Assert.That(contentDisposition.Parameters["Parameter"], Is.EqualTo("Value"));
        }
    }
}
