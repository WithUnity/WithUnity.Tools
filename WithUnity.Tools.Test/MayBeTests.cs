/* Copyright 2015-2020 Robin Murison

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. */
using System;
using NUnit.Framework;

namespace WithUnity.Tools.Tests
{ 
    using Tools;

    [TestFixture]
    public class MayBeTests
    {
        [Test]
        public void TestMayBeNullEqualsObjectnull()
        {
            // Arrange
            MayBe<string> nullMaybe1 = null;
            MayBe<string> nullMaybe2 = null;

            // Act
            bool actualResult = nullMaybe1.Equals(nullMaybe2 as object);

            // Assert
            Assert.IsTrue(actualResult);
        }

        [Test]
        public void TestMayBeNullNotEqualToNotNullObject()
        {
            // Arrange
            MayBe<string> nullMaybe = null;
            MayBe<string> Maybe2 = "Hello world";

            // Act
            bool actualResult = nullMaybe.Equals(Maybe2 as object);

            // Assert
            Assert.IsFalse(actualResult);
        }

        [TestCase("Any old non-empty string")]
        [TestCase("")]
        [TestCase("Another odd string")]
        public void MayBeValueTest(string anyOldString)
        {
            // Arrange
            MayBe<string> mayBe = anyOldString;

            // Act  & Assert (Check HasValue & HasNoValue return the correct values and that the Value returns the initial string)
            Assert.IsTrue(mayBe.HasValue);
            Assert.IsFalse(mayBe.HasNoValue);
            Assert.AreEqual(mayBe.Value, anyOldString);
        }

        [Test]
        public void MayBeNoValueTest()
        {
            // Arrange
            MayBe<string> mayBe = null;
            // Act  & Assert (Check HasValue & HasNoValue return the correct values and that the Value throws the correct exception.)
            Assert.IsFalse(mayBe.HasValue);
            Assert.IsTrue(mayBe.HasNoValue);
            try
            {
                if (mayBe.Value == null)
                {
                    Assert.Fail("mayBe.Value should throw an exception.");
                }
            }
            catch (InvalidOperationException)
            {
                // The Expected exception
            }
            catch (Exception ex)
            {
                Assert.Fail($"Wrong exception thrown: {ex.GetType().Name}, Message:{ex.Message}.");
            }
        }


        /* Comparison operators between MayBe and MayBe containers. */
        [TestCase("A quick brown fox jumped of the lazy cow.")]
        [TestCase("A slow red fox fell asleep under a log.")]
        [TestCase("The cow got away.")]
        [TestCase(null)]
        public void TestEqualsEqualsSuccess(string aString)
        {
            // Arrange
            MayBe<string> maybe1 = aString;
            MayBe<string> maybe2 = aString?.Duplicate();
            // Act & Assert  ( == operator under test.)
            Assert.IsTrue(maybe1 == maybe2);
        }

        [TestCase("A quick brown fox jumped of the lazy cow.")]
        [TestCase("A slow red fox fell asleep under a log.")]
        [TestCase("The cow got away.")]
        [TestCase(null)]
        public void TestNotEqualsFailure(string aString)
        {
            // Arrange
            MayBe<string> maybe1 = aString;
            MayBe<string> maybe2 = aString != null ? aString.Duplicate() : null;
            // Act & Assert  ( != operator under test.)
            Assert.IsFalse(maybe1 != maybe2);
        }

        [TestCase("A quick brown fox jumped of the lazy cow.", "A slow cow walked over a sleeping fox.")]
        [TestCase("A slow red fox fell asleep under a log.", " The cow walked away")]
        [TestCase("The cow got away.", "The fox went home in disgust")]
        [TestCase(null, "A string")]
        [TestCase("A string", null)]
        public void TestNotEqualsSuccess(string aString, string aDifferentString)
        {
            if (aString != null)
                Assert.IsFalse(aString.Equals(aDifferentString, StringComparison.InvariantCulture));
            MayBe<string> maybe1 = aString;
            MayBe<string> maybe2 = aDifferentString;
            Assert.IsTrue(maybe1 != maybe2);
        }

        [TestCase("A quick brown fox jumped of the lazy cow.", "A slow cow walked over a sleeping fox.")]
        [TestCase("A slow red fox fell asleep under a log.", " The cow walked away")]
        [TestCase("The cow got away.", "The fox went home in disgust")]
        [TestCase(null, "A string")]
        [TestCase("A string", null)]
        public void TestEqualsEqualsFailure(string aString, string aDifferentString)
        {

            MayBe<string> maybe1 = aString;
            MayBe<string> maybe2 = aDifferentString;
            Assert.IsFalse(maybe1 == maybe2);
        }
        /* Comparison operators between objects and MayBe containers. */
        [TestCase("A quick brown fox jumped of the lazy cow.")]
        [TestCase("A slow red fox fell asleep under a log.")]
        [TestCase("The cow got away.")]
        [TestCase(null)]
        public void TestEqualsEqualsSuccessWithObject(string aString)
        {
            // Arrange
            MayBe<string> maybe1 = aString;
            // Act & Assert  ( == operator under test.)
            if (maybe1.HasValue)
                Assert.IsTrue(maybe1 == aString.Duplicate());
            else
            {
                Assert.IsNull(aString);
            }
        }

