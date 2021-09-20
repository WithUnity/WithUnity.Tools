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
        public void NoAtFailure(string address)
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
        public void LessThanThreeCharacterFailure()
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

        [TestCase(" ")]
        [TestCase("\t \r\n\vsd\t \r\n\v")]
        [TestCase("\taa")]
        [TestCase("\raa")]
        [TestCase("\vaa")]
        [TestCase("\faa")]
        [TestCase("ab")]
        [TestCase("c")]
        [TestCase("@1")]
        [TestCase("2@")]
        public void LessThanThreeCharacterFailure(string address)
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
        public void NothingBeforeAt(string address)
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
        public void NothingAfterAt(string address)
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
    }
}
