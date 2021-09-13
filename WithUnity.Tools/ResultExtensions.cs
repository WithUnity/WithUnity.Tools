/* Copyright 2015-2021 Robin Murison

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. */
using System;
using System.Collections.Generic;

namespace WithUnity.Tools
{
    /// <summary>
    /// A series of error validation
    /// methods to call methods either 
    /// OnSuccess OnFailure or OnBoth.
    /// OnSuccess usually Chains to the next item in the list unless an error occured in which case it passes on the error.
    /// OnFailure does something only if the result has already failed. The Error is passed down the list.
    /// All subsequent OnSuccess fail.
    /// OnBoth happens in either case. e.g. releasing memory or logging.
    /// Ensure queries a predicate method or value. If the predicate fails, it raises an error that is passed on or the previous value is passed on.
    /// Add as many variants as you need for the methods you use.
    /// </summary>
    public static class ResultExtensions
    {
        /// <summary>
        /// Usually used in Get data calls.
        /// Converts the object to a Result.
        /// </summary>
        /// <typeparam name="T">The type of object got</typeparam>
        /// <param name="nullable">The MayBe reference to the object</param>
        /// <param name="errorMessage">The Error message we want to show users or log.</param>
        /// <returns>The result to the Get call wrapped up in a Result.</returns>
        public static Result<T> ToResult<T>(this MayBe<T> nullable, string errorMessage) where T : class
        {
            if (nullable.HasNoValue)
                return Result.Fail<T>(errorMessage);

            return Result.Ok(nullable.Value);
        }

        /// <summary>
        /// If the result is a success then we call the Action
        /// otherwise we pass on the failed result.
        /// </summary>
        /// <param name="result">The incoming result</param>
        /// <param name="action">The void method to call.</param>
        /// <returns>The next result in the chain.</returns>
        public static Result OnSuccess(this Result result, Action action)
        {
            if (result.IsFailure)
                return result;

            action();

            return result;
        }

        /// <summary>
        /// OnSuccess calls an action only if previous steps are successful.
        /// This is for house keeping actions. That do not affect the Result.
        /// </summary>
        /// <typeparam name="T">The inpout and output type</typeparam>
        /// <param name="result">The Incoming and out goiut</param>
        /// <param name="action">The Action to run</param>
        /// <returns></returns>
        public static Result<T> OnSuccess<T>(this Result<T> result, Action action)
        {
            if (result.IsFailure)
                return result;
            action();
            return result;
        }

        /// <summary>
        /// OnSuccess calls a function to modify the Value.
        /// </summary>
        /// <typeparam name="T">The input and output type</typeparam>
        /// <param name="result">The Incoming and out going</param>
        /// <param name="action">An action that uses the value but does not change it.</param>
        /// <returns></returns>
        public static Result<T> OnSuccess<T>(this Result<T> result, Action<T> action)
        {
            if (result.IsFailure)
                return result;
            action(result.Value);
            return result;
        }

        /// <summary>
        /// OnSuccess calls a function to modify the Value.
        /// </summary>
        /// <typeparam name="T">The input and output type</typeparam>
        /// <param name="result">The Incoming result.</param>
        /// <param name="func">The Func to run</param>
        /// <returns>A replacement Result with a modifed Value.</returns>
        /// <remarks>Note this version does not allow for error checking.</remarks>
        public static Result<T> OnSuccess<T>(this Result<T> result, Func<T, T> func)
        {
            if (result.IsFailure)
                return result;
            return Result.Ok(func(result.Value));
        }

        /// <summary>
        /// if all previous steps are successful, OnSuccess calls a Function on the value and returns a Result<typeparamref name="T"/>.
        /// This is for house keeping actions. That do not affect the Result.
        /// </summary>
        /// <typeparam name="T">The input and output type</typeparam>
        /// <param name="result">The Incoming and out going</param>
        /// <param name="func">The Func to run that does error checking</param>
        /// <returns>A new Result&lt;T&gt; with a modifed Value and possibly an error result.</returns>
        /// <remarks>Note this version does not allow for error checking.</remarks>
        public static Result<T> OnSuccess<T>(this Result<T> result, Func<T, Result<T>> func)
        {
            if (result.IsFailure)
                return result;
            return func(result.Value);
        }

