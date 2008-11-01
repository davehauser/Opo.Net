using System;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Moq;

namespace Opo.Net.Mime
{
    [TestFixture(Description = "Tests for Opo.Net.Mime.Utilities")]
    public class MimeUtilitiesTests
    {
        private string _rfc2822Date;
        private string _rfc2822Date2;
        private DateTime _utcDate;

        [TestFixtureSetUp]
        public void Setup()
        {
            _rfc2822Date = "Mon, 1 Jan 2001 00:00:00 +0100";
            _rfc2822Date2 = "Mon, 1 Jan 2001 00:00:00 BST";
            _utcDate = new DateTime(2001, 1, 1, 0, 0, 0).AddHours(-1);
        }

        [TestFixtureTearDown]
        public void Teardown()
        {

        }

        [Test]
        public void CanParseRfc2822DateWithNumericalOffset()
        {
            Assert.That(MimeUtilities.ParseRfc2822Date(_rfc2822Date), Is.EqualTo(_utcDate));
        }

        [Test]
        public void CanParseRfc2822DateWithAlphaOffset()
        {
            Assert.That(MimeUtilities.ParseRfc2822Date(_rfc2822Date2), Is.EqualTo(_utcDate));
        }
    }
}
