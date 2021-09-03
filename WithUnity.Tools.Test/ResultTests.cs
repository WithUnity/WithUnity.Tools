/* Copyright 2015-2020 Robin Murison

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. */
using NUnit.Framework;
using System;
using System.Data;

namespace WithUnity.Tools.Tests
{
    /// <summary>
    /// Summary description for ResultTests
    /// </summary>
    [TestFixture]
    public class ResultTests
    {
        [Test]
        public void TestcreateFailure()
        {
            // Arrange
            const string failureMessage = "It failed";

            // Act
            Result result = Result.Fail(failureMessage);


            //Assert
            Assert.IsTrue(result.IsFailure);
            Assert.AreEqual(result.Error, failureMessage);
            Assert.IsFalse(result.IsSuccess);
        }

        [Test]
        public void TestcreateSuccess()
        {
            // Arrange

            // Act
            Result result = Result.Ok();

            //Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(result.Error, string.Empty);
            Assert.IsFalse(result.IsFailure);
        }

        [Test]
        public void TestCreateSuccessWithValue()
        {
            // Arrange
            const string SuccessMessage = "Successful created";

            // Act
            Result<string> result = Result.Ok<string>(SuccessMessage);

            //Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(result.Error, string.Empty);
            Assert.IsFalse(result.IsFailure);
            Assert.AreEqual(result.Value, SuccessMessage);
        }

        [Test]
        public void TestCreateFailureWithTemplate()
        {
            // Arrange
            const string failureMessage = "failed to create";

            // Act
            Result<int> result = Result.Fail<int>(failureMessage);

            //Assert
            Assert.IsTrue(result.IsFailure);
            Assert.AreEqual(result.Error, failureMessage);
            Assert.IsFalse(result.IsSuccess);
            try
            {
                int i = result.Value;
                Assert.Fail("Accessing the result.Value should throw an exception");
            }
            catch (InvalidOperationException) { }
            catch (Exception ex)
            {
                Assert.Fail($"The wrong exception was thrown: {ex.GetType().Name} Message: {ex.Message}.");
            }
        }

        // Curious double entry. You can create a failure but not Success directly on Result<T>
        [Test]
        public void TestCreateFailureWithTemplateOnTemplate()
        {
            // Arrange
            const string failureMessage = "failed to create";

            // Act
            Result<int> result = Result<int>.Fail<int>(failureMessage);

            //Assert
            Assert.IsTrue(result.IsFailure);
            Assert.AreEqual(result.Error, failureMessage);
            Assert.IsFalse(result.IsSuccess);
            try
            {
                int i = result.Value;
                Assert.Fail("Accessing the result.Value should throw an exception");
            }
            catch (InvalidOperationException) { }
            catch (Exception ex)
            {
                Assert.Fail($"The wrong exception was thrown: {ex.GetType().Name} Message: {ex.Message}.");
            }
        }

        [Test]
        public void TestCreateSuccessWithTemplateOnTemplate()
        {
            // Arrange

            // Act
            Result<int> result = Result<int>.Ok<int>(1);

            //Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(result.Error, string.Empty);
            Assert.IsFalse(result.IsFailure);
            Assert.AreEqual(result.Value, 1);
        }

        internal sealed class TestClass
        {
            internal int Data { get; }
            internal TestClass(int data)
            {
                Data = data;
            }
        }

        [Test]
        public void TestResultTInitializeFailure()
        {
            // Arrange
            const string failureMessage = "It failed";

            // Act
            Result result = Result.Initialize<TestClass>(null, failureMessage);

            //Assert
            Assert.IsTrue(result.IsFailure);
            Assert.AreEqual(result.Error, failureMessage);
            Assert.IsFalse(result.IsSuccess);
        }

        [Test]
        public void TestResultTInitializeSuccess()
        {
            // Arrange
            const string failureMessage = "It failed";

            // Act
            Result<TestClass> result = Result.Initialize<TestClass>(new TestClass(32), failureMessage);

            //Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(result.Value.Data, 32);
            Assert.IsFalse(result.IsFailure);
        }


        [Test]
        public void TestResultInitializeFailure()
        {
            // Arrange
            const string failureMessage = "It failed";

            // Act
            Result result = Result.Initialize(false, failureMessage);

            //Assert
            Assert.IsTrue(result.IsFailure);
            Assert.AreEqual(result.Error, failureMessage);
            Assert.IsFalse(result.IsSuccess);
        }

