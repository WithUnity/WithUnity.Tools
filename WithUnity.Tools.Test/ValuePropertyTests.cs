/* Copyright 2015-2020 Robin Murison

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. */
using NUnit.Framework;
using System;

namespace WithUnity.Tools.Tests
{
    public sealed class TestStringValueProperty : ValueProperty<TestStringValueProperty, string>
    {
        private TestStringValueProperty(MayBe<String> value)
            : base(value)
        {
        }

        private TestStringValueProperty(String value) 
            : base(value)
        {
        }

#pragma warning disable CA2225 // Operator overloads have named alternates
        /* There already is one its called the constructor. */
        public static implicit operator TestStringValueProperty(MayBe<string> value)
#pragma warning restore CA2225 // Operator overloads have named alternates
        {
            return new TestStringValueProperty(value);
        }

#pragma warning disable CA2225 // Operator overloads have named alternates
        /* There already is one its called the constructor. */
        public static implicit operator TestStringValueProperty(String value)
#pragma warning restore CA2225 // Operator overloads have named alternates
        {
            return new TestStringValueProperty(value);
        }

        protected override bool EqualsCore(String other)
        {
            return Value.Equals(other);
        }

        public new int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }

    [TestFixture]
    public class ValuePropertyTests
    {
        [TestCase("Its a good night from me.")]
        [TestCase("And a good night from him.")]
        [TestCase(null)]
        public void TestValuePropertyEqualsStringSuccees(string value)
        {
            bool ActualResult; // Declared outside the try block so that is visible in the catch blocks.
            try
            {
                // Arrange
                TestStringValueProperty testValue = value;

                //act
                ActualResult = testValue.Equals(value);

                // Assert
                Assert.IsTrue(ActualResult);
            }
            catch (ArgumentNullException)
            {
                Assert.IsNull(value);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            /* Specifically testing that an invalid exception is not thrown. */
            catch (Exception ex)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                Assert.Fail($"Wrong exception thrown. Type is: {ex.GetType().Name}, Message is: {ex.Message}.");
            }
        }

        [TestCase("Its a good night from me.")]
        [TestCase("And a good night from him.")]
        [TestCase(null)]
        public void TestValuePropertyEqualsMayBeStringSuccees(string value)
        {
            try
            {
                // Arrange
                TestStringValueProperty testValue = value;
                MayBe<string> maybeValue = value;
                //act
                bool ActualResult = testValue.Equals(maybeValue);

                // Assert
                Assert.IsTrue(ActualResult);
            }
            catch (ArgumentNullException)
            {
                Assert.IsNull(value);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            // Specifically testing that no other exception is thrown
            catch (Exception ex)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                Assert.Fail($"Wrong exception thrown. Type is: {ex.GetType().Name}, Message is: {ex.Message}");
            }
        }

        [TestCase("Its a good night from me.")]
        [TestCase("And a good night from him.")]
        [TestCase(null)]
        public void TestValuePropertyEqualsValuePropertySuccees(string value)
        {
            try
            {
                // Arrange
                TestStringValueProperty testValue = value;
                TestStringValueProperty testValue2 = value.Duplicate();
                //act
                bool ActualResult = testValue.Equals(testValue2);

                // Assert
                Assert.IsTrue(ActualResult);
            }
            catch (ArgumentNullException)
            {
                Assert.IsNull(value);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            /* Specifically testing that an invalid exception is not thrown. */
            catch (Exception ex)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                Assert.Fail($"Wrong exception thrown. Type is: {ex.GetType().Name}, Message is: {ex.Message}");
            }
        }

        [TestCase("Its a good night from me.")]
        [TestCase("And a good night from him.")]
        [TestCase(null)]
        public void TestValuePropertyEqualsEqualsStringSuccees(string value)
        {
            try
            {
                // Arrange
                TestStringValueProperty testValue = value;

                //act
                bool ActualResult = value == testValue;

                // Assert
                Assert.IsTrue(ActualResult);
            }
            catch (ArgumentNullException)
            {

                Assert.IsNull(value);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            /* Specifically testing that an invalid exception is not thrown. */
            catch (Exception ex)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                Assert.Fail($"Wrong exception thrown: type: {ex.GetType().Name}, Message: {ex.Message}");
            }
        }

        [TestCase("Its a good night from me.")]
        [TestCase("And a good night from him.")]
        [TestCase(null)]
        public void TestValuePropertyEqualsEqualsMayBeStringSuccees(string value)
        {
            try
            {
                TestStringValueProperty testValue = value;
                // Arrange
                MayBe<string> maybeValue = value;
                //act
                bool ActualResult = testValue == maybeValue;

                // Assert
                Assert.IsTrue(ActualResult);
            }
            catch (ArgumentNullException)
            {

                Assert.IsNull(value);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            /* Specifically testing that an invalid exception is not thrown. */
            catch (Exception ex)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                Assert.Fail($"Wrong exception thrown: type: {ex.GetType().Name}, Message: {ex.Message}");
            }
        }

