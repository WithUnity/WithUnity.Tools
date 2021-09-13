/* Copyright 2015-2020 Robin Murison

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. */
using System;
using NUnit.Framework;
using WithUnity.Tools;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Security.Cryptography;
using System.Globalization;
using System.Reflection;

namespace WithUnity.Tools.Tests
{
    /// <summary>
    /// Summary description for ResultExtensionsTests
    /// </summary>
    [TestFixture]
    public class ResultExtensionsTests
    {
        [TestCase("John Wayne")]
        [TestCase("Ronald Reagon")]
        public void TestToResultSuccess(string param)
        {
            // Arrange
            MayBe<string> aString = param;
            // Act
            Result<string> result = aString.ToResult<string>("This is a null String.");

            //Asserts
            Assert.AreEqual(result.Value, aString.Value);
            Assert.IsTrue(result.IsSuccess);
            Assert.IsFalse(result.IsFailure);
            Assert.AreEqual(result.Error, string.Empty);
        }

        [Test]
        public void TestToResultFailure()
        {
            // Arrange
            MayBe<String> aString = null;
            const string errorMessage = "This is a null String.";

            // Act
            Result<String> result = aString.ToResult<String>(errorMessage);

            //Asserts
            Assert.IsTrue(result.IsFailure);
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual(result.Error, errorMessage);
            try
            {
                string s = result.Value ?? "Null";
                Assert.Fail("result.Value should have thrown an exception when there is no data.");
            }
            catch (InvalidOperationException) { }
            catch (Exception ex)
            {
                Assert.Fail($"Wrong Exception thrown. Type:{ex.GetType().Name}, Message {ex.Message}");
            }
        }

        // Arrange
        const string FailedText = "Result has already failed.";
        const string ForbiddenActionOccured = "An action occrred erroneously 'OnSuccess' after a result has already failed.";

        /// <summary>
        /// Test the action does not occur on a preceeding failure
        /// </summary>
        [Test]
        public void TestOnSuccessActionFromFailure()
        {
            // Arrange
            Result result = Result.Fail(FailedText)
            // Action Assert
                .OnSuccess(() => Assert.Fail(ForbiddenActionOccured));

            // Assert on Action
            Assert.IsTrue(result.IsFailure);
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual(result.Error, FailedText);
        }

        /// <summary>
        /// Test the Action does occur on a successful Result
        /// </summary>
        [Test]
        public void TestOnSuccessActionFromSuccess()
        {
            // Arrange
            int testValue = 4;
            // Action 
            Result result = Result.Ok()
                .OnSuccess(() => testValue = 2);

            // Asserts
            Assert.AreEqual(testValue, 2);
            Assert.IsTrue(result.IsSuccess);
            Assert.IsFalse(result.IsFailure);
            Assert.AreEqual(result.Error, string.Empty);
        }

        /// <summary>
        /// Test the action does not occur on a preceeding failure
        /// </summary>
        [TestCase("Hello world")]
        [TestCase("GoodBye")]
        public void TestOnSuccessTWorks(string input)
        {
            // Arrange
            Result<string> result = Result.Ok<string>(input).OnSuccess<string>((value) => { return Result.Ok(); });
            // Assert on Action
            Assert.IsTrue(result.IsSuccess);
            Assert.IsFalse(result.IsFailure);
            Assert.AreEqual(result.Value, input);
        }

        /// <summary>
        /// Test the action does not occur on a preceeding failure
        /// </summary>
        [TestCase("Hello world")]
        [TestCase("GoodBye")]
        public void TestOnSuccessTFails(string input)
        {
            // Arrange
            Result<string> result = Result.Ok<string>(input)
                .OnSuccess<string>((value) => { return Result.Fail(input); });
            // Assert on Action
            Assert.IsTrue(result.IsFailure);
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual(result.Error, input);
        }

        /// <summary>
        /// Test the action does not occur on a preceeding failure
        /// </summary>
        [Test]
        public void TestOnSuccessActionFromTemplatedFailure()
        {

            Result<int> result = Result.Fail<int>(FailedText)
            // Action Assert
                .OnSuccess(() => Assert.Fail(ForbiddenActionOccured));

            // Assert on Action
            Assert.IsTrue(result.IsFailure);
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual(result.Error, FailedText);
        }

        /// <summary>
        /// Test the Action does occur on a successful Result
        /// </summary>
        [Test]
        public void TestOnSuccessActionFromTemplatedSuccess()
        {
            // Arrange
            int testValue = 4;
            // Action 
            Result<int> result = Result.Ok<int>(1)
                .OnSuccess(() => testValue = 2);

            // Asserts
            Assert.AreEqual(result.Value, 1);
            Assert.AreEqual(testValue, 2);
            Assert.IsTrue(result.IsSuccess);
            Assert.IsFalse(result.IsFailure);
            Assert.AreEqual(result.Error, string.Empty);
        }

