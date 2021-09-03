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
    public class FileExtensionsTests
    {
        [TestCase("")]
        [TestCase(null)]
        [TestCase(" ")]
        [TestCase("\t \r\n\v")]
        [TestCase("\t")]
        [TestCase("\r")]
        [TestCase("\n\v")]
        [TestCase("\v")]
        [TestCase("\f")]
        public void NullBlankPathfailure(string nullPath)
        {
            // Arrange
            // Act
            try
            {
                FileExtension fileExtension = new FileExtension(nullPath);
                // Asserts
                Assert.Fail("null or empty string was accepted in error.");
            }
            catch (InvalidCastException ex)
            {
                // Asserts
                Assert.IsTrue(ex.Message.Contains("File extensions cannot be blank or null"));
            }
        }

        [TestCase("/")]
        [TestCase("\\")]
        [TestCase(":")]
        public void TestInvalidExtensionCharacters(string invalidPath)
        {
            // Arrange
            FileExtension fileExtension = null;
            // Act
            try
            {
                fileExtension = new FileExtension(invalidPath);
                // Asserts
                Assert.Fail("Invalid path was accepted in error.");
            }
            catch (InvalidCastException ex)
            {
                // Asserts
                Assert.IsTrue(ex.Message.Contains("Input string is not a valid file extension. Error: "));
                switch (invalidPath[0])
                {
                    case '/':
                        Assert.IsTrue(ex.Message.Contains(@"File extensions cannot contain '/'"));
                        break;
                    case '\\':
                        Assert.IsTrue(ex.Message.Contains(@"File extensions cannot contain '\'"));
                        break;
                    case ':':
                        Assert.IsTrue(ex.Message.Contains(@"File extensions cannot contain colons."));
                        break;
                    default:
                        Assert.Fail("Someone added a testCase without adding the check for specific errors.");
                        break;
                }
            }
        }

        [TestCase("Jpeg")]
        [TestCase("jPeg")]
        public void TestOuputIsLowerCase(string validExtension)
        {
            // Arrange
            string expected = validExtension.ToUpperInvariant();

            //Action
            FileExtension fileExtension = new FileExtension(validExtension);

            // Asserts
            Assert.AreEqual(fileExtension.Value.Value, expected);
        }

        [TestCase("*.Jpeg")]
        [TestCase("*.jPeg")]
        [TestCase("*.bmp")]
        public void TestOuputExcludesAsteriskDot(string validExtension)
        {
            // Arrange
            string expected = validExtension.ToUpperInvariant().Substring(2);

            //Action
            FileExtension fileExtension = new FileExtension(validExtension);

            // Asserts
            Assert.AreEqual(fileExtension.Value.Value, expected);
        }

        [TestCase("*.Jpe.g")]
        [TestCase("*.jPe.g")]
        [TestCase("*.bm.p")]
        public void TestExtensionsMustNotContainFullStops(string invalidExtension)
        {
            // Arrange
            string expected = invalidExtension.ToUpperInvariant().Substring(2);
            FileExtension fileExtension = null;
            try
            {
                //Action
                fileExtension = new FileExtension(invalidExtension);
                // Asserts
                Assert.Fail("Invalid path including full stops was accepted in error.");
            }
            catch (InvalidCastException ex)
            {
                // Asserts
                Assert.IsTrue(ex.Message.Contains("Input string is not a valid file extension. Error: "));
                Assert.IsTrue(ex.Message.Contains("The extension should not contain full stops."));
            }
        }

        //[Test]
        //public static void TestTemplate()
        //{
        //    // Arrange

        //    // Act

        //    // Asserts
        //}
    }
}
