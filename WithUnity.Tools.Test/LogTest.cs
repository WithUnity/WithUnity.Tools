/* Copyright 2015-2021 Robin Murison

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. */
using System;
using System.IO;
using NUnit.Framework;


namespace WithUnity.Tools.Tests
{
    using WithUnity.Tools;

    [TestFixture]
    public class LogTest
    {
        private string _TestLogFileName = $"{Environment.GetEnvironmentVariable("TEMP")}\\WithUnity.Tools.Tests.log";

        // Straight message
        const string message = "Message logged?";
        
        /// <summary>
        /// Check that more verbose messages are excludedwhen stricter logging option is chosen
        /// </summary>
        [TestCase(Log.Level.Debug, Log.Level.Verbose)]
        [TestCase(Log.Level.Information, Log.Level.Verbose)]
        [TestCase(Log.Level.Warning, Log.Level.Verbose)]
        [TestCase(Log.Level.Error, Log.Level.Verbose)]
        [TestCase(Log.Level.UnknownState, Log.Level.Verbose)]
        [TestCase(Log.Level.Information, Log.Level.Debug)]
        [TestCase(Log.Level.Warning, Log.Level.Debug)]
        [TestCase(Log.Level.Error, Log.Level.Debug)]
        [TestCase(Log.Level.UnknownState, Log.Level.Debug)]
        [TestCase(Log.Level.Warning, Log.Level.Information)]
        [TestCase(Log.Level.Error, Log.Level.Information)]
        [TestCase(Log.Level.UnknownState, Log.Level.Information)]
        [TestCase(Log.Level.Error, Log.Level.Warning)]
        [TestCase(Log.Level.UnknownState, Log.Level.Warning)]
        [TestCase(Log.Level.UnknownState, Log.Level.Error)]
        public void TestExcludedMessages(Log.Level loggingLevel, Log.Level omittedLevel)
        {
            //Setup
            File.Delete(_TestLogFileName);
            Log.Initialise(loggingLevel, new Log.Sink[] { Log.Sink.File }, logFileName: _TestLogFileName);

            //Action
            Log.LevelLog(omittedLevel, message);

            //result
            string text = File.ReadAllText(Log.LogFileName);
            Assert.AreEqual(expected: 0, text.Length);
        }

        /// <summary>
        /// Check that less verbose options are included when  a more verbose option is seleceted
        /// </summary>
        /// <param name="loggingLevel"></param>
        /// <param name="includedLevel"></param>
        [TestCase(Log.Level.UnknownState, Log.Level.UnknownState)]
        [TestCase(Log.Level.Error, Log.Level.UnknownState)]
        [TestCase(Log.Level.Error, Log.Level.Error)]
        [TestCase(Log.Level.Warning, Log.Level.UnknownState)]
        [TestCase(Log.Level.Warning, Log.Level.Error)]
        [TestCase(Log.Level.Warning, Log.Level.Warning)]
        [TestCase(Log.Level.Information, Log.Level.UnknownState)]
        [TestCase(Log.Level.Information, Log.Level.Error)]
        [TestCase(Log.Level.Information, Log.Level.Warning)]
        [TestCase(Log.Level.Information, Log.Level.Information)]
        [TestCase(Log.Level.Debug, Log.Level.UnknownState)]
        [TestCase(Log.Level.Debug, Log.Level.Error)]
        [TestCase(Log.Level.Debug, Log.Level.Warning)]
        [TestCase(Log.Level.Debug, Log.Level.Information)]
        [TestCase(Log.Level.Debug, Log.Level.Debug)]
        [TestCase(Log.Level.Verbose, Log.Level.UnknownState)]
        [TestCase(Log.Level.Verbose, Log.Level.Error)]
        [TestCase(Log.Level.Verbose, Log.Level.Warning)]
        [TestCase(Log.Level.Verbose, Log.Level.Information)]
        [TestCase(Log.Level.Verbose, Log.Level.Debug)]
        [TestCase(Log.Level.Verbose, Log.Level.Verbose)]
        public void TestIncludeMessage(Log.Level loggingLevel, Log.Level includedLevel)
        {
            //Setup
            File.Delete(_TestLogFileName);
            Log.Initialise(loggingLevel, new Log.Sink[] { Log.Sink.File }, logFileName: _TestLogFileName);


            //Action
            Log.LevelLog(includedLevel, message);

            //result
            string text = File.ReadAllText(Log.LogFileName);
            Assert.IsTrue(text.Contains(Log.LevelLabels[includedLevel]));
            Assert.IsTrue(text.Contains(message));
        }