        /// <summary>
        /// Test the action does not occur on a preceeding failure,
        /// When converting from one type to another.
        /// </summary>
        [Test]
        public void TestOnSuccessActionFromSwitchingTemplatedFailure()
        {

            Result<string> result = Result.Fail<int>(FailedText)
            // Action Assert
                .OnSuccess<int, string>((i) => { Assert.Fail(ForbiddenActionOccured); return i.ToString(CultureInfo.InvariantCulture); });

            // Assert on Action
            Assert.IsTrue(result.IsFailure);
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual(result.Error, FailedText);
        }

        /// <summary>
        /// Test the Action does occur on a successful Result,
        /// When converting from one type to another.
        /// </summary>
        [Test]
        public void TestOnSuccessActionFromSwitchingTemplatedSuccess()
        {
            // Arrange
            int testValue = 4;
            // Action 
            Result<string> result = Result.Ok<int>(1)
                .OnSuccess<int, string>((i) => { testValue = 2; return i.ToString(CultureInfo.InvariantCulture); });

            // Asserts
            Assert.AreEqual(result.Value, "1");
            Assert.AreEqual(testValue, 2);
            Assert.IsTrue(result.IsSuccess);
            Assert.IsFalse(result.IsFailure);
            Assert.AreEqual(result.Error, string.Empty);
        }

        /// <summary>
        /// Test the action does not occur on a preceeding failure,
        /// When replacing one type with another.
        /// </summary>
        [Test]
        public void TestOnSuccessActionFromSwapTemplatedFailure()
        {

            Result result = Result.Fail<int>(FailedText)
            // Action Assert
                .OnSuccess(() => { Assert.Fail(ForbiddenActionOccured); return; });

            // Assert on Action
            Assert.IsTrue(result.IsFailure);
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual(result.Error, FailedText);
        }

        /// <summary>
        /// Test the Action does occur on a successful Result,
        /// When converting from one type to another.
        /// </summary>
        [TestCase(1)]
        public void TestOnSuccessActionFromSwapTemplatedSuccess(int value)
        {
            // Arrange
            int testValue = 4;
            // Action 
            Result<string> result = Result.Ok<int>(value)
                .OnSuccess((i) => { testValue = 2; return i.ToString(CultureInfo.InvariantCulture); });

            // Asserts
            Assert.AreEqual(testValue, 2);
            Assert.IsTrue(result.IsSuccess);
            Assert.IsFalse(result.IsFailure);
            Assert.AreEqual(result.Error, string.Empty);
            Assert.AreEqual(result.Value, value.ToString(CultureInfo.InvariantCulture));
        }


        /// <summary>
        /// Test the action does not occur on a preceeding failure,
        /// When replacing one type with another.
        /// </summary>
        [Test]
        public void TestOnFailureActionWithFailure()
        {

            Result result = Result.Fail<int>(FailedText)
            // Action Assert
                .OnFailure(() => Result.Ok());

            // Assert on Action
            Assert.IsTrue(result.IsFailure);
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual(result.Error, FailedText);
        }

        /// <summary>
        /// Test the Action does occur on a successful Result,
        /// When converting from one type to another.
        /// </summary>
        [Test]
        public void TestOnFailureActionWithSuccess()
        {
            // Arrange
            // Action 
            Result result = Result.Ok()
                .OnFailure(() => Assert.Fail(ForbiddenActionOccured));

            // Asserts
            Assert.IsTrue(result.IsSuccess);
            Assert.IsFalse(result.IsFailure);
            Assert.AreEqual(result.Error, string.Empty);
        }

        /// <summary>
        /// Test the Action does occur on a successful Result,
        /// When converting from one type to another.
        /// </summary>
        [TestCase(true)]
        [TestCase(false)]
        public void TestActionOnResultOnBoth(bool testValue)
        {
            // Arrange
            const string errorMessage = "Testing the failed case.";
            Result result = testValue ? Result.Ok() : Result.Fail(errorMessage);
            
            // Action 
            result.OnBoth(r => Assert.IsTrue(r.IsSuccess == testValue));

            // Asserts
            Assert.IsTrue(result.IsSuccess == testValue);
            Assert.IsFalse(result.IsFailure == testValue);
            result.OnFailure(() => Assert.AreEqual(result.Error, errorMessage))
                .OnSuccess(() => Assert.AreEqual(result.Error, string.Empty));
        }

        /// <summary>
        /// Test the Action does occur on Both Result,
        /// When converting from one type to another.
        /// </summary>
        [TestCase(true)]
        [TestCase(false)]
        public void TestActionOnBoth(bool testValue)
        {
            // Arrange
            bool actual = false;
            Result result = testValue ? Result.Ok() : Result.Fail("Testing the failed case.");

            // Action 
            result.OnBoth(() => actual = true);

            // Asserts
            Assert.IsTrue(actual);
        }

