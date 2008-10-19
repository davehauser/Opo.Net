using System;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Moq;

namespace Opo.Net.Mime
{
    [TestFixture(Description = "Tests for Opo.Net.Mime.MimeHeaderCollection")]
    public class MimeHeaderCollectionTests
    {
        [TestFixtureSetUp]
        public void Setup()
        {

        }

        [TestFixtureTearDown]
        public void Teardown()
        {

        }

        [Test]
        public void CanAddHeaderToCollection()
        {
            IDictionary<string, string> mimeHeaderCollection = new Dictionary<string, string>();
            Assert.That(mimeHeaderCollection.Count, Is.EqualTo(0));

            mimeHeaderCollection.Add("Header-Name", "Header-Value");
            Assert.That(mimeHeaderCollection.Count, Is.EqualTo(1));
        }
    }
}
