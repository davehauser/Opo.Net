using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace Opo.Net.Mime
{
    [TestFixture(Description = "Tests for RegexMimeParser")]
    public class RegexMimeParserTests
    {
        RegexMimeParser _regexMimeParser;

        [TestFixtureSetUp]
        public void Setup()
        {
            _regexMimeParser = new RegexMimeParser();
        }

        [Test]
        public void CanParseHeaders()
        {
            Assert.That(String.Join("\r\n", _regexMimeParser.ParseHeaders(TestMimeMessage.mimeData)).Trim(), Is.EqualTo(TestMimeMessage.headers.Trim()));
        }

        [Test]
        public void CanParseHeaderValueUsingSubject()
        {
            Assert.That(_regexMimeParser.ParseHeaderValue(TestMimeMessage.mimeData, "Subject"), Is.EqualTo(TestMimeMessage.subject));
        }

        [Test]
        public void CanParseContentType()
        {
            Assert.That(_regexMimeParser.ParseContentType(TestMimeMessage.mimeData), Is.EqualTo(TestMimeMessage.contentType));
        }

        [Test]
        public void CanParseCharset()
        {
            Assert.That(_regexMimeParser.ParseCharset(TestMimeMessage.mimeData), Is.EqualTo(TestMimeMessage.textBodyCharset));
        }

        [Test]
        public void CanParseBoundary()
        {
            Assert.That(_regexMimeParser.ParseBoundary(TestMimeMessage.mimeData), Is.EqualTo(TestMimeMessage.boundaryLevel1));
        }

        [Test]
        public void CanParseContent()
        {
            Assert.That(_regexMimeParser.ParseContent(TestMimeMessage.mimeData), Is.EqualTo(TestMimeMessage.content.Trim()));
        }
    }
}