        /// <summary>
        /// Test the Action does occur on a successful Result,
        /// When converting from one type to another.
        /// </summary>
        [Test]
        public void TestTemplatedUnwrapActionOnBoth()
        {
            // Arrange
            int i = Result.Ok<int>(1)
            // Action 
                .OnBoth<int>(r => { return (r as Result<int>).Value; });

            Assert.AreEqual(i, 1);
        }

        private const string PredicateFailure = "Predicate failed.";

        /// <summary>
        /// Test the action does not occur on a preceeding failure,
        /// When replacing one type with another.
        /// </summary>
        [Test]
        public void TestEnsureFailsOnIncomingFailure()
        {

            Result<int> result = Result.Fail<int>(FailedText)
            // Action Assert
                .Ensure((i) => false, PredicateFailure);

            // Assert on Action
            Assert.IsTrue(result.IsFailure);
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual(result.Error, FailedText);
        }

        /// <summary>
        /// Test the Action does occur on a successful Result,
        /// When converting from one type to another.
        /// </summary>
        [TestCase(1)]
        public void TestEnsureFailsOnPredicateFailure(int i)
        {
            // Arrange

            // Action 
            Result<int> result = Result.Ok<int>(i)
                .Ensure((j) => false, PredicateFailure);

            // Asserts
            Assert.IsTrue(result.IsFailure);
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual(result.Error, PredicateFailure);
        }

        /// <summary>
        /// Test the Action does occur on a successful Result,
        /// When converting from one type to another.
        /// </summary>
        [TestCase(1)]
        public void TestEnsurePassesOnResultOnSuccess(int i)
        {
            // Arrange
            // Action 
            Result<int> result = Result.Ok(i)
                .Ensure((j) => j == i, PredicateFailure);

            // Asserts
            Assert.IsTrue(result.IsSuccess);
            Assert.IsFalse(result.IsFailure);
            Assert.AreEqual(result.Error, string.Empty);
            Assert.AreEqual(result.Value, i);
        }

        /// <summary>
        /// Test the action does not occur on a preceeding failure,
        /// When replacing one type with another.
        /// </summary>
        [Test]
        public void TestEnsureOnBoolMethodTestFailsOnIncomingFailure()
        {

            Result<int> result = Result.Fail<int>(FailedText)
            // Action Assert
                .Ensure(() => false, PredicateFailure);

            // Assert on Action
            Assert.IsTrue(result.IsFailure);
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual(result.Error, FailedText);
        }

        /// <summary>
        /// Test the action does not occur on a preceeding failure,
        /// When replacing one type with another.
        /// </summary>
        [Test]
        public void TestEnsureOnBoolTestFailsOnIncomingFailure()
        {

            Result<int> result = Result.Fail<int>(FailedText)
            // Action Assert
                .Ensure(false, PredicateFailure);

            // Assert on Action
            Assert.IsTrue(result.IsFailure);
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual(result.Error, FailedText);
        }

        /// <summary>
        /// Test the Action does occur on a successful Result,
        /// When converting from one type to another.
        /// </summary>
        [TestCase(1)]
        public void TestEnsureFailsOnBoolMethodTestFailure(int i)
        {
            // Arrange

            // Action 
            Result<int> result = Result.Ok<int>(i)
                .Ensure(() => false, PredicateFailure);

            // Asserts
            Assert.IsTrue(result.IsFailure);
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual(result.Error, PredicateFailure);
        }

        /// <summary>
        /// Test the Action does occur on a successful Result,
        /// When converting from one type to another.
        /// </summary>
        [TestCase(1)]
        public void TestEnsureFailsOnBoolTestFailure(int i)
        {
            // Arrange

            // Action 
            Result<int> result = Result.Ok<int>(i)
                .Ensure(false, PredicateFailure);

            // Asserts
            Assert.IsTrue(result.IsFailure);
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual(result.Error, PredicateFailure);
        }

        /// <summary>
        /// Test the Action does occur on a successful Result,
        /// When converting from one type to another.
        /// </summary>
        [TestCase(1)]
        public void TestEnsurePassesOnResultOnBoolMethodTestSuccess(int i)
        {
            // Arrange
            // Action 
            Result<int> result = Result.Ok(i)
                .Ensure(() => true, PredicateFailure);

            // Asserts
            Assert.IsTrue(result.IsSuccess);
            Assert.IsFalse(result.IsFailure);
            Assert.AreEqual(result.Error, string.Empty);
            Assert.AreEqual(result.Value, i);
        }