        [Test]
        public void TestResultInitializeSuccess()
        {
            // Arrange
            const string failureMessage = "It failed";

            // Act
            Result result = Result.Initialize(true, failureMessage);

            //Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.IsFalse(result.IsFailure);
            Assert.IsTrue(string.IsNullOrWhiteSpace(result.Error));
        }
        [Test]
        public void TestCreateWithException()
        {
            // Arrange
            const string failureMessage = "It failed";

            Exception exception = new ArgumentException(failureMessage);
            // Act
            Result result = Result.Fail(exception);


            //Assert
            Assert.IsTrue(result.IsFailure);
            Assert.True(result.Error.Contains(failureMessage));
            Assert.True(result.Error.Contains("TestCreateWithException"));
            Assert.True(result.Error.Contains(exception.GetType().Name));
            Assert.AreEqual(result.Exception, exception);
            Assert.IsFalse(result.IsSuccess);
        }

        [Test]
        public void TestCreateTemplateWithException()
        {
            // Arrange
            const string failureMessage = "It failed";

            Exception exception = new ArgumentException(failureMessage);
            // Act
            Result<TestClass> result = Result.Fail<TestClass>(exception);

            //Assert
            Assert.IsTrue(result.IsFailure);
            Assert.True(result.Error.Contains(failureMessage));
            Assert.True(result.Error.Contains("TestCreateTemplateWithException"));
            Assert.True(result.Error.Contains(exception.GetType().Name));
            Assert.AreEqual(result.Exception, exception);
            Assert.IsFalse(result.IsSuccess);
        }

        #region Testing creation of Calling Method
        [Test]
        public void TestResultFailMessageCallingMethod()
        {
            // Arrange
            const string failureMessage = "It failed";

            // Act
            Result result = Result.Fail(failureMessage);

            //Assert
            Assert.AreEqual(result.CallingMethod, ReflectiveTools.CurrentMethod());
        }

        [Test]
        public void TestResultFailExceptionCallingMethod()
        {
            // Arrange
            Exception exception = new DataException("It failed");

            // Act
            Result result = Result.Fail(exception);

            //Assert
            Assert.AreEqual(result.CallingMethod, ReflectiveTools.CurrentMethod());
        }

        [Test]
        public void TestResultTemplateFailMessageCallingMethod()
        {
            // Arrange
            const string failureMessage = "It failed";

            // Act
            Result result = Result.Fail<TestClass>(failureMessage);

            //Assert
            Assert.AreEqual(result.CallingMethod, ReflectiveTools.CurrentMethod());
        }

        [Test]
        public void TestResultTemplateFailExceptionCallingMethod()
        {
            // Arrange
            Exception exception = new DataException("It failed");

            // Act
            Result result = Result.Fail<TestClass>(exception);

            //Assert
            Assert.AreEqual(result.CallingMethod, ReflectiveTools.CurrentMethod());
        }

        [Test]
        public void TestResultOkCallingMethod()
        {
            // Arrange
            // Act
            Result result = Result.Ok();

            //Assert
            Assert.AreEqual(result.CallingMethod, ReflectiveTools.CurrentMethod());
        }

        [Test]
        public void TestResultTemplateOkCallingMethod()
        {
            // Arrange
            // Act
            Result result = Result.Ok<TestClass>(new TestClass(32));

            //Assert
            Assert.AreEqual(result.CallingMethod, ReflectiveTools.CurrentMethod());
        }

        [Test]
        public void TestResultTemplateInitializeSucessCallingMethod()
        {
            // Arrange
            const string failureMessage = "It failed";
            // Act
            Result<TestClass> result = Result.Initialize<TestClass>(new TestClass(32), failureMessage);

            //Assert
            Assert.AreEqual(result.CallingMethod, ReflectiveTools.CurrentMethod());
        }

        [Test]
        public void TestResultTemplateInitializeFailureCallingMethod()
        {
            // Arrange
            const string failureMessage = "It failed";
            // Act
            Result<TestClass> result = Result.Initialize<TestClass>(null, failureMessage);

            //Assert
            Assert.AreEqual(result.CallingMethod, ReflectiveTools.CurrentMethod());
        }

        [Test]
        public void TestResultImplicitConversionCallingMethod()
        {
            // Arrange
            // Act
            Result<TestClass> result = new TestClass(32);

            //Assert
            Assert.AreEqual(result.CallingMethod, ReflectiveTools.CurrentMethod());
        }

        [Test]
        public void TestResultTemplatedInitializationCallingMethod()
        {
            // Arrange
            const string failureMessage = "It failed";
            // Act
            Result<TestClass> result = Result<TestClass>.Initialize(null, failureMessage);

            //Assert
            Assert.AreEqual(result.CallingMethod, ReflectiveTools.CurrentMethod());
        }
        #endregion
    }
}
