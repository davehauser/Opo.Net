using System;
using System.Collections.Generic;
using Opo.Net.Mail;
using NUnit.Framework;

namespace Opo.Net.Mail.Tests
{
    /// <summary>
    /// Tests for Opo.Net.Mail.EmailAddress
    /// </summary>
    [TestFixture(Description = "Tests for Opo.Net.Mail.EmailAddress")]
    public class EmailAddressTests
    {
        /// <summary>
        /// Sample email account name
        /// </summary>
        private string accountName;

        /// <summary>
        /// Sample email domain
        /// </summary>
        private string domain;

        /// <summary>
        /// Sample email address
        /// </summary>
        private string address;

        /// <summary>
        /// Sample email display name
        /// </summary>
        private string displayName;

        /// <summary>
        /// Setup sample data
        /// </summary>
        [TestFixtureSetUp]
        public void Init()
        {
            this.accountName = "email";
            this.domain = "example.org";
            this.address = String.Format("{0}@{1}", this.accountName, this.domain);
            this.displayName = "Sample Email Address";
        }

        /// <summary>
        /// Tests for the constructor with both possibilities:
        /// <list type="number">
        /// <item>EmailAddress(address)</item>
        /// <item>EmailAddress(address, displayName)</item>
        /// </list>
        /// </summary>
        [Test(Description = "Tests constructor with one and two arguments.")]
        public void CanCreateEmailAddress()
        {
            EmailAddress ma = new EmailAddress(this.address);
            Assert.AreEqual(this.address, ma.Address);
            Assert.AreEqual("", ma.DisplayName);

            ma = new EmailAddress(this.address, this.displayName);
            Assert.AreEqual(this.address, ma.Address);
            Assert.AreEqual(this.displayName, ma.DisplayName);
        }

        /// <summary>
        /// Tests the constructors with invalid arguments
        /// </summary>
        [Test(Description = "Tests the constructors with invalid arguments")]
        [ExpectedException(typeof(ArgumentException))]
        public void CannotCreateEmailAddressWithInvalidArguments()
        {
            new EmailAddress("\"Email 2\" <email2@example.org>");
        }

        /// <summary>
        /// Tests the ToString() method, there should be two different outputs, depending on whether DisplayName property is set or not:
        /// <list type="number">
        /// <item>(DisplayName is empty): email&#64;example.org</item>
        /// <item>(DisplayName is set): "Sample Email Address" &lt;email&#64;example.org&gt;</item>
        /// </list>
        /// This takes a <see cref="string"></see>
        /// </summary>
        [Test(Description = "Tests the ToString() method.")]
        public void DisplayAddressString()
        {
            EmailAddress ma = new EmailAddress(this.address);
            Assert.AreEqual(this.address, ma.ToString());

            ma = new EmailAddress(this.address, this.displayName);
            string expected = String.Format("\"{0}\" <{1}>", this.displayName, this.address);
            Assert.AreEqual(expected, ma.ToString());
        }

        /// <summary>
        /// Tests the mail address parser with different valid inputs
        /// </summary>
        [Test(Description = "Tests the mail address parser with different valid inputs")]
        public void CanParseEmailAddress()
        {
            string testEmailAddress = "email@example.org";
            EmailAddress ma = EmailAddress.Parse(testEmailAddress);
            Assert.AreEqual("email@example.org", ma.Address);
            Assert.AreEqual("", ma.DisplayName);

            testEmailAddress = "my-email@blog.example.org  ";
            ma = EmailAddress.Parse(testEmailAddress);
            Assert.AreEqual("my-email@blog.example.org", ma.Address);
            Assert.AreEqual("", ma.DisplayName);

            testEmailAddress = " email@example.org  ";
            ma = EmailAddress.Parse(testEmailAddress);
            Assert.AreEqual("email@example.org", ma.Address);
            Assert.AreEqual("", ma.DisplayName);

            testEmailAddress = "\"Sample Email Address\" <email@example.org>";
            ma = EmailAddress.Parse(testEmailAddress);
            Assert.AreEqual("email@example.org", ma.Address);
            Assert.AreEqual("Sample Email Address", ma.DisplayName);

            testEmailAddress = "  Sample Email Address   email@example.org";
            ma = EmailAddress.Parse(testEmailAddress);
            Assert.AreEqual("email@example.org", ma.Address);
            Assert.AreEqual("Sample Email Address", ma.DisplayName);
        }

        /// <summary>
        /// Tests the mail address parser with invalid address input
        /// </summary>
        [Test(Description = "Tests the mail address parser with different invalid inputs")]
        [ExpectedException(typeof(ArgumentException))]
        public void CannotParseInvalidEmailAddress()
        {
            EmailAddress.Parse("email@blog#example.org");
        }

        /// <summary>
        /// Tests the GetAccountName method
        /// </summary>
        [Test(Description = "Tests the GetAccountName method")]
        public void CanGetAccountName()
        {
            EmailAddress ma = new EmailAddress(this.address);
            Assert.AreEqual(this.accountName, EmailAddress.GetAccountName(ma));
        }

        /// <summary>
        /// Tests the AccountName property of EmailAddress
        /// </summary>
        [Test(Description = "Tests the AccountName property of EmailAddress")]
        public void CanGetAndSetAccountName()
        {
            EmailAddress ma = new EmailAddress(this.address);
            Assert.AreEqual(this.accountName, ma.AccountName);

            ma.AccountName = "sample";
            Assert.AreEqual("sample@example.org", ma.Address);
        }

        /// <summary>
        /// Tests the GetDomain method
        /// </summary>
        [Test(Description = "Tests the GetDomain method")]
        public void CanGetDomain()
        {
            EmailAddress ma = new EmailAddress(this.address);
            Assert.AreEqual(this.domain, EmailAddress.GetDomain(ma));
        }

        /// <summary>
        /// Tests the Domain property of EmailAddress
        /// </summary>
        [Test(Description = "Tests the Domain property of EmailAddress")]
        public void CanGetAndSetDomain()
        {
            EmailAddress ma = new EmailAddress(this.address);
            Assert.AreEqual(this.domain, ma.Domain);

            ma.Domain = "email.org";
            Assert.AreEqual("email@email.org", ma.Address);
        }
    }
}
