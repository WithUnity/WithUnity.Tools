/* Copyright 2015-2021 Robin Murison

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. */
using NullGuard;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace WithUnity.Tools
{
    /// <summary>
    /// Wraps the results of methods into a standard class.
    /// 
    /// This allows a series of method calls to be chained.
    /// 
    /// This means that you can do error handling in a lot cleaner way.
    /// </summary>
    public class Result
    {
        ///// <summary>
        ///// NullArgumentExcetptionCheck
        ///// </summary>
        ///// <param name="parameters">The list of incoming parameters</param>
        ///// <remarks>Needs Unit Test and joining to constructors</remarks>
        //public Result ThrowArgumentNullException(object[] parameters)
        //{
        //    // Get call stack
        //    StackTrace stackTrace = new StackTrace();
        //    var sf = stackTrace.GetFrame(1);
        //    var mi =sf.GetMethod() as MethodInfo;
            
        //    var ps = mi.GetParameters();
        //    if (ps.Length != parameters.Length)
        //    {
        //        new InvalidProgramException($"{mi.Name} past in {parameters.Length} values. There should be {ps.Length} Parameters");
        //    }
        //    for (int i = 0; i < ps.Length;i++)
        //    {
        //        ParameterInfo pi = ps[i];
        //        object param = parameters[i];
        //        if (param == null)
        //        {
        //            throw new ArgumentNullException($"For Method {mi.Name}: Parameter{i}:{pi.Name} of type {pi.ParameterType} is null");
        //        }
        //    }
        //    return Result.Ok();
        //}

        /// <summary>
        /// Used to test for a successful result.
        /// </summary>
        public bool IsSuccess { get; }

        /// <summary>
        /// Used to test whether the results has any notes.
        /// </summary>
        public bool HasNotes { get; private set; }

        /// <summary>
        /// The returned error message.
        /// This returns the first error that caused a failure.
        /// </summary>
        public string Error { get; }

        /// <summary>
        /// A list of Notes.
        /// </summary>
        public IList<string> Notes { get; private set; }

        /// <summary>
        /// Used to test if the result is a failure.
        /// </summary>
        public bool IsFailure => !IsSuccess;

        /// <summary>
        /// Used to store an exception, if an exception caused the failure
        /// </summary>
        public MayBe<Exception> Exception { get; }

        /// <summary>
        /// Used for logging errors so that we can log where the error occured.
        /// </summary>
        public string CallingMethod { get; }

        #region Constructors

        /// <summary>
        /// Constructs a new copy of a result from the previous version
        /// </summary>
        /// <param name="result"></param>
        protected Result(Result result)
        {
            CallingMethod = result.CallingMethod;
            Error = result.Error;
            Exception = result.Exception;
            HasNotes = result.HasNotes;
            IsSuccess = result.IsSuccess;
            if (!IsSuccess)
            {
                Log.Error(Error);
            }
            else
            {
                Log.VerboseOk();
            }
            Notes = new List<string>();
            foreach (var note in result.Notes)
            {
                Notes.Add(note);
                Log.Information(note);
            }
        }
        /// <summary>
        /// The protected constructor. Used when a logically discovered error or success was found.
        /// </summary>
        /// <param name="isSuccess">Whether the result is a success</param>
        /// <param name="error">If the result is a failure this contains the error message</param>
        /// <param name="callingMethod">The method that is expecting the result used for logging errors.</param>
        /// <exception cref="InvalidOperationException">If IsSuccess and there is an error text xor It is not a success and there is not an error text.</exception>
        protected Result(bool isSuccess, string error, string callingMethod)
        {
            if (isSuccess && !string.IsNullOrWhiteSpace(error))
                throw new InvalidOperationException();
            if (!isSuccess && string.IsNullOrWhiteSpace(error))
                throw new InvalidOperationException();

            IsSuccess = isSuccess;
            Error = error;
            if (!isSuccess)
            {
                Log.Error(Error);
            }
            else
            {
                Log.VerboseOk();
            }
            Notes = new List<string>();
            HasNotes = false;
            Exception = null;
            CallingMethod = callingMethod;
        }

        /// <summary>
        /// The protected constructor. Used when an error occurs due to an exception.
        /// </summary>
        /// <param name="exception">If the result is a failure this contains the error message</param>
        /// <param name="callingMethod">The method that is expecting the result used for logging errors.</param>
        protected Result(Exception exception, string callingMethod)
        {
            CallingMethod = callingMethod;
            IsSuccess = false;
            Error = $"Exception thrown by  {CallingMethod}. It threw exception '{exception.GetType().Name}', with error message '{exception.Message}'";
            Log.Error(Error);
            Log.Error(exception);
            Notes = new List<string>();
            HasNotes = false;
            Exception = exception;
        }
        #endregion

        /// <summary>
        /// Static failure Result constructor method.
        /// </summary>
        /// <param name="message">Some text giving the reason for the failure.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static Result Fail(string message)
        {
            return new Result(false, message, ReflectiveTools.CallingMethod(2));
        }

        /// <summary>
        /// Static failure Result constructor method.
        /// </summary>
        /// <param name="exception">The exception that caused the failure</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static Result Fail(Exception exception)
        {
            return new Result(exception, ReflectiveTools.CallingMethod(2));
        }

        /// <summary>
        /// Fail&lt;T&gt; returns a failure with the appropriate message.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message">The error message you want to use</param>
        /// <param name="callingMethod">Should be left at the default blank string. Present for internal use.</param>
        /// <returns>A typed result with a failure report.</returns>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static Result<T> Fail<T>(string message, string callingMethod = "")
        {
            if (string.IsNullOrWhiteSpace(callingMethod))
            {
                callingMethod = ReflectiveTools.CallingMethod(2);
            }
            return new Result<T>(default, false, message, callingMethod);
        }

        /// <summary>
        /// Fail&lt;T&gt; returns a failure with the appropriate exception and message.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exception">The excepion you need to report</param>
        /// <param name="callingMethod">Should be left at the default blank string. Present for internal use.</param>
        /// <returns>A typed result with a failure report.</returns>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static Result<T> Fail<T>(Exception exception, string callingMethod = "")
        {
            if (string.IsNullOrWhiteSpace(callingMethod))
            {
                callingMethod = ReflectiveTools.CallingMethod(2);
            }
            return new Result<T>(default(T), exception, callingMethod);
        }

        /// <summary>
        /// The standard OK constructor method
        /// </summary>
        /// <returns>An OK result</returns>
        public static Result Ok()
        {
            return new Result(true, string.Empty, ReflectiveTools.CallingMethod(2));
        }

        /// <summary>
        /// Ok sets up a valid value of the <![CDATA[.]]>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="callingMethod">This should be left at the default value. Only used for internal use.</param>
        /// <returns></returns>
        public static Result<T> Ok<T>(T value, string callingMethod = "")
        {
            if (string.IsNullOrWhiteSpace(callingMethod))
            {
                callingMethod = ReflectiveTools.CallingMethod(2);
            }
            return new Result<T>(value, true, string.Empty, callingMethod);
        }

        /// <summary>
        /// Initialize takes the current value and if it is not Null then it does the same as OK
        /// and if it is null then it does the same as Fail
        /// </summary>
        /// <param name="value">The value to set</param>
        /// <param name="error">The error message to show if this is null</param>
        /// <param name="callingMethod">The method receiving the Result</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static Result<T> Initialize<T>([AllowNull]T value, string error, string callingMethod = "")
        {
            if (string.IsNullOrWhiteSpace(callingMethod))
            {
                callingMethod = ReflectiveTools.CallingMethod(2);
            }
            if (value != null)
            {
                error = string.Empty;
            }
            return new(value, value != null, error, callingMethod);
        }

        /// <summary>
        /// Initialize takes the current value and if it is not Null then it does the same as OK
        /// and if it is null then it does the same as Fail
        /// </summary>
        /// <param name="value">The value to set</param>
        /// <param name="error">The error message to show if this is null</param>
        /// <param name="callingMethod">The method receiving the Result</param>
        /// <returns>The validated result</returns>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static Result<T> Initialize<T>(MayBe<T> value, string error, string callingMethod = "") where T : class
        {
            if (string.IsNullOrWhiteSpace(callingMethod))
            {
                callingMethod = ReflectiveTools.CallingMethod(2);
            }
            Result<T> result;
            if (value.HasValue)
            {
                error = string.Empty;
                result = new Result<T>(value.Value, true, error, callingMethod);
            }
            else
            {
                result = new Result<T>(null, false, error, callingMethod);
            }
            return result;
        }

        /// <summary>
        /// Result.Initialize uses a boolean predicate to set a Result to Ok if true or fail if false.
        /// </summary>
        /// <param name="predicate">The predicate to create an Ok Result or a failed Result.</param>
        /// <param name="errorMessage">The Error message to store in case the predicate is false.</param>
        /// <returns>The next result in the chain</returns>
        /// <remarks>This is using Result to simplify if tests as opposed to recording a true failure.
        /// </remarks>
        public static Result Initialize(bool predicate, string errorMessage)
        {
            return predicate ? Result.Ok() : Result.Fail(errorMessage);
        }

        /// <summary>
        /// AddNote adds a note that can be used to allow the calling method to
        /// have more information than a task succeeded  or failed.
        /// Mainly used for debugging or testing for unusual conditions.
        /// </summary>
        /// <param name="newNote">A comment on an unusual event.</param>
        public void AddNote(string newNote)
        {
            Notes.Add(newNote);
            HasNotes = true;
        }
    }


    /// <summary>
    /// Result&lt;T&gt; is a Result that holds some data on success.
    /// </summary>
    /// <typeparam name="T">The type of data the Result holds</typeparam>
    public class Result<T> : Result
    {
        private readonly T _Value;

        /// <summary>
        /// The Value held of Type T.
        /// </summary>
        public T Value
        {
            get
            {
                if (!IsSuccess)
                    throw new InvalidOperationException();

                return _Value;
            }
        }

        /// <summary>
        /// The Result constructor used internally to pass on a previous Result
        /// </summary>
        /// <param name="result">A previous result</param>
        protected internal Result(Result<T> result)
            : base(result)
        {
            _Value = result._Value;
        }

        /// <summary>
        /// The Result constructor used internally with an error message
        /// </summary>
        /// <param name="value">The value to be stored</param>
        /// <param name="isSuccess">Whether the Result is a success (true) or a failure (false)</param>
        /// <param name="error">Text containing the an explanation of the error that caused a failure if it failed</param>
        /// <param name="callingMethod">The method that called the public constructor for logging purposes.</param>
        protected internal Result(T value, bool isSuccess, string error, string callingMethod)
            : base(isSuccess, error, callingMethod)
        {
            _Value = value;
        }

        /// <summary>
        /// The Result constructor used internally with an exception
        /// </summary>
        /// <param name="value">The value to be stored</param>
        /// <param name="exception">The exception that caused the failure</param>
        /// <param name="callingMethod">The method that called the public constructor for logging purposes.</param>
        protected internal Result(T value, Exception exception, string callingMethod)
            : base(exception, callingMethod)
        {
            _Value = value;
        }

#pragma warning disable CA1000 // Do not declare static members on generic types
        // Justification = "Extension methods have to be static"

        /// <summary>
        /// Implicitly casts an object T to the appropriate Result.
        /// Returns Fail() if null or OK if a valid value
        /// </summary>
        /// <param name="value">A potentially null pointer</param>
        /// <remarks>TODO: Add unit test</remarks>
        public static Result<T> ToResult([AllowNull]T value)
#pragma warning restore CA1000 // Do not declare static members on generic types
        {
            if (value == null)
            {
                return Result<T>.Fail<T>($"Result<{typeof(T).Name}> invalid implicit cast.", ReflectiveTools.CallingMethod(2));
            }
            return Result<T>.Ok<T>(value, ReflectiveTools.CallingMethod(2));
        }

        /// <summary>
        /// Implicitly casts an object T to the appropriate Result.
        /// Returns Fail() if null or OK if a valid value
        /// </summary>
        /// <param name="value">A potentially null pointer</param>
        /// <remarks>TODO: Add unit test</remarks>
        public static implicit operator Result<T>([AllowNull]T value)
        {
            if (value == null)
            {
                return Result<T>.Fail<T>($"Result<{typeof(T).Name}> invalid implicit cast.", ReflectiveTools.CallingMethod(2));
            }
            return Result<T>.Ok<T>(value, ReflectiveTools.CallingMethod(2));
        }

#pragma warning disable CA1000 // Do not declare static members on generic types
        // Justification = "Extension methods have to be static"

        /// <summary>
        /// Initialize takes the current value and if it is not Null the does the same as OK
        /// and if it is null then it does the same as Fail
        /// </summary>
        /// <param name="value">The value to set</param>
        /// <param name="error">The error message to show if this is null</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static Result<T> Initialize([AllowNull]T value, string error)
#pragma warning restore CA1000 // Do not declare static members on generic types
        {
            return Result.Initialize<T>(value, error, ReflectiveTools.CallingMethod(2));
        }
    }
}