        /// <summary>
        /// if all previous steps are successful, OnSuccess calls a function and returns.
        /// This is for house keeping actions. That do not affect the Result.
        /// </summary>
        /// <typeparam name="T">The input and output Result<typeparamref name="T"/>, unless func fails</typeparam>
        /// <param name="result">The Incoming and out going result</param>
        /// <param name="func">The Function to run</param>
        /// <returns>
        /// 1. If it has previously failed it returns that failure;
        /// 2. If the Func was a failure then it returns that failure;
        /// 3. If the Func was a success is passes on the Previous the Result with the previous Value.</returns>
        public static Result<T> OnSuccess<T>(this Result<T> result, 
            Func<T, Result> func)
        {
            if (result.IsFailure)
                return result;
            var internalResult = func(result.Value);
            internalResult.OnFailure(
                (error) =>
                {
                    result = Result.Fail<T>(error);
                });
            return result;
        }

        /// <summary>
        /// if all previous steps are successful, OnSuccess use a function that changes input type Result&lt;TInput&gt; to output type Resukt&lt;TOutput&gt;
        /// </summary>
        /// <typeparam name="TInput">The incoming Result&lt;TInput&gt;</typeparam>
        /// <typeparam name="TOutput">The returned Result&lt;TOutput&gt;</typeparam>
        /// <param name="result">The incoming result</param>
        /// <param name="func">The function used to swap between the two types</param>
        /// <remarks>There is no error handling in the Func and it has to succeed. 
        /// If you want error handling add a similar static method replacing Func&lt;TInput, TOutput&gt; with Func&lt;TInput, Result&lt;TOutput&gt;&gt;</remarks>
        /// <returns>
        /// 1. If it has previously a failed, it returns that failure. Otherwise it calls the func;
        /// 2. If returns a new Result&lt;TOutput&gt; containing the return value of the Func.
        /// </returns>
        public static Result<TOutput> OnSuccess<TInput, TOutput>(this Result<TInput> result, Func<TInput, TOutput> func)
        {
            Result<TOutput> newResult;
            if (result.IsFailure)
            {
                if (result.Exception.HasValue)
                {
                    newResult = Result.Fail<TOutput>(result.Exception.Value);
                }
                else
                {
                    newResult = Result.Fail<TOutput>(result.Error);
                }
            }
            else
            {
                newResult = Result.Ok(func(result.Value));
            }
            if (result.HasNotes)
            {
                foreach (var note in result.Notes)
                {
                    newResult.AddNote(note);
                }
            }
            return newResult;
        }


        /// <summary>
        /// if all previous steps are successful, OnSuccess use a function that changes input type Result&lt;TInput&gt; to output type Resukt&lt;TOutput&gt;
        /// </summary>
        /// <typeparam name="TInput">The incoming Result&lt;TInput&gt;</typeparam>
        /// <typeparam name="TOutput">The returned Result&lt;TOutput&gt;</typeparam>
        /// <param name="result">The incoming result</param>
        /// <param name="func">The function takes incoming TInput and directly returns a Result&lt;<typeparamref name="TOutput"/>&gt; that can be returned directly</param>
        /// <remarks>This version has error handling</remarks>
        /// <returns>
        /// 1. If it has previously a failed, it returns that failure. Otherwise it calls the func;
        /// 2. If returns a new Result&lt;TOutput&gt; containing the return value of the Func.
        /// </returns>
        public static Result<TOutput> OnSuccess<TInput, TOutput>(this Result<TInput> result, Func<TInput, Result<TOutput>> func)
        {
            if (result.IsFailure)
                return Result.Fail<TOutput>(result.Error);
            return func(result.Value);
        }

