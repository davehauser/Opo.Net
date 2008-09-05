using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace Opo.Net.Mail
{
    [TestFixture(Description="Tests for AlternativeView")]
    public class AlternativeViewTests
    {
        string _content = "Content";
        string _contentType = "ContentType";
        string _charset = "Charset";
        string _transferEncoding = "TransferEncoding";

        [Test]
        public void CanCreateAlternativeView()
        {
            AlternativeView alternativeView = new AlternativeView(_content, _contentType);

            Assert.That(alternativeView.Content, Is.EqualTo(_content));
            Assert.That(alternativeView.ContentType, Is.EqualTo(_contentType));

            alternativeView = new AlternativeView(_content, _contentType, _charset, _transferEncoding);

            Assert.That(alternativeView.Content, Is.EqualTo(_content));
            Assert.That(alternativeView.ContentType, Is.EqualTo(_contentType));
            Assert.That(alternativeView.Charset, Is.EqualTo(_charset));
            Assert.That(alternativeView.TransferEncoding, Is.EqualTo(_transferEncoding));
        }

        [Test]
        public void CanLoadFromXml()
        {
            // provide only content and content type
            XElement xml = XElement.Parse("<View ContentType=\"" + _contentType + "\">" + _content + "</View>");
            AlternativeView alternativeView = AlternativeView.LoadXmlAlternativeView(xml);

            Assert.That(alternativeView.Content, Is.EqualTo(_content));
            Assert.That(alternativeView.ContentType, Is.EqualTo(_contentType));

            // provide all possible arguments
            xml = XElement.Parse("<View ContentType=\"" + _contentType + "\" Charset=\"" + _charset + "\" TransferEncoding=\"" + _transferEncoding + "\">" + _content + "</View>");
            alternativeView = AlternativeView.LoadXmlAlternativeView(xml);

            Assert.That(alternativeView.Content, Is.EqualTo(_content));
            Assert.That(alternativeView.ContentType, Is.EqualTo(_contentType));
            Assert.That(alternativeView.Charset, Is.EqualTo(_charset));
            Assert.That(alternativeView.TransferEncoding, Is.EqualTo(_transferEncoding));
        }
    }
}
