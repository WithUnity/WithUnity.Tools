/* Copyright 2015-2021 Robin Murison

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. */
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Diagnostics;
using System.Threading;

namespace WithUnity.Tools
{
    /// <summary>
    /// A class for generic logging
    /// </summary>
    public static class Log // : IAspect
    {
        /// <summary>
        /// The level of requested logging
        /// Both for reading and writing logs.
        /// </summary>
        public enum Level : int
        {
            /// <summary>
            /// All logging
            /// </summary>
            Verbose = 1,
            /// <summary>
            /// Debugging logs
            /// </summary>
            Debug = 2,
            /// <summary>
            /// Information logging messages
            /// </summary>
            Information = 4,
            /// <summary>
            /// Warning Logging
            /// </summary>
            Warning = 8,
            /// <summary>
            /// Error Logs
            /// </summary>
            Error = 16,
            /// <summary>
            /// This is for pieces of code that should never run.
            /// This extreme level of error can only occur because of bugs. e.g. a switch statement that already has a case for all the possible values of the Enum
            /// should keep a separate 'default:' case too. to log unknown states and normally should never run. 
            /// This is to protect code from the situation where someone adds a new value to the enum without knowing about all the switch statements.
            /// You cannot hide UnknownState logging. You may prefer to throw an exception but that may increase the problem in production rather than containing the problem.
            /// </summary>
            /// <remarks>Unkown states are always logged to Visual Studio Debug as well as the Sinks requested and also throw exceptions in Debug mode</remarks>
            UnknownState = 32
        }

        /// <summary>
        /// Places you can log your errors to
        /// </summary>
        public enum Sink : int
        {
            /// <summary>
            /// The command line
            /// </summary>
            Console = 1,
            /// <summary>
            /// To the Visual Studio Debug output window
            /// </summary>
            Debug = 2,
            /// <summary>
            /// To a Log file in the %TEMP% directory using the calling assembly's name + ".log"
            /// </summary>
            File = 4/*,
            /// <summary>
            /// To the Windows event log
            /// </summary>
            EventLog = 8*/
        }

        private static bool Initialised = false;

        /// <summary>
        /// Used as a Level ToString() to give strings the same length
        /// to keep the log alignment the same so that it is easier to read.
        /// </summary>
        public readonly static IDictionary<Level, string> LevelLabels = new Dictionary<Level, string>
        {
            { Level.Verbose,      "Verbose" },
            { Level.Debug,        "Debug  " },
            { Level.Information,  "Inform " },
            { Level.Warning,      "Warning" },
            { Level.Error,        "Error  " },
            { Level.UnknownState, "Unknown" }
        };
        private static bool Utc { get; set; }

        private static bool TimeOffset { get; set; }

        /// <summary>
        /// The Entry AssemblyName
        /// </summary>
        public static string EntryAssemblyName { get; private set; }

        /// <summary>
        /// The File name used when logging to a file.
        /// </summary>
        public static string LogFileName { get; private set; }

        private static Level LoggingLevel { get; set; }

        private static void OutputToDebug(string text)
        {
            System.Diagnostics.Debug.WriteLine("text");
        }

        private static StreamWriter FileStream;

         private static void WriteToFile(string text)
        {
            AssemblyLock.WaitOne();
            FileStream = new StreamWriter(LogFileName, append: true);
            FileStream.WriteLine(text);
            FileStream.Flush();
            FileStream.Close();
            AssemblyLock.ReleaseMutex();
        }

        /// <summary>
        /// A lock for creating and individual locks for specific assemblies.
        /// </summary>
        private static Mutex LockOfLocks { get; set; } = new Mutex(initiallyOwned: false, "WithUnity.Tools.564B132A-BCA8-4775-92D8-A835BA13D514");

        private static Mutex AssemblyLock { get; set; }

        /// <summary>
        /// Contains the list of sinks to the default methods used to implement them
        /// </summary>
        private static IDictionary<Sink, Action<string>> DefaultOutputs { get; set; } = new Dictionary<Sink, Action<string>>
        {
            { Sink.Console, Console.WriteLine },
            { Sink.Debug, OutputToDebug },
            { Sink.File, WriteToFile }
        };

        /// <summary>
        /// Contains a list of sinks to the methods used to implement them
        /// </summary>
        private static IDictionary<Sink, Action<string>> Outputs { get; set; } = new Dictionary<Sink, Action<string>>();