        /// <summary>
        /// If the previous result has not failed then we call the Function
        /// returning a Result otherwise we pass the failed result.
        /// down the list.
        /// </summary>
        /// <param name="result">The incoming result</param>
        /// <param name="func">The method returning a Result to call.</param>
        /// <returns>The failure result passed down the chain or the return result of the Function.</returns>
        public static Result OnSuccess(this Result result, Func<Result> func)
        {
            if (result.IsFailure)
                return result;

            return func();
        }

        /// <summary>
        /// If the result has failed then we call the Action
        /// otherwise we pass on the failed result.
        /// </summary>
        /// <param name="result">The incoming result</param>
        /// <param name="action">The void method to call. Usually error 
        /// reporting or returning an error.</param>
        /// <returns>Passes the Result down the chain.</returns>
        public static Result OnFailure(this Result result, Action action)
        {
            if (result.IsFailure)
            {
                action();
            }

            return result;
        }

        /// <summary>
        /// OnFailure it generate a replacement default value. OnSuccess it is left as was.
        /// If you need to handle success before generating a default always call OnSuccess first because OnFailureGeneratedDefault will also be a success.
        /// Note: the default value will normally be a success. At least I cannot think of a reason why you would generate a bad default.
        /// </summary>
        /// <typeparam name="T">The data wrapped in this Result&lt;T&gt;</typeparam>
        /// <param name="result">The incoming result</param>
        /// <param name="func">A function to generate a default replacement value.</param>
        /// <returns>On Failure it generates a new valid Result, On Success it Passes the Result down the chain.</returns>
        public static Result<T> OnFailureGeneratedDefault<T>(this Result<T> result, Func<Result<T>> func)
        {
            if (result.IsFailure)
            {
                return func();
            }

            return result;
        }

        /// <summary>
        /// If the result has failed then we call the Action
        /// otherwise we pass on the failed result.
        /// </summary>
        /// <typeparam name="T">The data wrapped in this Result&lt;T&gt;</typeparam>
        /// <param name="result">The incoming result</param>
        /// <param name="action">The void method to call. Usually error 
        /// reporting or returning an error.</param>
        /// <returns>Passes the Result down the chain.</returns>
        public static Result<T> OnFailure<T>(this Result<T> result, Action action)
        {
            if (result.IsFailure)
            {
                action();
            }

            return result;
        }

        /// <summary>
        /// If the result has failed then we call the Action
        /// otherwise we pass on the failed result.
        /// </summary>
        /// <param name="result">The incoming result</param>
        /// <param name="action">A method call that takes the current Error message.</param>
        /// <returns>Passes the Result down the chain.</returns>
        public static Result OnFailure(this Result result, Action<string> action)
        {
            if (result.IsFailure)
            {
                action(result.Error);
            }

            return result;
        }

        /// <summary>
        /// If the result has failed then we Report the Error value
        /// otherwise we pass on the result.
        /// </summary>
        /// <typeparam name="T">The type of the contained data.</typeparam>
        /// <param name="result">The incoming result</param>
        /// <param name="action">The void method to call. That OnFailure reports the error or throws an exception.</param>
        /// <returns>Passes the result down the chain.</returns>
        public static Result<T> OnFailure<T>(this Result<T> result, Action<string> action)
        {
            if (result.IsFailure)
            {
                action(result.Error);
            }

            return result;
        }

        /// <summary>
        /// We call the Action regardless of the Result
        /// and pass the Result down the Chain.
        /// </summary>
        /// <param name="result">The incoming result</param>
        /// <param name="action">The void method to call. that uses or affects the result. Usually closing files or handles...</param>
        /// <returns>Passes the Result down the chain.</returns>
        public static Result OnBoth(this Result result, Action<Result> action)
        {
            action(result);

            return result;
        }

        /// <summary>
        /// We call the Action regardless of the Result
        /// and pass the Result down the Chain.
        /// </summary>
        /// <param name="result">The incoming result</param>
        /// <param name="action">The void method to call. That has no use for the result. Usually closing files or handles...</param>
        /// <returns>Passes the Result down the chain.</returns>
        /// <remarks>TODO Unit tests</remarks>
        public static Result OnBoth(this Result result, Action action)
        {
            action();

            return result;
        }