        /// <summary>
        /// Test the Action does occur on a successful Result,
        /// When converting from one type to another.
        /// </summary>
        [TestCase(1)]
        public void TestEnsurePassesOnResultOnBoolTestSuccess(int i)
        {
            // Arrange
            // Action 
            Result<int> result = Result.Ok(i)
                .Ensure(true, PredicateFailure);

            // Asserts
            Assert.IsTrue(result.IsSuccess);
            Assert.IsFalse(result.IsFailure);
            Assert.AreEqual(result.Error, string.Empty);
            Assert.AreEqual(result.Value, i);
        }

        /// <summary>
        /// Test the action does not occur on a preceeding failure,
        /// When replacing one type with another.
        /// </summary>
        [Test]
        public void TestEnsureResultOnBoolMethodTestFailsOnIncomingFailure()
        {

            Result result = Result.Fail(FailedText)
            // Action Assert
                .Ensure(() => false, PredicateFailure);

            // Assert on Action
            Assert.IsTrue(result.IsFailure);
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual(result.Error, FailedText);
        }

        /// <summary>
        /// Test the action does not occur on a preceeding failure,
        /// When replacing one type with another.
        /// </summary>
        [Test]
        public void TestEnsureResultOnBoolTestFailsOnIncomingFailure()
        {

            Result result = Result.Fail(FailedText)
            // Action Assert
                .Ensure(false, PredicateFailure);

            // Assert on Action
            Assert.IsTrue(result.IsFailure);
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual(result.Error, FailedText);
        }

        /// <summary>
        /// Test the Action does occur on a successful Result,
        /// When converting from one type to another.
        /// </summary>
        [Test]
        public void TestEnsureResultFailsOnBoolMethofTestFailure()
        {
            // Arrange

            // Action 
            Result result = Result.Ok()
                .Ensure(() => false, PredicateFailure);

            // Asserts
            Assert.IsTrue(result.IsFailure);
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual(result.Error, PredicateFailure);
        }

        /// <summary>
        /// Test the Action does occur on a successful Result,
        /// When converting from one type to another.
        /// </summary>
        [Test]
        public void TestEnsureResultFailsOnBoolTestFailure()
        {
            // Arrange

            // Action 
            Result result = Result.Ok()
                .Ensure(false, PredicateFailure);

            // Asserts
            Assert.IsTrue(result.IsFailure);
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual(result.Error, PredicateFailure);
        }

        /// <summary>
        /// Test the Action does occur on a successful Result,
        /// When converting from one type to another.
        /// </summary>
        [Test]
        public void TestEnsureResultPassesOnResultOnBoolMethodTestSuccess()
        {
            // Arrange
            // Action 
            Result result = Result.Ok()
                .Ensure(() => true, PredicateFailure);

            // Asserts
            Assert.IsTrue(result.IsSuccess);
            Assert.IsFalse(result.IsFailure);
            Assert.AreEqual(result.Error, string.Empty);
        }

        /// <summary>
        /// Test the Action does occur on a successful Result,
        /// When converting from one type to another.
        /// </summary>
        [Test]
        public void TestEnsureResultPassesOnResultOnBoolTestSuccess()
        {
            // Arrange
            // Action 
            Result result = Result.Ok()
                .Ensure(true, PredicateFailure);

            // Asserts
            Assert.IsTrue(result.IsSuccess);
            Assert.IsFalse(result.IsFailure);
            Assert.AreEqual(result.Error, string.Empty);
        }

        /// <summary>
        /// Test the action does not occur on a preceeding failure,
        /// When replacing one type with another.
        /// </summary>
        [Test]
        public void TestEnsureOnBoolTestFailsOnIncomingFailureWithLogging()
        {
            // Arrange
            ICollection<string> errors = new Collection<string>();

            Result<int> result = Result.Fail<int>(FailedText)
            // Action 
                .Ensure(false, PredicateFailure, out bool StoredPredicateResult, errors);

            // Asserts 
            Assert.IsTrue(result.IsFailure);
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual(result.Error, FailedText);
            Assert.IsFalse(StoredPredicateResult);
            Assert.AreEqual(errors.Count, 0);
        }

        /// <summary>
        /// Test the Action does occur on a successful Result,
        /// When converting from one type to another.
        /// </summary>
        [TestCase(1)]
        public void TestEnsureFailsOnBoolTestFailureWithLogging(int i)
        {
            // Arrange
            ICollection<string> errors = new Collection<string>();

            // Action 
            Result<int> result = Result.Ok<int>(i)
                .Ensure(false, PredicateFailure, out bool StoredPredicateResult, errors);

            // Asserts
            Assert.IsTrue(result.IsFailure);
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual(result.Error, PredicateFailure);
            Assert.IsFalse(StoredPredicateResult);
            Assert.AreEqual(errors.Count, 1);
            Assert.AreEqual(errors.Last(), PredicateFailure);
        }

