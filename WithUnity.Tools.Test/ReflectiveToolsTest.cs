/* Copyright 2015-2020 Robin Murison

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. */
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace WithUnity.Tools.Tests
{
    using Tools;
    [TestFixture]
    public class ReflectiveToolsTest
    {
        [Test]
        public void TestCurrentMethod ()
        {
            // Arrange
            string expectedMethodName = "TestCurrentMethod";

            // Act
            string actualResult = ReflectiveTools.CurrentMethod();

            //Assert
            Assert.AreEqual(expectedMethodName, actualResult);
        }

        [Test]
        public void TestUnsafeCurrentMethodSuccess ()
        {
            // Arrange
            string expectedMethodName = "TestUnsafeCurrentMethodSuccess";

            // Act
            string actualResult = ReflectiveTools.UnsafeCurrentMethod();

            //Assert
            Assert.AreEqual(expectedMethodName, actualResult);
        }

        [Test]
        public void TestUnsafeCurrentMethodFailure()
        {
            // Arrange
            string expectedMethodName = "TestUnsafeCurrentMethodFailure";

            // Act
            string actualResult = ReflectiveTools.UnsafeCurrentMethod("TestUnsafeCurrentMethodSuccess");

            //Assert
            Assert.AreNotEqual(expectedMethodName, actualResult);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]

        private static string CalledMethod()
        {
            return ReflectiveTools.CallingMethod(2);
        }

        [Test]
        public void TestCallingMethod ()
        {
            // Arrange
            string expectedMethodName = "TestCallingMethod";

            // Act
            string actualResult = CalledMethod();

            //Assert
            Assert.AreEqual(expectedMethodName, actualResult);
        }
    }

    /// <summary>
    /// Test class for checking Missing assemblies
    /// </summary>
    public class MissingAssembly
    {
        public MissingAssembly(string missingAssemblyName, string missingAssemblyNameParent)
        {
            MissingAssemblyName = missingAssemblyName;
            MissingAssemblyNameParent = missingAssemblyNameParent;
        }

        public string MissingAssemblyName { get; set; }
        public string MissingAssemblyNameParent { get; set; }
    }
}