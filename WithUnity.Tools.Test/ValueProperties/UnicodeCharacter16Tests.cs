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
    public class UnicodeCaharacter16Tests
    {
        public void NullStringThrowsMullReferenceException()
        {
            // Arrange
            // Act
            try
            {
                UnicodeCharacter16 fileExtension = new UnicodeCharacter16(null);

                // Asserts
                Assert.Fail("Should throw NullReferenceException");

            }
            catch (Exception ex)
            {
                // Asserts
                Assert.IsTrue(ex is InvalidCastException);
            }
        }

        [TestCase("")]
        [TestCase("ass")]
        [TestCase("ssssss")]
        public void InvalidnumberOfCodePointsShouldThrowInvalidCastException(string uc)
        {
            // Arrange
            // Act
            try
            {
                UnicodeCharacter16 fileExtension = new UnicodeCharacter16(uc);
                // Asserts
                Assert.Fail("Wrong levngth string did not fail");
            }
            catch (Exception ex)
            {
                // Asserts
                Assert.IsTrue(ex is InvalidCastException);
            }
        }

        public void SingleHighSurrogateShouldThrowInvalidCastException()
        {
            for(int uc = 0xD800; uc < 0xDC00; uc++)
            {
                // Arrange
                string highSurrogateCodePoint = new string(new char[] { (char)uc });
                // Act
                try
                {
                    UnicodeCharacter16 uc16 = new UnicodeCharacter16(highSurrogateCodePoint);
                    // Asserts
                    Assert.Fail("A high surtrogate code point slipped through");
                }
                catch (Exception ex)
                {
                    // Asserts
                    Assert.IsTrue(ex is InvalidCastException);
                }

            }
        }

        public void SingleLowhSurrogateShouldThrowInvalidCastException()
        {
            for (int uc = 0xDC00; uc < 0xE000; uc++)
            {
                // Arrange
                string lowSurrogateCodePoint = new string(new char[] { (char)uc });
                // Act
                try
                {
                    UnicodeCharacter16 uc16 = new UnicodeCharacter16(lowSurrogateCodePoint);
                    // Asserts
                    Assert.Fail("A low surtrogate code point slipped throughas a single codepoint");
                }
                catch (Exception ex)
                {
                    // Asserts
                    Assert.IsTrue(ex is InvalidCastException);
                }
            }
        }

        public void ASingleNonSurrogateCodePointShouldNotThrowInvalidCastException()
        {
            // The Lower list
            for (int uc = 0; uc < 0xDC00; uc++)
            {
                // Arrange
                string singleCodePoint = new string(new char[] { (char)uc });
                // Act
                try
                {
                    UnicodeCharacter16 uc16 = new UnicodeCharacter16(singleCodePoint);
                    // Asserts
                }
                catch (Exception ex)
                {
                    // Asserts
                    Assert.Fail($"A single code point threw {ex.GetType().Name}");
                }
            }
            // The upper list
            for (int uc = 0xE000; uc < 0xFFFE; uc++)
            {
                // Arrange
                string singleCodePoint = new string(new char[] { (char)uc });
                // Act
                try
                {
                    UnicodeCharacter16 uc16 = new UnicodeCharacter16(singleCodePoint);
                    // Asserts
                }
                catch (Exception ex)
                {
                    // Asserts
                    Assert.Fail($"A single code point threw  {ex.GetType().Name}");
                }
            }
        }

        [TestCase(0xfffe)]
        [TestCase(0xffff)]
        public void InvalidEndCodePointShouldThrowInvalidCastException(int invalidCodePoint)
        {
            // Arrange
            string singleCodePoint = new string(new char[] { (char)invalidCodePoint });
            // Act
            try
            {
                UnicodeCharacter16 uc16 = new UnicodeCharacter16(singleCodePoint);
                // Asserts
                Assert.Fail("Terminal code points are not valid");
            }
            catch (Exception ex)
            {
                // Asserts
                Assert.IsTrue(ex is InvalidCastException);
            }
        }

        [TestCase(0xDBFF)]
        [TestCase(0xE000)]
        public void ADoubleCodePointNotStartingwithAHighSurrogateCodePointShouldThrowInvalidCastException(int invalidLowSurrogateCodePoint)
        {
            // Arrange
            string singleCodePoint = new string(new char[] { (char)0xD800, (char)invalidLowSurrogateCodePoint });
            // Act
            try
            {
                UnicodeCharacter16 uc16 = new UnicodeCharacter16(singleCodePoint);
                // Asserts
                Assert.Fail("Character ending with an invalid low Surrogate Code Point did not throw an exceptioon");
            }
            catch (Exception ex)
            {
                // Asserts
                Assert.IsTrue(ex is InvalidCastException);
            }
        }

        [TestCase(0x0000)]
        [TestCase(0xD7FF)]
        [TestCase(0xDC00)]
        [TestCase(0xFFFD)]
        public void ADoubleCodePointNotEndingwithALlowSurrogateCodePointShouldThrowInvalidCastException(int invalidHighSurrogateCodePoint)
        {
            // Arrange
            string singleCodePoint = new string(new char[] { (char)invalidHighSurrogateCodePoint, (char)0xDC00 });
            // Act
            try
            {
                UnicodeCharacter16 uc16 = new UnicodeCharacter16(singleCodePoint);
                // Asserts
                Assert.Fail("Character Starting with an invalid High Surrogate Code Point did not throw an exceptioon");
            }
            catch (Exception ex)
            {
                // Asserts
                Assert.IsTrue(ex is InvalidCastException);
            }
        }

        /// <summary>
        ///  U+D800–U+DBFF (1,024 code points) are known as high-surrogate code points, and code points in the range U+DC00–U+DFFF
        /// </summary>
        /// <param name="validHighSurrogateCodePoint"></param>
        /// <param name="validLowSurrogateCodePoint"></param>

        [TestCase(0xD800, 0xDC00)]
        [TestCase(0xDBFF, 0xDC00)]
        [TestCase(0xD800, 0xDFFF)]
        [TestCase(0xDBFF, 0xDFFF)]
        public void AValidUnicodeCharacterShouldNotThrowInvalidCastException(int validHighSurrogateCodePoint, int validLowSurrogateCodePoint)
        {
            // Arrange
            string singleCharacter = new string(new char[] { (char)validHighSurrogateCodePoint, (char)validLowSurrogateCodePoint });
            // Act
            try
            {
                UnicodeCharacter16 uc16 = new UnicodeCharacter16(singleCharacter);
                // Asserts
            }
            catch (Exception ex)
            {
                // Asserts
                Assert.Fail($"A valid character {validHighSurrogateCodePoint:X}:{validLowSurrogateCodePoint:X} threw {ex.GetType().Name}");
            }
        }

    }
}