        [TestCase("Its a good night from me.")]
        [TestCase("And a good night from him.")]
        [TestCase(null)]
        public void TestValuePropertyEqualsEqualsValuePropertySuccees(string value)
        {
            try
            {
                // Arrange
                TestStringValueProperty testValue = value;
                TestStringValueProperty testValue2 = value.Duplicate();
                //act
                bool ActualResult = testValue == testValue2;

                // Assert
                Assert.IsTrue(ActualResult);
            }
            catch (NullReferenceException)
            {
                Assert.IsNull(value);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            /* Specifically testing that an invalid exception is not thrown. */
            catch (Exception ex)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                Assert.Fail($"Wrong exception thrown: type: {ex.GetType().Name}, Message: {ex.Message}");
            }
        }

        [TestCase("Its a good night from me.", "And a good night from him.")]
        [TestCase("And a good night from him.", "Its a good night from me.")]
        [TestCase(null, "Its a good night from me.")]
        [TestCase("Its a good night from me.", null)]
        public void TestValuePropertyEqualsStringFails(string value1, string value2)
        {
            try
            {
                // Arrange
                TestStringValueProperty testValue = value1;

                //act
                bool ActualResult = testValue.Equals(value2);

                // Assert
                Assert.IsFalse(ActualResult);
            }
            catch (ArgumentNullException)
            {

                Assert.IsTrue(value1 == null || value2 ==null);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            /* Specifically testing that an invalid exception is not thrown. */
            catch (Exception ex)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                Assert.Fail($"Wrong exception thrown: type: {ex.GetType().Name}, Message: {ex.Message}");
            }
        }

        [TestCase("Its a good night from me.", "And a good night from him.")]
        [TestCase("And a good night from him.", "Its a good night from me.")]
        [TestCase(null, "Its a good night from me.")]
        [TestCase("Its a good night from me.", null)]
        public void TestValuePropertyEqualsMayBeStringFails(string value1, string value2)
        {
            try
            {
                // Arrange
                TestStringValueProperty testValue = value1;
                MayBe<string> maybeValue = value2;
                // Act
                bool ActualResult = testValue.Equals(maybeValue);

                // Assert
                Assert.IsFalse(ActualResult);
            }
            catch (ArgumentNullException)
            {

                Assert.IsNull(value1);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            /* Specifically testing that an invalid exception is not thrown. */
            catch (Exception ex)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                Assert.Fail($"Wrong exception thrown: type: {ex.GetType().Name}, Message: {ex.Message}");
            }
        }

        [TestCase("Its a good night from me.", "And a good night from him.")]
        [TestCase("And a good night from him.", "Its a good night from me.")]
        [TestCase(null, "Its a good night from me.")]
        [TestCase("Its a good night from me.", null)]
        public void TestValuePropertyEqualsValuePropertyFails(string value1, string value2)
        {
            try
            {
                // Arrange
                TestStringValueProperty testValue = value1;
                TestStringValueProperty testValue2 = value2;
                // Act
                bool ActualResult = testValue.Equals(testValue2);

                // Assert
                Assert.IsFalse(ActualResult);
            }
            catch (ArgumentNullException)
            {

                Assert.IsTrue(value1 == null || value2 == null);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            /* Specifically testing that an invalid exception is not thrown. */
            catch (Exception ex)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                Assert.Fail($"Wrong exception thrown: type: {ex.GetType().Name}, Message: {ex.Message}");
            }
        }

        [TestCase("Its a good night from me.")]
        [TestCase("And a good night from him.")]
        [TestCase(null)]
        public void TestValuePropertyEqualsHeldTypeSuccees(string value)
        {
            try
            {
                // Arrange
                TestStringValueProperty testValue = value;
                string copiedValue = null;
                if (value != null)
                {
                    copiedValue = value.Duplicate();
                }
                //act
                bool ActualResult = testValue == copiedValue;

                // Assert
                Assert.IsTrue(ActualResult);
            }
            catch (ArgumentNullException)
            {
                Assert.IsNull(value);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            /* Specifically testing that an invalid exception is not thrown. */
            catch (Exception ex)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                Assert.Fail($"Wrong exception thrown: type: {ex.GetType().Name}, Message: {ex.Message}");
            }
        }

        [TestCase("Its a good night from me.")]
        [TestCase("And a good night from him.")]
        [TestCase(null)]
        public void TestNullValuePropertyEqualsHeldTypeSuccees(string value)
        {
            try
            {
                // Arrange
                TestStringValueProperty testValue = null;
                // Act
                bool ActualResult = testValue == value;

                // Assert
                Assert.IsFalse(ActualResult);
            }
            catch (ArgumentNullException)
            {
                Assert.IsNull(value);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            /* Specifically testing that an invalid exception is not thrown. */
            catch (Exception ex)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                Assert.Fail($"Wrong exception thrown: type: {ex.GetType().Name}, Message: {ex.Message}");
            }
        }
    }
}
