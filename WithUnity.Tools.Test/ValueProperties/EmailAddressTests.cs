/* Copyright 2015-2020 Robin Murison

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. */
using NUnit.Framework;
using System;
using WithUnity.Tools.ValueProperties;

namespace WithUnity.Tools.Tests
{
    /// <summary>
    /// Summary description for FileExtensionsTests
    /// </summary>
    [TestFixture]
    public class EmailAddressTests
    {
        [TestCase("asdadsaa.sss")]
        [TestCase("asda")]
        [TestCase("lidle")]
        [TestCase("tesco")]
        public void NoAtCausesInvalidCastException(string address)
        {
            // Arrange
            // Act
            try
            {
                EmailAddress emailAddress = address;
                // Asserts
                Assert.Fail("email address without @ accepted.");
            }
            catch (InvalidCastException)
            {
                // Test passed
            }
            catch (Exception ex)
            {
                // Asserts
                Assert.Fail($"Wrong type of exception thrown {ex.GetType().Name}. Message is {ex.Message}");
            }
        }

        [TestCase("asda@asda@asda")]
        [TestCase("a@s@da")]
        [TestCase("l@i@@le")]
        [TestCase("t@es@co")]
        public void MultipleAtCausesInvalidCastException(string address)
        {
            // Arrange
            // Act
            try
            {
                EmailAddress emailAddress = address;
                // Asserts
                Assert.Fail("email address without @ accepted.");
            }
            catch (InvalidCastException)
            {
                // Test passed
            }
            catch (Exception ex)
            {
                // Asserts
                Assert.Fail($"Wrong type of exception thrown {ex.GetType().Name}. Message is {ex.Message}");
            }
        }



        [Test]
        public void NullCausesInvalidCastException()
        {
            // Arrange
            // Act
            try
            {
#pragma warning disable CS0219 // Variable is assigned but its value is never used
                // We are testing for an InvalidCastException being thrown on asssignment.
                EmailAddress emailAddress = (string)null;
#pragma warning restore CS0219 // Variable is assigned but its value is never used
                              // Asserts
                Assert.Fail("null email accepted.");
            }
            catch (InvalidCastException ex)
            {
                // Asserts
                Assert.IsTrue(ex.Message.Contains("Null email Address"));
            }
            catch (Exception ex)
            {
                // Asserts
                Assert.Fail($"Wrong type of exception thrown {ex.GetType().Name}. Message is {ex.Message}");
            }
        }

        [TestCase(" a@b")]
        [TestCase("\t \r\n\a@b\t \r\n\v")]
        [TestCase("\ta@b")]
        [TestCase("\ra@b")]
        [TestCase("\va@b")]
        [TestCase("\fa@b")]
        public void AddressNeedsTrimmingCausesInvalidCastException(string address)
        {
            // Arrange
            // Act
            try
            {
                EmailAddress emailAddress = address;
                // Asserts
                Assert.Fail("short email accepted.");
            }
            catch (InvalidCastException)
            {
                // Various errprs can be returned so unsure which one
            }
            catch (Exception ex)
            {
                // Asserts
                Assert.Fail($"Wrong type of exception thrown {ex.GetType().Name}. Message is {ex.Message}");
            }
        }


        [TestCase("sd")]
        [TestCase("aa")]
        [TestCase("aa")]
        [TestCase("aa")]
        [TestCase("aa")]
        [TestCase("ab")]
        [TestCase("c")]
        [TestCase("@1")]
        [TestCase("2@")]
        public void LessThanThreeCharacterCausesInvalidCastException(string address)
        {
            // Arrange
            // Act
            try
            {
                EmailAddress emailAddress = address;
                // Asserts
                Assert.Fail("short email accepted.");
            }
            catch (InvalidCastException)
            {
                // Various errprs can be returned so unsure which one
            }
            catch (Exception ex)
            {
                // Asserts
                Assert.Fail($"Wrong type of exception thrown {ex.GetType().Name}. Message is {ex.Message}");
            }
        }

        [TestCase("@a.com")]
        [TestCase("@charlie.com")]
        [TestCase("@abc")]
        public void NothingBeforeAtCausesInvalidCastException(string address)
        {
            try
            {
                // Arrange
                EmailAddress emailAddress = address;
                // Asserts
                Assert.Fail("email accepts @...");
            }
            catch (InvalidCastException ex)
            {
                // Asserts
                Assert.IsTrue(ex.Message.Contains("No preceding name before @ sign in EmailAddress"));
            }
            catch (Exception ex)
            {
                // Asserts
                Assert.Fail($"Wrong type of exception thrown {ex.GetType().Name}. Message is {ex.Message}");
            }
        }

