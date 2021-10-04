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
        public void NullOrBlankPathThrowsInvalidCastException(string nullPath)
        {
            // Arrange
            // Act
            try
            {
                FileExtension fileExtension = nullPath;
                // Asserts
                Assert.Fail("null or empty string was accepted in error.");
            }
            catch (InvalidCastException ex)
            {
                // Asserts
                Assert.IsTrue(ex.Message.Contains("File extensions cannot be blank or null")|| ex.Message.Contains("Null file extension.") || ex.Message.Contains("File extensions has leading or trailing white space characters."));
            }
            catch (Exception ex)
            {
                // Asserts
                Assert.Fail($"Unexpected Exception thrown Type: {ex.GetType().Name} Message: {ex.Message}");
            }
        }

        [TestCase("/a")]
        [TestCase("\\a")]
        [TestCase(":a")]
        [TestCase(".a")]
        public void TestInvalidExtensionCharacters(string invalidPath)
        {
            // Arrange
            FileExtension fileExtension = null;
            // Act
            try
            {
                fileExtension = invalidPath;
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
                    case '.':
                        Assert.IsTrue(ex.Message.Contains(@"The fileExtension should not contain full stops."));
                        break;
                    default:
                        Assert.Fail("Someone added a testCase without adding the check for specific errors.");
                        break;
                }
            }
            catch (Exception ex)
            {
                // Asserts
                Assert.Fail($"Unexpected Exception thrown Type: {ex.GetType().Name} Message: {ex.Message}");
            }
        }
    }
}