        /// <summary>
        /// Initializas the log
        /// </summary>
        /// <param name="level">The minimum level of logging to record. (Verbose &lt; Debug &lt; Information &lt; Warning &lt; Error &lt; UnkownState)</param>
        /// <param name="sinks">An array of sinks you want to use</param>
        /// <param name="timeOffset">Report the time offset in the log.</param>
        /// <param name="utc"><see langword="true"/> logs things against UTC time <see langword="false"/> logs against local system time.</param>
        /// <param name="logFileName">Optional file name when logging to a file, the default is %TEMP%\%ApplicationAssemblyName%.log</param>
        public static void Initialise(Level level, Sink[] sinks, bool timeOffset = false, bool utc = true, string logFileName = null)
        {
            if (level == 0)
                level = Level.UnknownState;
            LoggingLevel = level;
            TimeOffset = timeOffset;
            Utc = utc;
            EntryAssemblyName = Assembly.GetEntryAssembly().GetName().Name;
            if (logFileName == null)
            {
                LogFileName = $"{Path.Join(Environment.GetEnvironmentVariable("TEMP"),EntryAssemblyName)}.log";
            }
            else
            {
                LogFileName = logFileName;
            }
            Outputs.Clear();
            foreach (var sink in sinks)
            {
                Outputs.Add(sink, DefaultOutputs[sink]);
            }
            if (sinks.Contains(Sink.File))
            {
                // Check if the Mutex Already exists
                if (AssemblyLock == null)
                {
                    LockOfLocks.WaitOne();
                    try
                    {
                        AssemblyLock = Mutex.OpenExisting(EntryAssemblyName);
                    }
                    catch
                    {
                        // If exception occurred, there is no such mutex.
                        AssemblyLock = new Mutex(initiallyOwned: false, EntryAssemblyName);
                    }
                    LockOfLocks.ReleaseMutex();
                }
                AssemblyLock.WaitOne();
                FileStream = new StreamWriter(LogFileName, append: true);
                FileStream.Flush();
                FileStream.Close();
                AssemblyLock.ReleaseMutex();
            }
            Initialised = true;
        }

        private static void DefaultInitialise()
        {
            if (Initialised) return;
            LoggingLevel = Level.Warning;
            TimeOffset = false;
            Utc = true;
            EntryAssemblyName = Assembly.GetEntryAssembly().GetName().Name;
            LogFileName = $"{Environment.GetEnvironmentVariable("TEMP")}/{EntryAssemblyName}.log";
            Outputs.Clear();
            Outputs.Add(Sink.Console, DefaultOutputs[Sink.Console]);
            Outputs.Add(Sink.Debug, DefaultOutputs[Sink.Debug]);
            Initialised = true;
        }

        /// <summary>
        /// To change the logging level for a certain part of the code
        /// </summary>
        /// <param name="newLevel">The new minimum logging level</param>
        public static void SetLoggingLevel(Level newLevel)
        {
            if (Level.UnknownState < newLevel)
                newLevel = Level.UnknownState;
            LoggingLevel = newLevel;
        }

        /// <summary>
        /// Set Log Prefix adds The appropriate dateTime or DateTimeOffset followed by the calling Assembly's name and the warning level to the message
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        private static string SetLogPrefixText(Level level)
        {
            string prefix;
            if (TimeOffset)
            {
                if (Utc)
                {
                    prefix = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture);
                }
                else
                {
                    prefix = DateTimeOffset.Now.ToString(CultureInfo.InvariantCulture);
                }
            }
            else
            {
                if (Utc)
                {
                    prefix = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture);
                }
                else
                {
                    prefix = DateTime.Now.ToString(CultureInfo.InvariantCulture);
                }
            }

