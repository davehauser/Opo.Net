using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace Opo.Net.Mime
{
    [TestFixture(Description = "Tests for MimeEncoding")]
    public class MimeEncodingTests
    {
        private string _plainText;
        private string _quotedPrintableEncodedText;
        private string _base64EncodedText;

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            _plainText = @"ABCD
EFGHIJKL MNOPQ RSTUVW XYZ. abcdefgh ijkl mnopq rst uv wxyz. äöü éèê =-_ ?!()/&%ç*+""";

            _quotedPrintableEncodedText = @"ABCD
EFGHIJKL MNOPQ RSTUVW XYZ. abcdefgh ijkl mnopq rst uv wxyz. =E4=F6=FC=
 =E9=E8=EA =3D-_ ?!()/&%=E7*+""";

            _base64EncodedText = @"QUJDRA0KRUZHSElKS0wgTU5PUFEgUlNUVVZXIFhZWi4gYWJjZGVmZ2ggaWprbCBtbm9wcSByc3Qg
dXYgd3h5ei4gPz8/ID8/PyA9LV8gPyEoKS8mJT8qKyI=";
        }

        [Test]
        public void CanEncodeQuotedPrintable()
        {
            Assert.That(MimeEncoding.QuotedPrintable.Encode(_plainText), Is.EqualTo(_quotedPrintableEncodedText));
        }

        [Test]
        public void CanDecodeQuotedPrintable()
        {
            Assert.That(MimeEncoding.QuotedPrintable.Decode(_quotedPrintableEncodedText), Is.EqualTo(_plainText));
        }

        [Test]
        public void CanEncodeBase64()
        {
            Assert.That(MimeEncoding.Base64.Encode(_plainText), Is.EqualTo(_base64EncodedText));
        }

        [Test]
        public void CanDecodeBase64()
        {
            throw new NotImplementedException();
        }
    }
}
