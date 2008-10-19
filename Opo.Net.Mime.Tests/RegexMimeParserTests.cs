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
        RegexMimeParser regexMimeParser;

        [TestFixtureSetUp]
        public void Setup()
        {
            regexMimeParser = new RegexMimeParser();
        }

        [Test]
        public void CanParseMessageID()
        {
            Assert.That(regexMimeParser.ParseMessageID(""), Is.EqualTo(""));
            Assert.That(regexMimeParser.ParseMessageID(TestMimeMessage.mimeData), Is.EqualTo(TestMimeMessage.messageID));
        }

        [Test]
        public void CanParseSubject()
        {
            Assert.That(regexMimeParser.ParseSubject(""), Is.EqualTo(""));
            Assert.That(regexMimeParser.ParseSubject(TestMimeMessage.mimeData), Is.EqualTo(TestMimeMessage.subject));
        }

        [Test]
        public void CanParseFromAddress()
        {
            Assert.That(regexMimeParser.ParseFrom(""), Is.EqualTo(""));
            string mailAddress = regexMimeParser.ParseFrom(TestMimeMessage.mimeData);
            Assert.That(mailAddress, Is.EqualTo(TestMimeMessage.emailAdresses[0]));
        }

        [Test]
        public void CanParseToAddresses()
        {
            Assert.That(regexMimeParser.ParseTo(""), Is.EqualTo(new string[] { }));

            string[] actual = regexMimeParser.ParseTo(TestMimeMessage.mimeData);

            Assert.That(actual.Length, Is.EqualTo(4));
            Assert.That(actual[0], Is.EqualTo(TestMimeMessage.emailAdresses[0]));
            Assert.That(actual[1], Is.EqualTo(TestMimeMessage.emailAdresses[1]));
            Assert.That(actual[2], Is.EqualTo(TestMimeMessage.emailAdresses[2]));
            Assert.That(actual[3], Is.EqualTo(TestMimeMessage.emailAdresses[3]));
        }

        [Test]
        public void CanParseCCAddresses()
        {
            Assert.That(regexMimeParser.ParseTo(""), Is.EqualTo(new string[] { }));

            string[] actual = regexMimeParser.ParseCC(TestMimeMessage.mimeData);

            Assert.That(actual.Length, Is.EqualTo(4));
            Assert.That(actual[0], Is.EqualTo(TestMimeMessage.emailAdresses[0]));
            Assert.That(actual[1], Is.EqualTo(TestMimeMessage.emailAdresses[1]));
            Assert.That(actual[2], Is.EqualTo(TestMimeMessage.emailAdresses[2]));
            Assert.That(actual[3], Is.EqualTo(TestMimeMessage.emailAdresses[3]));
        }

        [Test]
        public void CanParseBCCAddresses()
        {
            Assert.That(regexMimeParser.ParseTo(""), Is.EqualTo(new string[] { }));

            string[] actual = regexMimeParser.ParseBCC(TestMimeMessage.mimeData);

            Assert.That(actual.Length, Is.EqualTo(4));
            Assert.That(actual[0], Is.EqualTo(TestMimeMessage.emailAdresses[0]));
            Assert.That(actual[1], Is.EqualTo(TestMimeMessage.emailAdresses[1]));
            Assert.That(actual[2], Is.EqualTo(TestMimeMessage.emailAdresses[2]));
            Assert.That(actual[3], Is.EqualTo(TestMimeMessage.emailAdresses[3]));
        }

        [Test]
        public void CanParseMessageDate()
        {
            string messageDate = regexMimeParser.ParseDate(TestMimeMessage.mimeData);
            Assert.That(messageDate, Is.EqualTo(TestMimeMessage.messageDate));
        }

        [Test]
        public void CanParseMimeVersion()
        {
            Assert.That(regexMimeParser.ParseMimeVersion(TestMimeMessage.mimeData), Is.EqualTo(TestMimeMessage.mimeVersion));
        }

        [Test]
        public void CanParsePriority()
        {
            Assert.That(regexMimeParser.ParsePriority(TestMimeMessage.mimeData), Is.EqualTo(TestMimeMessage.priority));
        }

        [Test]
        public void CanParseContentType()
        {
            Assert.That(regexMimeParser.ParseContentType(TestMimeMessage.mimeData), Is.EqualTo(TestMimeMessage.contentType));
        }

        [Test]
        public void CanParseCharset()
        {
            Assert.That(regexMimeParser.ParseCharset(TestMimeMessage.mimeData), Is.EqualTo(TestMimeMessage.textBodyCharset));
        }

        [Test]
        public void CanParseBoundary()
        {
            Assert.That(regexMimeParser.ParseBoundary(TestMimeMessage.mimeData), Is.EqualTo(TestMimeMessage.boundaryLevel1));
        }

        [Test]
        public void CanParseContent()
        {
            System.Diagnostics.Debug.WriteLine(regexMimeParser.ParseContent(TestMimeMessage.mimeData));
            Assert.That(regexMimeParser.ParseContent(TestMimeMessage.mimeData), Is.EqualTo(TestMimeMessage.content.Trim()));
        }
    }
}