        /// <summary>
        /// Test the Action does occur on a successful Result,
        /// When converting from one type to another.
        /// </summary>
        [TestCase(1)]
        public void TestEnsurePassesOnResultOnBoolTestSuccessWithLogging(int i)
        {
            // Arrange
            ICollection<string> errors = new Collection<string>();

            // Action 
            Result<int> result = Result.Ok(i)
                .Ensure(true, PredicateFailure, out bool StoredPredicateResult, errors);

            // Asserts
            Assert.IsTrue(result.IsSuccess);
            Assert.IsFalse(result.IsFailure);
            Assert.AreEqual(result.Error, string.Empty);
            Assert.AreEqual(result.Value, i);
            Assert.IsTrue(StoredPredicateResult);
            Assert.AreEqual(errors.Count, 0);
        }

        private bool CompareObjects(object o1, object o2)
        {
            if (o1 == null)
            {
                return o2 == null;
            }
            if (o2 == null)
                return false;
            return o1.Equals(o2);
        }

        private const string ArrayNull = "Array {0} of the 2 compared arrays[1,2] is null. The arrays are not equal";

        private const string WrongLength = "Wrong length array1 is {0} elements, array2 is {1}";

        private const string DifferentValue = "The arrays differ at offset: {0}. {1} != {2}";

        [TestCase(new object[] { 1, 2, 3 }, new object[] { 1, 2, 3 })]
        [TestCase(new object[] { "1", "2", "3" }, new object[] { "1", "2", "3" })]
        [TestCase(new object[] { '1', '2', '3' }, new object[] { '1', '2', '3' })]
        [TestCase(new object[] { 1, '2', "3" }, new object[] { 1, '2', "3" })]
        [TestCase(null, null)]
        public void TestEnsureArrayComparisonSuccess(object[] array1, object[] array2)
        {
            try
            {
                // Arrange
                Result result = Result.Ok()
                    //Act
                    .Ensure(array1,
                            array2,
                            CompareObjects,
                            (array) => string.Format(CultureInfo.InvariantCulture, ArrayNull, array),
                            (l1, l2) =>
                            string.Format(CultureInfo.InvariantCulture, WrongLength,
                                          l1,
                                          l2),
                            (index, o1, o2) => string.Format(CultureInfo.InvariantCulture, DifferentValue, index, o1.ToString(), o2.ToString()));
                // Assert
                Assert.IsTrue(result.IsSuccess);
            }
            catch (ArgumentNullException ex)
            {
                Assert.Fail($"Bad call to String.Format in test: {ex.GetType().Name}: {ex.Message}.");
            }
            catch (FormatException ex)
            {
                Assert.Fail($"Bad call to String.Format in test: {ex.GetType().Name}: {ex.Message}.");
            }
            catch (Exception ex)
            {
                Assert.Fail($"Unexpected exception thrown {ex.GetType().Name}: {ex.Message}.");
            }
        }

        [TestCase(new object[] { 1, 2, 3 }, new object[] { 3, 1, 2 })]
        public void TestEnsureArrayComparisonDifferenceFailure(object[] array1, object[] array2)
        {
            try
            {
                // Arrange
                Result result = Result.Ok()
                //Act
                    .Ensure(array1,
                            array2,
                            CompareObjects,
                            (array) => string.Format(CultureInfo.InvariantCulture, ArrayNull, array),
                            (l1, l2) =>
                            string.Format(CultureInfo.InvariantCulture, WrongLength,
                                          l1,
                                          l2),
                            (index, o1, o2) => DifferentValue);
                // Assert
                Assert.IsTrue(result.IsFailure);
                Assert.AreEqual(result.Error, DifferentValue);
            }
            catch (ArgumentNullException ex)
            {
                Assert.Fail($"Bad call to String.Format in test: {ex.GetType().Name}: {ex.Message}.");
            }
            catch (FormatException ex)
            {
                Assert.Fail($"Bad call to String.Format in test: {ex.GetType().Name}: {ex.Message}.");
            }
            catch (Exception ex)
            {
                Assert.Fail($"Unexpected exception thrown {ex.GetType().Name}: {ex.Message}.");
            }
        }