        /// <summary>
        /// We call the Function regardless of the Result
        /// and pass in the Result.
        /// It then returns a value of Type T.
        /// </summary>
        /// <typeparam name="T">The type the func returns</typeparam>
        /// <param name="result">The incoming result</param>
        /// <param name="func">The Func to call on that Result that will return a value of type T</param>
        /// <returns>Data of an unspecified Type</returns>
        public static T OnBoth<T>(this Result result, Func<Result, T> func)
        {
            return func(result);
        }

        /// <summary>
        /// Ensures that a predicate is true if not it raises an Error
        /// </summary>
        /// <typeparam name="T">Incoming wrapped type</typeparam>
        /// <param name="result">Incoming Resukt object</param>
        /// <param name="predicate">The loical predicate method to perform</param>
        /// <param name="errorMessage">Error message</param>
        /// <returns>The next result in the chain</returns>
        public static Result<T> Ensure<T>(this Result<T> result, Func<T, bool> predicate, string errorMessage)
        {
            if (result.IsFailure)
                return result;
            if (!predicate(result.Value))
                return Result.Fail<T>(errorMessage);

            return result;
        }

        /// <summary>
        /// Ensures that a predicate is true if not it raises an Error, using a lamda function.
        /// </summary>
        /// <typeparam name="T">Type of  wrapped data</typeparam>
        /// <param name="result">Incoming Result object</param>
        /// <param name="predicate">The logical predicate method to perform</param>
        /// <param name="errorMessage">Error message</param>
        /// <returns>The next result in the chain</returns>
        public static Result<T> Ensure<T>(this Result<T> result, Func<bool> predicate, string errorMessage)
        {
            return Ensure<T>(result, predicate(), errorMessage);
        }

        /// <summary>
        /// Ensures that a predicate is true if not it raises an Error
        /// </summary>
        /// <typeparam name="T">Type of  wrapped data</typeparam>
        /// <param name="result">Incoming Resukt object</param>
        /// <param name="predicate">The logical predicate method to perform</param>
        /// <param name="errorMessage">Error message</param>
        /// <returns>The next result in the chain</returns>
        public static Result<T> Ensure<T>(this Result<T> result, bool predicate, string errorMessage)
        {
            if (result.IsFailure)
                return result;
            if (!predicate)
                return Result.Fail<T>(errorMessage);

            return result;
        }

        /// <summary>
        /// Ensures that a predicate is true if not it raises an Error. Using a function or Lamda.
        /// </summary>
        /// <param name="result">Incoming Result object</param>
        /// <param name="predicate">The logical predicate method to perform</param>
        /// <param name="errorMessage">Error message</param>
        /// <returns>The next result in the chain</returns>
        public static Result Ensure(this Result result, Func<bool> predicate, string errorMessage)
        {
            return Ensure(result, predicate(), errorMessage);
        }

        /// <summary>
        /// Ensures that a predicate is true if not it raises an Error
        /// </summary>
        /// <param name="result">Incoming Result object</param>
        /// <param name="predicate">The logical predicate as a boolean</param>
        /// <param name="errorMessage">Error message</param>
        /// <returns>The next result in the chain</returns>
        public static Result Ensure(this Result result, bool predicate, string errorMessage)
        {
            if (result.IsFailure)
                return result;
            if (!predicate)
                return Result.Fail(errorMessage);

            return result;
        }