            prefix = $"{prefix}: {EntryAssemblyName}: {Process.GetCurrentProcess().Id}: {Thread.CurrentThread.ManagedThreadId}: {LevelLabels[level]}: ";
            return prefix;

        }

        /// <summary>
        /// Writes of an accepted logging level to each of the sinks in turn.
        /// </summary>
        /// <param name="level">The level of the message</param>
        /// <param name="text"></param>
        private static void WriteToLogs(Level level, string text)
        {
            if (LoggingLevel <= level)
            {
                foreach (var log in Outputs.Values)
                {
                    log(text);
                }
            }
        }

        /// <summary>
        /// The base logging used by all variants Error, Warniung etc.
        /// </summary>
        public static void LevelLog(Level level, string text)
        {
            DefaultInitialise();
            if (LoggingLevel <= level)
            {
                string prefix = SetLogPrefixText(level);
                int offset = prefix.Length + 1;
                string padding = new string(' ', offset);
                text = text.Replace("\r\n", "\r\n" + padding).Trim();
                WriteToLogs(level, $"{prefix}{text}");
            }
        }

        /// <summary>
        /// The basic Logging for exception Logs the exception and all inner exceptions
        /// </summary>
        public static void LevelLog(Level level, Exception exception)
        {
            StringBuilder sb = new StringBuilder();
            while (exception != null)
            {
                sb.Append($"'{exception.GetType().Name}': {exception.Message}\r\n");
                if (exception.StackTrace != null)
                {
                    sb.Append($"{exception.StackTrace}\r\n");
                }
                exception = exception.InnerException;
            }
            string output = sb.ToString();
            LevelLog(level, output);
        }

        /// <summary>
        /// BaseLogging with a format string and an array of parameters
        /// </summary>
        public static void LevelLog(Level level, string Format, object[] parameters)
        {
            LevelLog(level, string.Format(CultureInfo.InvariantCulture, Format, parameters));
        }

        /// <summary>
        /// Log a warning
        /// </summary>
        /// <param name="text">The text to log</param>
        public static void Warning(string text)
        {
            LevelLog(Level.Warning, text);
        }

        /// <summary>
        /// Log a warning message about an exception
        /// </summary>
        /// <param name="exception">The exception to log</param>
        public static void Warning(Exception exception)
        {
            LevelLog(Level.Warning, exception);
        }

        /// <summary>
        /// Log a formatted warning message
        /// </summary>
        /// <param name="format">The format string used in string.Format(format, parameters)</param>
        /// <param name="parameters">The parameters to pass to string.Format(format, parameters)</param>
        public static void Warning(string format, object[] parameters)
        {
            LevelLog(Level.Warning, format, parameters);
        }

        /// <summary>
        /// Log a verbose OK message
        /// </summary>
        public static void VerboseOk()
        {
            LevelLog(Level.Verbose, "OK");
        }


        /// <summary>
        /// Log a verbose message
        /// </summary>
        /// <param name="text">The text to log</param>
        public static void Verbose(string text)
        {
            LevelLog(Level.Verbose, text);
        }

        /// <summary>
        /// Log a verbose message about an exception
        /// </summary>
        /// <param name="exception">The exception to log</param>
        public static void Verbose(Exception exception)
        {
            LevelLog(Level.Verbose, exception);
        }

        /// <summary>
        /// Log a formatted verbose message
        /// </summary>
        /// <param name="format">The format string used in string.Format(format, parameters)</param>
        /// <param name="parameters">The parameters to pass to string.Format(format, parameters)</param>
        public static void Verbose(string format, object[] parameters)
        {
            LevelLog(Level.Verbose, format, parameters);
        }

        /// <summary>
        /// Log an information message
        /// </summary>
        /// <param name="text">The text to log</param>
        public static void Information(string text)
        {
            LevelLog(Level.Information, text);
        }

        /// <summary>
        /// Log an information message about an exception
        /// </summary>
        /// <param name="exception">The exception to log</param>
        public static void Information(Exception exception)
        {
            LevelLog(Level.Information, exception);
        }

        /// <summary>
        /// Log a formatted information message
        /// </summary>
        /// <param name="format">The format string used in string.Format(format, parameters)</param>
        /// <param name="parameters">The parameters to pass to string.Format(format, parameters)</param>
        public static void Information(string format, object[] parameters)
        {
            LevelLog(Level.Information, format, parameters);
        }

        /// <summary>
        /// Log a Debug message
        /// </summary>
        /// <param name="text">The text to log</param>
        public static void Debug(string text)
        {
            LevelLog(Level.Debug, text);
        }

        /// <summary>
        /// Log an debug message about an exception
        /// </summary>
        /// <param name="exception">The exception to log</param>
        public static void Debug(Exception exception)
        {
            LevelLog(Level.Debug, exception);
        }

        /// <summary>
        /// Log a formatted Debug message
        /// </summary>
        /// <param name="format">The format string used in string.Format(format, parameters)</param>
        /// <param name="parameters">The parameters to pass to string.Format(format, parameters)</param>
        public static void Debug(string format, object[] parameters)
        {
            LevelLog(Level.Debug, format, parameters);
        }

        /// <summary>
        /// Log an Error message
        /// </summary>
        /// <param name="text">The text to log</param>
        public static void Error(string text)
        {
            LevelLog(Level.Error, text);
        }

        /// <summary>
        /// Log an Error message about an exception
        /// </summary>
        /// <param name="exception">The exception to log</param>
        public static void Error(Exception exception)
        {
            LevelLog(Level.Error, exception);
        }

        /// <summary>
        /// Log a formatted Error message
        /// </summary>
        /// <param name="format">The format string used in string.Format(format, parameters)</param>
        /// <param name="parameters">The parameters to pass to string.Format(format, parameters)</param>
        public static void Error(string format, object[] parameters)
        {
            LevelLog(Level.Error, format, parameters);
        }

        /// <summary>
        /// Log an UnkownState message
        /// </summary>
        /// <param name="text">The text to log</param>
       public static void UnknownState(string text)
        {
            LevelLog(Level.UnknownState, text);
        }

        /// <summary>
        /// Log an UnknownState message about an exception
        /// </summary>
        /// <param name="exception">The exception to log</param>
        public static void UnknownState(Exception exception)
        {
            LevelLog(Level.UnknownState, exception);
        }

        /// <summary>
        /// Log a UnknownState Error message
        /// </summary>
        /// <param name="format">The format string used in string.Format(format, parameters)</param>
        /// <param name="parameters">The parameters to pass to string.Format(format, parameters)</param>
        public static void UnknownState(string format, object[] parameters)
        {
            LevelLog(Level.UnknownState, format, parameters);
        }
    }
}