        [TestCase(new object[] { 1, 2, 3 }, new object[] { 1, 2 })]
        [TestCase(new object[] { 1 }, new object[] { 1, 2 })]
        public void TestEnsureArrayComparisonLengthFailure(object[] array1, object[] array2)
        {
            try
            {
                // Arrange
                Result result = Result.Ok()
                //Act
                .Ensure(array1,
                        array2,
                        CompareObjects,
                        (array) => string.Format(CultureInfo.InvariantCulture, ArrayNull, array),
                        (l1, l2) =>
                        string.Format(CultureInfo.InvariantCulture, WrongLength,
                                      l1,
                                      l2),
                        (index, o1, o2) => string.Format(CultureInfo.InvariantCulture, DifferentValue, index, o1.ToString(), o2.ToString()));
                // Assert
                Assert.IsTrue(result.IsFailure);
                Assert.AreEqual(result.Error,
                                string.Format(CultureInfo.InvariantCulture, WrongLength,
                                              array1 == null ? 0 : array1.Length,
                                              array2 == null ? 0 : array2.Length));
            }
            catch (ArgumentNullException ex)
            {
                Assert.Fail($"Bad call to String.Format in test: {ex.GetType().Name}: {ex.Message}.");
            }
            catch (FormatException ex)
            {
                Assert.Fail($"Bad call to String.Format in test: {ex.GetType().Name}: {ex.Message}.");
            }
            catch (Exception ex)
            {
                Assert.Fail($"Unexpected exception thrown {ex.GetType().Name}: {ex.Message}.");
            }
        }

        [TestCase(null, new object[] { })]
        [TestCase(null, new object[] { 1,2,3})]
        public void TestEnsureArrayComparisonNull1Failure(object[] array1, object[] array2)
        {
            try
            {
                // Arrange
                Result result = Result.Ok()
                //Act
                    .Ensure(array1,
                            array2,
                            CompareObjects,
                            (array) => string.Format(CultureInfo.InvariantCulture, ArrayNull, array),
                            (l1, l2) =>
                            string.Format(CultureInfo.InvariantCulture, WrongLength,
                                          l1,
                                          l2),
                            (index, o1, o2) => string.Format(CultureInfo.InvariantCulture, DifferentValue, index, o1.ToString(), o2.ToString()));
                // Assert
                Assert.IsTrue(result.IsFailure);
                Assert.AreEqual(result.Error,
                                string.Format(CultureInfo.InvariantCulture, ArrayNull, 1));
            }
            catch (ArgumentNullException ex)
            {
                Assert.Fail($"Bad call to String.Format in test: {ex.GetType().Name}: {ex.Message}.");
            }
            catch (FormatException ex)
            {
                Assert.Fail($"Bad call to String.Format in test: {ex.GetType().Name}: {ex.Message}.");
            }
            catch (Exception ex)
            {
                Assert.Fail($"Unexpected exception thrown {ex.GetType().Name}: {ex.Message}.");
            }
        }

        [TestCase(new object[] { }, null)]
        [TestCase(new object[] { 1, 2, 3 }, null)]
        public void TestEnsureArrayComparisonNull2Failure(object[] array1, object[] array2)
        {
            try
            {

                // Arrange
                Result result = Result.Ok()
                //Act
                    .Ensure(array1,
                            array2,
                            CompareObjects,
                            (array) => string.Format(CultureInfo.InvariantCulture, ArrayNull, array),
                            (l1, l2) =>
                            string.Format(CultureInfo.InvariantCulture, WrongLength,
                                          l1,
                                          l2),
                            (index, o1, o2) => string.Format(CultureInfo.InvariantCulture, DifferentValue, index, o1.ToString(), o2.ToString()));
                // Assert
                Assert.IsTrue(result.IsFailure);
                Assert.AreEqual(result.Error,
                                string.Format(CultureInfo.InvariantCulture, ArrayNull, 2));
            }
            catch (ArgumentNullException ex)
            {
                Assert.Fail($"Bad call to String.Format in test: {ex.GetType().Name}: {ex.Message}.");
            }
            catch (FormatException ex)
            {
                Assert.Fail($"Bad call to String.Format in test: {ex.GetType().Name}: {ex.Message}.");
            }
            catch (Exception ex)
            {
                Assert.Fail($"Unexpected exception thrown {ex.GetType().Name}: {ex.Message}.");
            }
        }

        [TestCase("")]
        [TestCase("Hello world")]
        [TestCase(null)]
        public void TestOnSuccessActionOnValue(string input)
        {
            const string errorString = "Invalid string";
            Result<string> result = Result.Initialize(input, errorString)
                .OnSuccess(s => { Assert.AreEqual(s, input); })
                .OnSuccess(s => { Assert.IsTrue(s != null); })
                .OnFailure(error => Assert.AreEqual(error, errorString));
        }

        /// <summary>
        /// Test the action does not occur on a preceeding failure
        /// </summary>
        [Test]
        public void TestOnSuccessActionOnValueFail()
        {
            string errorMessage = "Error!";
            // Arrange
            Result<string> result = Result.Fail<string>(errorMessage)

            // Action Assert
                .OnSuccess((value) => { Assert.Fail(); });

            // Assert on Action
            Assert.IsTrue(result.IsFailure);
            Assert.IsFalse(result.IsSuccess);
        }

