/* Copyright 2015-2021 Robin Murison

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. */
using NUnit.Framework;

namespace WithUnity.Tools.Tests
{
    /// <summary>
    /// Tests StringExtensions
    /// </summary>
    [TestFixture]
    public sealed class StringExtensionTest
    {
        [Test]
        public void AreDuplicatedReferencesDifferent()
        {
            // Arrange
            string s1 = "assd3";
            // Act
            string s2 = s1.Duplicate();
            // Assert
            Assert.IsFalse(object.ReferenceEquals(s1, s2));
        }

        [Test]
        public void AreContentsTheSame()
        {
            // Arrange
            string s1 = "assd3";
            // Act
            string s2 = s1.Duplicate();
            // Assert
            Assert.IsTrue(s1.Equals(s2));
        }

        [Test]
        public void CanItDuplicateNull()
        {
            // Arrange
            string s1 = null;
            // Act
            string s2 = s1.Duplicate();
            // Assert
            Assert.IsNull(s2);
        }
    }
}