        // Testing the Formated error message functions
        const string format = "Message {0}";
        static object[] data = (new object[] { "Data" });
        string expectedFormattedData = string.Format(format, data);

        /// <summary>
        /// Check that more verbose messages are excludedwhen stricter logging option is chosen
        /// </summary>
        [TestCase(Log.Level.Debug, Log.Level.Verbose)]
        [TestCase(Log.Level.Information, Log.Level.Verbose)]
        [TestCase(Log.Level.Warning, Log.Level.Verbose)]
        [TestCase(Log.Level.Error, Log.Level.Verbose)]
        [TestCase(Log.Level.UnknownState, Log.Level.Verbose)]
        [TestCase(Log.Level.Information, Log.Level.Debug)]
        [TestCase(Log.Level.Warning, Log.Level.Debug)]
        [TestCase(Log.Level.Error, Log.Level.Debug)]
        [TestCase(Log.Level.UnknownState, Log.Level.Debug)]
        [TestCase(Log.Level.Warning, Log.Level.Information)]
        [TestCase(Log.Level.Error, Log.Level.Information)]
        [TestCase(Log.Level.UnknownState, Log.Level.Information)]
        [TestCase(Log.Level.Error, Log.Level.Warning)]
        [TestCase(Log.Level.UnknownState, Log.Level.Warning)]
        [TestCase(Log.Level.UnknownState, Log.Level.Error)]
        public void TestExcludedFormattedMessages(Log.Level loggingLevel, Log.Level omittedLevel)
        {
            //Setup
            File.Delete(_TestLogFileName);
            Log.Initialise(loggingLevel, new Log.Sink[] { Log.Sink.File }, logFileName: _TestLogFileName);

            //Action
            Log.LevelLog(omittedLevel, format, data);

            //result
            string text = File.ReadAllText(Log.LogFileName);
            Assert.AreEqual(expected: 0, text.Length);
        }

        /// <summary>
        /// Check that less verbose options are included when  a more verbose option is seleceted
        /// </summary>
        /// <param name="loggingLevel"></param>
        /// <param name="includedLevel"></param>
        [TestCase(Log.Level.UnknownState, Log.Level.UnknownState)]
        [TestCase(Log.Level.Error, Log.Level.UnknownState)]
        [TestCase(Log.Level.Error, Log.Level.Error)]
        [TestCase(Log.Level.Warning, Log.Level.UnknownState)]
        [TestCase(Log.Level.Warning, Log.Level.Error)]
        [TestCase(Log.Level.Warning, Log.Level.Warning)]
        [TestCase(Log.Level.Information, Log.Level.UnknownState)]
        [TestCase(Log.Level.Information, Log.Level.Error)]
        [TestCase(Log.Level.Information, Log.Level.Warning)]
        [TestCase(Log.Level.Information, Log.Level.Information)]
        [TestCase(Log.Level.Debug, Log.Level.UnknownState)]
        [TestCase(Log.Level.Debug, Log.Level.Error)]
        [TestCase(Log.Level.Debug, Log.Level.Warning)]
        [TestCase(Log.Level.Debug, Log.Level.Information)]
        [TestCase(Log.Level.Debug, Log.Level.Debug)]
        [TestCase(Log.Level.Verbose, Log.Level.UnknownState)]
        [TestCase(Log.Level.Verbose, Log.Level.Error)]
        [TestCase(Log.Level.Verbose, Log.Level.Warning)]
        [TestCase(Log.Level.Verbose, Log.Level.Information)]
        [TestCase(Log.Level.Verbose, Log.Level.Debug)]
        [TestCase(Log.Level.Verbose, Log.Level.Verbose)]
        public void TestIncludeFormattedMessage(Log.Level loggingLevel, Log.Level includedLevel)
        {
            //Setup
            File.Delete(_TestLogFileName);
            Log.Initialise(loggingLevel, new Log.Sink[] { Log.Sink.File }, logFileName: _TestLogFileName);


            //Action
            Log.LevelLog(includedLevel, format, data);

            //result
            string text = File.ReadAllText(Log.LogFileName);
            Assert.IsTrue(text.Contains(Log.LevelLabels[includedLevel]));
            Assert.IsTrue(text.Contains(expectedFormattedData));
        }

        // Testing the Exception handling functions
        Exception logException = new InvalidCastException();

        /// <summary>
        /// Check that more verbose messages are excludedwhen stricter logging option is chosen
        /// </summary>
        [TestCase(Log.Level.UnknownState, Log.Level.Error)]
        public void TestExcludedException(Log.Level loggingLevel, Log.Level omittedLevel)
        {
            //Setup
            File.Delete(_TestLogFileName);
            Log.Initialise(loggingLevel, new Log.Sink[] { Log.Sink.File }, logFileName: _TestLogFileName);

            //Action
            Log.LevelLog(omittedLevel, logException);

            //result
            string text = File.ReadAllText(Log.LogFileName);
            Assert.AreEqual(expected: 0, text.Length);
        }