        /// <summary>
        /// Test the action does not occur on a preceeding failure
        /// </summary>
        [TestCase("")]
        [TestCase("Hello world")]
        public void TestOnSuccessActionOnValueSuccess(string input)
        {
            // Arrange
            Result<string> result = Result.Ok<string>(input)

            // Action Assert
                .OnSuccess((value) => Assert.AreEqual(value, input));

            // Assert on Action
            Assert.IsTrue(result.IsSuccess);
            Assert.IsFalse(result.IsFailure);
        }

        /// <summary>
        /// Test the action does not occur on a preceeding failure
        /// </summary>
        [Test]
        public void TestOnSuccessFuncOnValueFail()
        {
            string errorMessage = "Error!";
            // Arrange
            Result<string> result = Result.Fail<string>(errorMessage)

            // Action Assert
                .OnSuccess<string>((value) => 
                {
                    Assert.Fail();
                    return value;
                });

                    // Assert on Action
            Assert.IsTrue(result.IsFailure);
            Assert.IsFalse(result.IsSuccess);
        }
        private static string Reverse(string s)
        {
            char[] ca = s.ToCharArray();
            Array.Reverse(ca);
            return new string(ca);
        }


        /// <summary>
        /// Test the action does not occur on a preceeding failure
        /// </summary>
        [TestCase("")]
        [TestCase("Hello world")]
        public void TestOnSuccessFuncOnValueSuccess(string input)
        {
            // Arrange
            Result<string> result = Result.Ok<string>(input)

            // Action Assert
                .OnSuccess<string>((value) =>
                {
                    return Reverse(value);
                })
            // Assert on Action
                .OnSuccess<string>((value) =>
                {
                    Assert.AreEqual(value, Reverse(input));
                });
            Assert.IsTrue(result.IsSuccess);
            Assert.IsFalse(result.IsFailure);
        }

        /// <summary>
        /// Test the action does not occur on a preceeding failure
        /// </summary>
        [TestCase("")]
        [TestCase("Hello world")]
        public void TestOnSuccessResultFuncOnValueFail(string input)
        {
            string errorMessage = "Error!";
            // Arrange
            Result<string> result = Result.Fail<string>(errorMessage)

            // Action Assert
                .OnSuccess<string>((value) =>
                {
                    Assert.Fail();
                    return Result.Ok(input);
                });

            // Assert on Action
            Assert.IsTrue(result.IsFailure);
            Assert.IsFalse(result.IsSuccess);
        }

        /// <summary>
        /// Test the action does not occur on a preceeding failure
        /// </summary>
        [TestCase("")]
        [TestCase("Hello world")]
        public void TestOnSuccessResultFuncOnValueSuccessSucceeds(string input)
        {
            // Arrange
            Result<string> result = Result.Ok<string>(input)

            // Action Assert
                .OnSuccess<string>((value) =>
                {
                    Assert.AreEqual(value, input);
                    return Result.Ok(Reverse(value));
                })
            // Assert on Action
                .OnSuccess<string>((value) =>
                {
                    Assert.AreEqual(value, Reverse(input));
                });
            Assert.IsTrue(result.IsSuccess);
            Assert.IsFalse(result.IsFailure);
        }


        /// <summary>
        /// Test a failed function on the Value is recorded as a failure.
        /// </summary>
        [TestCase("")]
        [TestCase("Hello world")]
        public void TestOnSuccessResultFuncOnValueSuccessFails (string input)
        {
            string errorString = "Error!";
            // Arrange
            Result<string> result = Result.Ok<string>(input)

            // Action Assert
                .OnSuccess<string>((value) =>
                {
                    Assert.AreEqual(value, input);
                    return Result.Fail<string>(errorString);
                })
            // Assert on Action
                .OnSuccess<string>((value) =>
                {
                    Assert.Fail("Failed to record failure");
                });
            Assert.IsTrue(result.IsFailure);
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual(result.Error, errorString);
        }

        [TestCase("Any old password")]
        [TestCase("Yet another silly password")]
        [TestCase("what sort of password do you call this")]

