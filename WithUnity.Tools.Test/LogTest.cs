/* Copyright 2015-2021 Robin Murison

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. */
using System;
using NUnit.Framework;

namespace WithUnity.Tools.Tests
{ 
    [TestFixture]
    public class LogTest
    {
        private readonly object _LockObject = new object();

        [Test]
        public void TestExcludeVerbose()
        {
            //Setup
            Log.Initialise(Log.Level.Information, new Log.Sink[] { Log.Sink.File }, false);

            //Action
            Log.Verbose("Verbose Message");

            //result
            // The test result is a blank output
        }

        [Test]
        public void TestIncludeVerbose()
        {
            //Setup
            Log.Initialise(Log.Level.Verbose, new Log.Sink[] { Log.Sink.File }, false);

            //Action
            Log.Verbose("Verbose Message");

            //result
            // The test result is a blank output
        }

        [Test]
        public void TestIncludeWarning()
        {
            //Setup
            Log.Initialise(Log.Level.Warning, new Log.Sink[] { Log.Sink.File }, false);

            //Action
            Log.Warning(new DataMisalignedException("Warning Message"));

            //result
            // The test result is a blank output
        }

        [Test]
        public void FileOutputTest()
        {
            lock (_LockObject)
            {
                //Setup
                Log.Initialise(Log.Level.Warning, new Log.Sink[] { Log.Sink.File }, false);

                //Action
                Log.Error(new DataMisalignedException("Error Message"));

                //result
                // The test result is in a file
            }
        }
    }
}