        [TestCase("A quick brown fox jumped of the lazy cow.")]
        [TestCase("A slow red fox fell asleep under a log.")]
        [TestCase("The cow got away.")]
        [TestCase(null)]
        public void TestNotEqualsFailureWithObject(String aString)
        {
            // Arrange
            MayBe<String> maybe1 = aString;
            // Act & Assert  ( != operator under test.)
            if (maybe1.HasValue)
            {
                // Act
                bool actual = maybe1 != aString.Duplicate();
                // Assert
                Assert.IsFalse(actual);
            }
            else
            {
                // Assert the arrange used a null string
                Assert.IsNull(aString);
            }
        }

        [TestCase("A quick brown fox jumped of the lazy cow.", "A slow cow walked over a sleeping fox.")]
        [TestCase("A slow red fox fell asleep under a log.", " The cow walked away")]
        [TestCase("The cow got away.", "The fox went home in disgust")]
        [TestCase(null, "A string")]
        [TestCase("A string", null)]
        public void TestNotEqualsSuccessWithObject(string aString, string aDifferentString)
        {
            if (aString != null)
                Assert.IsFalse(aString.Equals(aDifferentString,StringComparison.InvariantCulture));
            MayBe<string> maybe1 = aString;
            Assert.IsTrue(maybe1 != aDifferentString);
        }

        [TestCase("A quick brown fox jumped of the lazy cow.", "A slow cow walked over a sleeping fox.")]
        [TestCase("A slow red fox fell asleep under a log.", " The cow walked away")]
        [TestCase("The cow got away.", "The fox went home in disgust")]
        [TestCase(null, "A string")]
        [TestCase("A string", null)]
        public void TestEqualsEqualsFailureWithObject(string aString, string aDifferentString)
        {
            MayBe<string> maybe1 = aString;
            Assert.IsFalse(maybe1 == aDifferentString);
        }

        /* Equals Methods between this and objects and MayBe containers. */
        [TestCase("A quick brown fox jumped of the lazy cow.", "A slow cow walked over a sleeping fox.")]
        [TestCase("A slow red fox fell asleep under a log.", " The cow walked away")]
        [TestCase("The cow got away.", "The fox went home in disgust")]
        [TestCase(null, "A string")]
        [TestCase("A string", null)]
        [TestCase("A string", 1)]
        [TestCase("A string", 0.45)]
        public void TestEqualsObjectFails(string string1, object object2)
        {
            MayBe<string> object1 = string1;
            Assert.IsFalse(object1.Equals(object2));
        }

        [TestCase("A quick brown fox jumped of the lazy cow.", "A slow cow walked over a sleeping fox.")]
        [TestCase("A slow red fox fell asleep under a log.", " The cow walked away")]
        [TestCase("The cow got away.", "The fox went home in disgust")]
        [TestCase(null, "A string")]
        [TestCase("A string", null)]
        public void TestEqualsMayBeFails(string object1, string object2)
        {
            MayBe<string> mayBe1 = object1;
            MayBe<string> mayBe2 = object2;
            Assert.IsFalse(mayBe1.Equals(mayBe2));
        }

        [TestCase("A quick brown fox jumped of the lazy cow.", "A slow cow walked over a sleeping fox.")]
        [TestCase("A slow red fox fell asleep under a log.", " The cow walked away")]
        [TestCase("The cow got away.", "The fox went home in disgust")]
        [TestCase(null, "A string")]
        [TestCase("A string", null)]
        [TestCase("A string", 1)]
        [TestCase("A string", 0.45)]
        public void TestEqualsObjectFalseSuccess(String string1, object object2)
        {
            MayBe<String> object1 = string1;
            if (object1.HasValue)
                Assert.IsFalse(object1.Equals(object2));
            else
                Assert.IsNull(string1);
        }

        [TestCase("A quick brown fox jumped of the lazy cow.")]
        [TestCase("A slow red fox fell asleep under a log.")]
        [TestCase("The cow got away.")]
        [TestCase(null)]
        public void TestEqualsMayBeSuccess(string aString)
        {
            // Arrange
            MayBe<string> maybe1 = aString;
            MayBe<string> maybe2 = aString?.Duplicate();
            // Act & Assert  ( Equals method under test.)
            Assert.IsTrue(maybe1.Equals(maybe2));
        }
    }
}