        [TestCase("ape@")]
        [TestCase("charlie@")]
        [TestCase("abc@")]
        public void NothingAfterAtCausesInvalidCastException(string address)
        {
            try
            {
                // Arrange
                EmailAddress emailAddress = address;
                // Asserts
                Assert.Fail("email accepts ...@");
            }
            catch (InvalidCastException ex)
            {
                // Asserts
                Assert.IsTrue(ex.Message.Contains("No domain name after @ sign in EmailAddress"));
            }
            catch (Exception ex)
            {
                // Asserts
                Assert.Fail($"Wrong type of exception thrown {ex.GetType().Name}. Message is {ex.Message}");
            }
        }

        [TestCase("robin@age.com")]
        [TestCase("join@gmail.com")]
        [TestCase("j@g")]
        [TestCase("k@v")]
        public void AddressAssignmentWorks(string address)
        {
            // Arrange
            Result<EmailAddress> emailAddress = Result.Ok<EmailAddress>(address);

            // Asserts
            Assert.IsTrue(emailAddress.IsSuccess);
            Assert.AreEqual(emailAddress.Value, address);
        }
        //////////////////////////
        /// Repeat tests on Create
        //////////////////////////
        [TestCase("asdadsaa.sss")]
        [TestCase("asda")]
        [TestCase("lidle")]
        [TestCase("tesco")]
        public void NoAtCausesFailureOnCreate(string address)
        {
            // Arrange
            // Act
            Result<EmailAddress> emailAddressResult = EmailAddress.CreateEmailAddress(address);
            // Asserts
            Assert.IsFalse(emailAddressResult.IsSuccess);
        }

        [TestCase("asda@asda@asda")]
        [TestCase("a@s@da")]
        [TestCase("l@i@@le")]
        [TestCase("t@es@co")]
        public void MultipleAtCausesFailureOnCreate(string address)
        {
            // Arrange
            // Act
            Result<EmailAddress> emailAddressResult = EmailAddress.CreateEmailAddress(address);
            // Asserts
            Assert.IsFalse(emailAddressResult.IsSuccess);
        }

        [Test]
        public void NullCausesFailureOnCreate()
        {
            // Arrange
            // Act
            // We are testing for an InvalidCastException being thrown on asssignment.
            Result<EmailAddress> emailAddressResult = EmailAddress.CreateEmailAddress((string) null);
            // Asserts
            Assert.IsFalse(emailAddressResult.IsSuccess);
        }


        [TestCase(" a@b")]
        [TestCase("\t \r\n\a@b\t \r\n\v")]
        [TestCase("\ta@b")]
        [TestCase("\ra@b")]
        [TestCase("\va@b")]
        [TestCase("\fa@b")]
        public void AddressNeedsTrimmingCausesFailureOnCreate(string address)
        {
            // Arrange
            // Act
            // We are testing for an InvalidCastException being thrown on asssignment.
            Result<EmailAddress> emailAddressResult = EmailAddress.CreateEmailAddress((string)null);
            // Asserts
            Assert.IsFalse(emailAddressResult.IsSuccess);
        }
        [TestCase("sd")]
        [TestCase("aa")]
        [TestCase("aa")]
        [TestCase("aa")]
        [TestCase("aa")]
        [TestCase("ab")]
        [TestCase("c")]
        [TestCase("@1")]
        [TestCase("2@")]
        public void LessThanThreeCharacterCausesFailureOnCreate(string address)
        {
            // Arrange
            // Act
            Result<EmailAddress> emailAddressResult = EmailAddress.CreateEmailAddress(address);
            // Asserts
            Assert.IsFalse(emailAddressResult.IsSuccess);
        }

        [TestCase("@a.com")]
        [TestCase("@charlie.com")]
        [TestCase("@abc")]
        public void NothingBeforeAtCausesFailureOnCreate(string address)
        {
            // Arrange
            // Act
            Result<EmailAddress> emailAddressResult = EmailAddress.CreateEmailAddress(address);
            // Asserts
            Assert.IsFalse(emailAddressResult.IsSuccess);
        }

        [TestCase("ape@")]
        [TestCase("charlie@")]
        [TestCase("abc@")]
        public void NothingAfterAtCausesFailureOnCreate(string address)
        {
            // Arrange
            // Act
            Result<EmailAddress> emailAddressResult = EmailAddress.CreateEmailAddress(address);
            // Asserts
            Assert.IsFalse(emailAddressResult.IsSuccess);
        }

        [TestCase("robin@age.com")]
        [TestCase("join@gmail.com")]
        [TestCase("j@g")]
        [TestCase("k@v")]
        public void AddressAssignmentWorksOnCreate(string address)
        {
            // Arrange
            // Act
            Result<EmailAddress> emailAddressResult = EmailAddress.CreateEmailAddress(address);
            // Asserts
            Assert.IsTrue(emailAddressResult.IsSuccess);
            Assert.AreEqual(emailAddressResult.Value.Value, address);
        }

    }
}