        [TestCase("339771A1544CC5BD5E6B45EB2AA6F841")]
        [TestCase("5AC9C51BDEC2D7B123333D75AB2EA2D8")]
        [TestCase("2D29BC2639805F404C886445BA5265FA")]
        [TestCase("E8C7D3714AF9738E8CBB5113CCD09E12")]
        public void TestOnFailureGenerateDefault(string clearTextPassword)
        {
            try
            {
                string s = Result.Ok("")
                        .Ensure(clearTextPassword.Length == 32, "Password is the wrong length.")
                        .Ensure(new Regex("^[0-9A-Fa-f]{32}$").IsMatch(clearTextPassword), "Password is not hexidecimal.")
                        .OnSuccess<string>((v) => v = clearTextPassword)
                        .OnFailureGeneratedDefault<string>(() =>
                        {
                            // we are not dealing with a standard AES password so generate one.
                            byte[] hash;
                            try
                            {
#pragma warning disable CA5351 // Do Not Use Broken Cryptographic Algorithms
                                // MD5 is used to generate a 128 bit key not for encryption.
                                hash = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(clearTextPassword));
#pragma warning restore CA5351 // Do Not Use Broken Cryptographic Algorithms
                            }
                            catch (TargetInvocationException ex)
                            {
                                Assert.Fail($"Bad call to MD5.Create() in test: {ex.GetType().Name}: {ex.Message}.");
                                return Result.Fail<string>(ex);
                            }
                            catch (ArgumentNullException ex)
                            {
                                Assert.Fail($"Bad call to MD5.Create().ComputeHash() in test: {ex.GetType().Name}: {ex.Message}.");
                                return Result.Fail<string>(ex);
                            }
                            catch (ObjectDisposedException ex)
                            {
                                Assert.Fail($"Theoretically impossible bad call to MD5.Create().ComputeHash() in test: {ex.GetType().Name}: {ex.Message}.");
                                return Result.Fail<string>(ex);
                            }
                            var key = new byte[32];
                            StringBuilder hex = new();
                            foreach (byte b in hash)
                                hex.AppendFormat(CultureInfo.InvariantCulture, "{0:X2}", b);
                            return Result.Ok(hex.ToString());
                        }).Value;
                Assert.AreEqual(s.Length, 32);
                Assert.IsTrue(new Regex("^[0-9A-Fa-f]{32}$").IsMatch(s));
            }
            catch (ArgumentNullException ex)
            {
                Assert.Fail($"Bad call to StringBuilder.AppendFormat in test: {ex.GetType().Name}: {ex.Message}.");
            }
            catch (FormatException ex)
            {
                Assert.Fail($"Bad call to StringBuilder.AppendFormat in test: {ex.GetType().Name}: {ex.Message}.");
            }
            catch (Exception ex)
            {
                Assert.Fail($"Unexpected exception thrown {ex.GetType().Name}: {ex.Message}.");
            }
        }

        [Test]
        public void TestOnExceptionExceptionLoggedCorrectly()
        {
            const string Message = "Message";
            Exception exception = new Exception(Message);
            Result r = Result.Fail(exception);
            Assert.IsTrue(r.Exception.HasValue);
            Assert.AreEqual(r.Exception.Value, exception);
            Assert.AreEqual(r.Exception.Value.Message, Message);
        }

        [Test]
        public void TestOnResultSwitchExceptionPassedCorrectly()
        {
            const string Text = "Error Message";
            Result<string> result = Result.Fail<string>(new Exception(Text));
            Result<bool> finalResult = result.OnSuccess((v) => v == Text);
            Assert.IsTrue(finalResult.Exception.HasValue);
            Assert.AreEqual(finalResult.Exception.Value.Message, Text);
        }

        [Test]
        public void TestOnResultSwitchErrorTextPassedCorrectly()
        {
            const string Text = "Error Message";
            Result<string> result = Result.Fail<string>(Text);
            Result<bool> finalResult = result.OnSuccess((e) => e == Text);
            Assert.IsTrue(finalResult.Exception.HasNoValue);
            Assert.AreEqual(finalResult.Error, Text);
        }

        [Test]
        public void TestOnResultSwitchNotesArePassedCorrectlyOnSuccess()
        {
            const string Text = "Error Message";
            Result<string> result = Result.Ok(Text);
            result.AddNote(Text);
            Result<bool> finalResult = result.OnSuccess((v) => v == Text);
            Assert.IsTrue(finalResult.HasNotes);
            Assert.AreEqual(finalResult.Notes.First(), Text);
        }

        [Test]
        public void TestOnResultSwitchNotesArePassedCorrectlyOnError()
        {
            const string Text = "Error Message";
            Result<string> result = Result.Fail<string>(Text);
            result.AddNote(Text);
            Result<bool> finalResult = result.OnSuccess((v) => v == Text);
            Assert.IsTrue(finalResult.HasNotes);
            Assert.AreEqual(finalResult.Notes.First(), Text);
        }

        [Test]
        public void TestOnResultSwitchNotesArePassedCorrectlyOnException()
        {
            const string Text = "Error Message";
            Result<string> result = Result.Fail<string>(new Exception(Text));
            result.AddNote(Text);
            Result<bool> finalResult = result.OnSuccess((v) => v == Text);
            Assert.IsTrue(finalResult.HasNotes);
            Assert.AreEqual(finalResult.Notes.First(), Text);
        }
    }
}