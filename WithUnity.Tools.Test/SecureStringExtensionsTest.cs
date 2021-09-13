/* Copyright 2015-2020 Robin Murison

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. */
using NUnit.Framework;
using System.Security;

namespace WithUnity.Tools.Tests
{
    using System;
    using System.Globalization;
    using System.Security.Cryptography;
    using System.Text;
    using WithUnity.Tools;
    /// <summary>
    /// Summary description for SecureStringExtensions
    /// </summary>
    [TestFixture]
    public class SecureStringExtensionsTests
    {
        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion
        [Test]
        public void TestWipeString()
        {
            string source = "Hello This is a string";
            string destination = source.Duplicate();
            destination.WipeString();
            Assert.AreEqual(source.Length, destination.Length);
            for(int i = 0; i < destination.Length;i++)
            {
                Assert.AreEqual(destination[i], 0);
            }
        }

#if DEBUG
        private const string InputPassword = "Dobin was here 1.";

        [Test]
        public void TestGetSecureStringHash()
        {
            // set up
            SecureString ss1 = new();
            SecureString ss2 = new();
            foreach (var c in InputPassword)
            {
                ss1.AppendChar(c);
                ss2.AppendChar(c);
            }
            // No Action

            // Assert
            Assert.AreEqual(ss1.GetSecureStringSecurityHash(), ss2.GetSecureStringSecurityHash());
            ss1.Dispose();
            ss2.Dispose();
        }

        [Test]
        public void TestConvertToSecureString()
        {
            // set up
            SecureString ss2 = new();
            foreach (var c in InputPassword)
            {
                ss2.AppendChar(c);
            }

            // Action
            SecureString ss = InputPassword.MoveToSecureString();
            string ssh1 = ss.GetSecureStringSecurityHash();
            string ssh2 = ss2.GetSecureStringSecurityHash();

            // Assert
            Assert.AreEqual(ssh1, ssh2);
            ss2.Dispose();
        }

        [TestCase("George Best is the best!")]
        [TestCase("Jimmy Car is a star!")]
        public void TestConvertToUnsecureString(string inputPassword)
        {
            // set up
            string tempS = string.Empty;
            for (int i = 0; i < inputPassword.Length; i++)
            {
                tempS += inputPassword[i];
            }
            SecureString ss = inputPassword.MoveToSecureString();
            // Action
            string unsecureString = ss.ConvertToUnsecureString().Value;

            // Assert
            Assert.AreNotEqual(unsecureString, inputPassword);
            Assert.AreEqual(unsecureString, tempS);
        }

        [Test]
        public void TestSecureStringGetHashCodeFails()
        {
            // set up
            SecureString ss = InputPassword.MoveToSecureString();
            SecureString ss2 = InputPassword.MoveToSecureString();
            // No Action

            // Assert
            Assert.AreNotEqual(ss.GetHashCode(), ss2.GetHashCode());
        }

        [TestCase(null, "2")]
        [TestCase("2", null)]
        [TestCase("2", "3")]
        [TestCase("2", "33")]
        public void EqualContentsTestNotEqual(string arg1, string arg2)
        {
            SecureString ss1 = null;
            if (arg1 != null)
            {
                ss1 = arg1.MoveToSecureString();
            }
            SecureString ss2 = null;
            if(arg2 != null)
            {
                ss2 = arg2.MoveToSecureString();
            }

            Assert.IsFalse(ss1.EqualContents(ss2));
        }

        [Test]
        public void RegressionTestGetStringHash()
        {
            SecureString ss1 = new();
            foreach (var c in InputPassword)
            {
                ss1.AppendChar(c);
            }
            // No Action

            // Assert
            Assert.AreEqual(GetStringHash(InputPassword), ss1.GetSecureStringSecurityHash());
            ss1.Dispose();
        }

        [TestCase(null, null)]
        [TestCase("2", "2")]
        [TestCase("33", "33")]
        public void EqualContentsTestEqual(string arg1, string arg2)
        {
            SecureString ss1 = null;
            if(arg1 != null)
            {
                ss1 = arg1.MoveToSecureString();
            }
            SecureString ss2 = null;
            if(arg2 != null)
            {
                ss2 = arg2.MoveToSecureString();
            }

            Assert.IsTrue(ss1.EqualContents(ss2));
        }

        [TestCase(null)]
        [TestCase("2")]
        public void EqualContentsTestEqualReference(string arg1)
        {
            SecureString ss1 = null;
            if(arg1 != null)
            {
                ss1 = arg1.MoveToSecureString();
            }

            Assert.IsTrue(ss1.EqualContents(ss1));
        }

