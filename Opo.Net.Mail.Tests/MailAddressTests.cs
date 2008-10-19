using System;
using System.Collections.Generic;
using Opo.Net.Mail;
using NUnit.Framework;

namespace Opo.Net.Mail
{
    [TestFixture(Description = "Tests for Opo.Net.Mail.MailAddress")]
    public class MailAddressTests
    {
        private string accountName;
        private string domain;
        private string address;
        private string displayName;

        [TestFixtureSetUp]
        public void Init()
        {
            this.accountName = "email";
            this.domain = "example.org";
            this.address = String.Format("{0}@{1}", this.accountName, this.domain);
            this.displayName = "Sample Email Address";
        }

        [Test(Description = "Tests constructor with one and two arguments.")]
        public void CanCreatMailAddress()
        {
            MailAddress ma = new MailAddress(this.address);
            Assert.AreEqual(this.address, ma.Address);
            Assert.AreEqual("", ma.DisplayName);

            ma = new MailAddress(this.address, this.displayName);
            Assert.AreEqual(this.address, ma.Address);
            Assert.AreEqual(this.displayName, ma.DisplayName);
        }

        [Test(Description = "Tests the constructors with invalid arguments")]
        [ExpectedException(typeof(ArgumentException))]
        public void CannotCreatMailAddressWithInvalidArguments()
        {
            new MailAddress("\"Email 2\" <email2@example.org>");
        }

        [Test(Description = "Tests the ToString() method.")]
        public void DisplayAddressString()
        {
            MailAddress ma = new MailAddress(this.address);
            Assert.AreEqual(this.address, ma.ToString());

            ma = new MailAddress(this.address, this.displayName);
            string expected = String.Format("\"{0}\" <{1}>", this.displayName, this.address);
            Assert.AreEqual(expected, ma.ToString());
        }

        [Test(Description = "Tests the ToString(string format) method.")]
        public void DisplayAddressStringWithFormat()
        {
            MailAddress mailAddress = new MailAddress(this.address, this.displayName);
            string expexted = String.Format("Address: {0}, DisplayName: {1}, AccountName: {2}, Domain: {3}", this.address, this.displayName, this.accountName, this.domain);
            Assert.AreEqual(expexted, mailAddress.ToString("Address: {0}, DisplayName: {1}, AccountName: {2}, Domain: {3}"));
            Assert.AreEqual(expexted, mailAddress.ToString("Address: {address}, DisplayName: {displayname}, AccountName: {accountname}, Domain: {domain}"));
            Assert.AreEqual(this.domain, mailAddress.ToString("{domain}"));
        }

        [Test(Description = "Tests the mail address parser with different valid inputs")]
        public void CanParseMailAddress()
        {
            string testMailAddress = "email@example.org";
            IMailAddress ma = MailAddress.Parse(testMailAddress);
            Assert.AreEqual("email@example.org", ma.Address);
            Assert.AreEqual("", ma.DisplayName);

            testMailAddress = "my-email@blog.example.org  ";
            ma = MailAddress.Parse(testMailAddress);
            Assert.AreEqual("my-email@blog.example.org", ma.Address);
            Assert.AreEqual("", ma.DisplayName);

            testMailAddress = " email@example.org  ";
            ma = MailAddress.Parse(testMailAddress);
            Assert.AreEqual("email@example.org", ma.Address);
            Assert.AreEqual("", ma.DisplayName);

            testMailAddress = "\"Sample Email Address\" <email@example.org>";
            ma = MailAddress.Parse(testMailAddress);
            Assert.AreEqual("email@example.org", ma.Address);
            Assert.AreEqual("Sample Email Address", ma.DisplayName);

            testMailAddress = "  Sample Email Address   email@example.org";
            ma = MailAddress.Parse(testMailAddress);
            Assert.AreEqual("email@example.org", ma.Address);
            Assert.AreEqual("Sample Email Address", ma.DisplayName);
        }

        [Test(Description = "Tests the mail address parser with different invalid inputs")]
        [ExpectedException(typeof(ArgumentException))]
        public void CannotParseInvalidMailAddress()
        {
            MailAddress.Parse("email@blog#example.org");
        }

        [Test(Description = "Tests the GetAccountName method")]
        public void CanGetAccountName()
        {
            MailAddress ma = new MailAddress(this.address);
            Assert.AreEqual(this.accountName, MailAddress.GetAccountName(ma));
        }

        [Test(Description = "Tests the AccountName property of MailAddress")]
        public void CanGetAndSetAccountName()
        {
            MailAddress ma = new MailAddress(this.address);
            Assert.AreEqual(this.accountName, ma.AccountName);

            ma.AccountName = "sample";
            Assert.AreEqual("sample@example.org", ma.Address);
        }

        [Test(Description = "Tests the GetDomain method")]
        public void CanGetDomain()
        {
            MailAddress ma = new MailAddress(this.address);
            Assert.AreEqual(this.domain, MailAddress.GetHost(ma));
        }

        [Test(Description = "Tests the Domain property of MailAddress")]
        public void CanGetAndSetDomain()
        {
            MailAddress ma = new MailAddress(this.address);
            Assert.AreEqual(this.domain, ma.Host);

            ma.Host = "email.org";
            Assert.AreEqual("email@email.org", ma.Address);
        }
    }
}
