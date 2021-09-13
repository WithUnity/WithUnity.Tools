/* Copyright 2015-2020 Robin Murison

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. */
using NullGuard;
using System;
using System.Linq;

namespace WithUnity.Tools.ValueProperties
{
    /// <summary>
    /// FileExtension ValueProperty
    /// </summary>
    /// <remarks>
    ///     This is provided with minimal validataion for a file extension. You may want to create your own variant with different validation.
    /// </remarks>
    public sealed class FileExtension : ValueProperty<FileExtension, string>
    {
        /// <summary>
        /// The constructor for a file extension
        /// </summary>
        /// <param name="extension">The initial file extension primitive</param>
        /// <exception cref="InvalidCastException">The message will contain any validation errors</exception>
        /// <remarks>
        /// It strips out any leading '*.'.
        /// 
        /// It Verifies:
        ///     The File Extension is not blank or <see langword="null"/>';
        ///     The File extension does not contain '\', '/', ':' or '.'
        /// </remarks>
        public FileExtension([AllowNull]string extension) : base(extension ?? string.Empty)
        {
            Result result = Result.Ok()
                .Ensure(!string.IsNullOrWhiteSpace(Value.Value), "File extensions cannot be blank or null")
                .Ensure(!Value.Value.ToCharArray().Contains('\\'), @"File extensions cannot contain '\'")
                .Ensure(!Value.Value.ToCharArray().Contains('/'), @"File extensions cannot contain '/'")
                .Ensure(!Value.Value.ToCharArray().Contains(':'), @"File extensions cannot contain colons.")
                .OnSuccess(() =>
                {
                    if (Value.Value.IndexOf("*.", StringComparison.InvariantCulture) == 0)
                    {
                        Value = Value.Value.Substring(2);
                    }
                }).Ensure(() => Value.Value.IndexOf(".", StringComparison.InvariantCulture) == -1, "The extension should not contain full stops.")
                .OnFailure((errorMessage) => { throw new InvalidCastException($"Input string is not a valid file extension. Error: {errorMessage}"); });
        }
    }
}