        /// <summary>
        /// Check that less verbose options are included when  a more verbose option is seleceted
        /// </summary>
        /// <param name="loggingLevel"></param>
        /// <param name="includedLevel"></param>
        [TestCase(Log.Level.Error, Log.Level.Error)]
        [TestCase(Log.Level.Warning, Log.Level.Error)]
        [TestCase(Log.Level.Information, Log.Level.Error)]
        [TestCase(Log.Level.Debug, Log.Level.Error)]
        [TestCase(Log.Level.Verbose, Log.Level.Error)]
        public void TestIncludedException(Log.Level loggingLevel, Log.Level includedLevel)
        {
            //Setup
            File.Delete(_TestLogFileName);
            Log.Initialise(loggingLevel, new Log.Sink[] { Log.Sink.File }, logFileName: _TestLogFileName);


            //Action
            Log.LevelLog(includedLevel, logException);

            //result
            string text = File.ReadAllText(Log.LogFileName);
            Assert.IsTrue(text.Contains(Log.LevelLabels[includedLevel]));
            Assert.IsTrue(text.Contains("InvalidCastException"));
        }

        [Test]
        public void UnkownStateIsLevelLogUnknownState()
        {
            //Setup
            File.Delete(_TestLogFileName);
            Log.Initialise(Log.Level.UnknownState, new Log.Sink[] { Log.Sink.File }, logFileName: _TestLogFileName);


            //Action
            Log.UnknownState(message);

            //result
            string text = File.ReadAllText(Log.LogFileName);
            Assert.IsTrue(text.Contains(Log.LevelLabels[Log.Level.UnknownState]));
            Assert.IsTrue(text.Contains(message));
        }

        [Test]
        public void ErrorIsLevelLogError()
        {
            //Setup
            File.Delete(_TestLogFileName);
            Log.Initialise(Log.Level.Error, new Log.Sink[] { Log.Sink.File }, logFileName: _TestLogFileName);


            //Action
            Log.Error(message);

            //result
            string text = File.ReadAllText(Log.LogFileName);
            Assert.IsTrue(text.Contains(Log.LevelLabels[Log.Level.Error]));
            Assert.IsTrue(text.Contains(message));
        }

        [Test]
        public void WarningIsLevelLogWarning()
        {
            //Setup
            File.Delete(_TestLogFileName);
            Log.Initialise(Log.Level.Warning, new Log.Sink[] { Log.Sink.File }, logFileName: _TestLogFileName);


            //Action
            Log.Warning(message);

            //result
            string text = File.ReadAllText(Log.LogFileName);
            Assert.IsTrue(text.Contains(Log.LevelLabels[Log.Level.Warning]));
            Assert.IsTrue(text.Contains(message));
        }

        [Test]
        public void InformationIsLevelLogInformation()
        {
            //Setup
            File.Delete(_TestLogFileName);
            Log.Initialise(Log.Level.Information, new Log.Sink[] { Log.Sink.File }, logFileName: _TestLogFileName);


            //Action
            Log.Information(message);

            //result
            string text = File.ReadAllText(Log.LogFileName);
            Assert.IsTrue(text.Contains(Log.LevelLabels[Log.Level.Information]));
            Assert.IsTrue(text.Contains(message));
        }

        [Test]
        public void DebugIsLevelLogDebug()
        {
            //Setup
            File.Delete(_TestLogFileName);
            Log.Initialise(Log.Level.Debug, new Log.Sink[] { Log.Sink.File }, logFileName: _TestLogFileName);


            //Action
            Log.Debug(message);

            //result
            string text = File.ReadAllText(Log.LogFileName);
            Assert.IsTrue(text.Contains(Log.LevelLabels[Log.Level.Debug]));
            Assert.IsTrue(text.Contains(message));
        }

        [Test]
        public void VerboseIsLevelLogVerbose()
        {
            //Setup
            File.Delete(_TestLogFileName);
            Log.Initialise(Log.Level.Verbose, new Log.Sink[] { Log.Sink.File }, logFileName: _TestLogFileName);


            //Action
            Log.Verbose(message);

            //result
            string text = File.ReadAllText(Log.LogFileName);
            Assert.IsTrue(text.Contains(Log.LevelLabels[Log.Level.Verbose]));
            Assert.IsTrue(text.Contains(message));
        }
    }
}