        /// <summary>
        /// Ensures that a predicate is true if not it raises an Error.
        /// It also returns the result of the Predicate in an out parameter and
        /// adds any errors to a collection of error strings.
        /// </summary>
        /// <typeparam name="T">Type of  wrapped data</typeparam>
        /// <param name="result">Incoming Result object</param>
        /// <param name="predicate">The local predicate method to perform</param>
        /// <param name="errorMessage">Error message</param>
        /// <param name="storedResult">Allows the result to be stored for separate processing</param>
        /// <param name="errors">A collection of error messages to store all errors.</param>
        /// <returns>The next result in the chain</returns>
        public static Result<T> Ensure<T>(this Result<T> result, bool predicate, string errorMessage, out bool storedResult, ICollection<string> errors = null)
        {
            storedResult = predicate;
            if (result.IsFailure)
                return result;
            if (!storedResult)
            {
                if (errors != null)
                {
                    errors.Add(errorMessage);
                }
                return Result.Fail<T>(errorMessage);
            }
            return result;
        }

        /// <summary>
        /// Ensures that a predicate is true if not it raises an Error
        /// over the whole of an array.
        /// The input arrays may be null and so we test for that too
        /// and return true if they are both null or wrongLengthErrorMessage if only one of them is null.
        /// </summary>
        /// <param name="result">Incoming Resukt object</param>
        /// <param name="array1">The first array to be compared (Maybe null)</param>
        /// <param name="array2">The second array to be compared against (Maybe null).</param>
        /// <param name="predicate">The logical predicate method to perform. Note null arrays are considered equal.</param>
        /// <param name="NullErrorMessage">A Function to return which parameter array [1 or 2] is null assuming only one of them is null.</param>
        /// <param name="wrongLengthErrorMessage">A wrong length method. The length of the two arrays will be passed in.</param>
        /// <param name="predicateErrorMessage">A method to return the predicate Error Message.  
        /// This allows the use of parameters without forcing you to use them. The parameters passed in are the offset 
        /// and the two objects that differ at that offset.</param>
        /// <returns>The next result in the chain</returns>
        public static Result Ensure(this Result result, 
                                    MayBe<object[]> array1,
                                    MayBe<object[]> array2, 
                                    Func<object, object, bool> predicate,
                                    Func<int, string> NullErrorMessage,
                                    Func<int, int, string> wrongLengthErrorMessage,
                                    Func<int, object,  object, string> predicateErrorMessage)
        {
            if (result.IsFailure)
                return result;

            if (array1.HasNoValue)
            {
                if (array2.HasNoValue) return result;
                return Result.Fail(NullErrorMessage(1));
            }

            if (array2.HasNoValue)
            {
                return Result.Fail(NullErrorMessage(2));
            }

            if (array1.Value.Length != array2.Value.Length)
                return Result.Fail(wrongLengthErrorMessage(array1.Value.Length, array2.Value.Length));

            for (int i = 0; i < array1.Value.Length; i++)
            {
                if (!predicate(array1.Value[i], array2.Value[i]))
                    return Result.Fail(predicateErrorMessage(i, array1.Value[i], array2.Value[i]));
            }
            return result;
        }

        /// <summary>
        /// Initialize ensures that the data is a valid object and returns a Result.Fail&lt;T&gt; if it is not
        /// or returns the A Result.Ok&lt;T&gt; if the constructor is OK.
        /// </summary>
        /// <typeparam name="T">Type of  wrapped data</typeparam>
        /// <param name="data">data is a structure MayBe&lt;T*gt; that can contain null or a value</param>
        /// <param name="errorMessage">Error message</param>
        /// <returns>The next result in the chain</returns>
        /// <remarks>TODO: Add unit tests</remarks>
        public static Result<T> Initialize<T>(MayBe<T> data, string errorMessage) where T : class, new()
        {
            return data.HasValue ? Result.Ok(data.Value) : Result.Fail<T>(errorMessage);
        }

        /// <summary>
        /// Converts a Result&lt;<typeparamref name="T"/>&gt; to a Result, because. Some results need to hide the data.
        /// </summary>
        /// <typeparam name="T">The type of the underlying data</typeparam>
        /// <param name="result">The incoming result.</param>
        /// <returns>A simple Result with out the data.</returns>
        public static Result StripData<T>(this Result<T> result)
        {
            return result.IsFailure ? Result.Fail(result.Error) : Result.Ok();
        }
    }
}