        [TestCase("2")]
        [TestCase("300")]
        [TestCase("")]
        public void EqualContentsConfirmContentsStillPresent(string arg1)
        {
            try
            {
                string Copy1 = arg1.Duplicate();
                SecureString ss1 = null;
                ss1 = arg1.MoveToSecureString();
                ss1.EqualContents(ss1);
                string out1 = ss1.ConvertToUnsecureString().Value;
                Assert.AreEqual(out1, Copy1);
                ss1.Dispose();
            }
            catch(Exception ex)
            {
                Assert.Fail($"Unexpected exception {ex.GetType().Name}: {ex.Message}");
            }

        }

        [Test]
        public static void EqualContentsConfirmNullStillPresent()
        {
            SecureString ss1 = null;
            bool test = ss1.EqualContents(ss1);
            Assert.IsTrue(test);
            Assert.IsTrue(ss1.ConvertToUnsecureString().HasNoValue);
        }
#endif

        /// <summary>
        /// Test code to create the hash directly off a string.
        /// Using a copy and pasted copy
        /// 
        /// It is here for regresssion tests in case the Extension code gets changed.
        /// and the new code provides different hashes.
        /// 
        /// Alerts us to update all the stored hashes.
        /// </summary>
        /// <param name="securePassword"></param>
        /// <returns></returns>
        private static string GetStringHash(String securePassword)
        {
            if (securePassword == null)
                throw new ArgumentNullException(nameof(securePassword));
            byte[] output;
            using (SHA512 hasher = SHA512.Create())
            {
                output = hasher.ComputeHash(Encoding.UTF8.GetBytes(securePassword));
            }
            StringBuilder sBuilder = new();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < output.Length; i++)
            {
                sBuilder.Append(output[i].ToString("x02", CultureInfo.InvariantCulture));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        [TestCase("2")]
        [TestCase("300")]
        [TestCase("")]
        public void TestUseContentsWorks(string arg1)
        {
            SecureString ss = arg1.Duplicate().MoveToSecureString();
            Assert.IsTrue(ss.UseContents((s) =>
            {
                return Result.Ok()
                  .Ensure(s.Equals(arg1, StringComparison.InvariantCulture), "Not Equal");
                }).IsSuccess);

        }

        [TestCase("2")]
        [TestCase("300")]
        [TestCase("")]
        public void TestUseContentsTWorks(string arg1)
        {
            SecureString ss = arg1.Duplicate().MoveToSecureString();
            int length = arg1.Length;
            ss.UseContents<int>(length, (l, insecure) =>
        {
                return Result.Ok<int>(l)
                    .Ensure<int>((v) =>
                    {
                        return v == insecure.Length;
                    }, "Not Equal");
            }, false);
        }


        [TestCase("2", "e")]
        [TestCase("300", "e")]
        [TestCase("", "e")]
        [TestCase("a", null)]
        public void TestUseContentsFails(string arg1, string arg2)
        {
            var errorMessage = "Not Equal";
            SecureString ss = arg1.Duplicate().MoveToSecureString();
            var res = ss.UseContents((s) =>
            {
                return Result.Ok()
                  .Ensure(s.Equals(arg2, StringComparison.InvariantCulture), errorMessage);
            });

            Assert.IsTrue(res.IsFailure);
            Assert.AreEqual(res.Error, errorMessage);

        }

        [TestCase("2")]
        [TestCase("300")]
        [TestCase("")]
        [TestCase("a")]
        public void TestUseContentsTFails(string arg1)
        {
            SecureString ss = null;
            int length = 0;
            ss = arg1.Duplicate().MoveToSecureString();
            length = arg1.Length;
            ss.UseContents<int>(length, (l, insecure) =>
            {
                return Result.Ok<int>(l)
                    .Ensure<int>((v) =>
                    {
                        return v == insecure.Length;
                    }, "Not Equal");
            }, false);
        }

        [TestCase("2", "dadads")]
        [TestCase("300", 3, "ddd")]
        [TestCase("3" )]
        public void TestUseContentsVariableParametersFails(Object[] arg1)
        {
            SecureString ss = null;
            int length = 0;
            Assert.IsTrue(0 < arg1.Length);
            Assert.IsTrue(arg1[0] is string);
            ss = (arg1[0] as string).Duplicate().MoveToSecureString();
            length = arg1.Length;
            ss.UseContents(arg1, (p, insecure) =>
            {
                return Result.Ok()
                    .Ensure(() =>
                    {
                        return p.Length < 1;
                    }, "Not Equal");
            }, false);
        }

        [TestCase("2", "dadads")]
        [TestCase("300", 3, "ddd")]
        [TestCase("3")]
        public void TestUseContentsVariableParametersSucceds(Object[] arg1)
        {
            SecureString ss = null;
            int length = 0;
            Assert.IsTrue(0 < arg1.Length);
            Assert.IsTrue(arg1[0] is string);
            ss = (arg1[0] as string).Duplicate().MoveToSecureString();
            length = arg1.Length;
            ss.UseContents(arg1, (p, insecure) =>
            {
                return Result.Ok()
                    .Ensure(() =>
                    {
                        return p.Length == arg1.Length;
                    }, "Not Equal");
            }, false);
        }
    }
}
