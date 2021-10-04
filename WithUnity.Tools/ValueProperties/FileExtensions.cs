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
    ///     This is provided with minimal validataion for a file fileExtension. You may want to create your own variant with different validation.
    /// </remarks>
    public sealed class FileExtension : ValueProperty<FileExtension, string>
    {
        /// <summary>
        /// Validates a file fileExtension
        /// </summary>
        /// <param name="fileExtension"></param>
        /// <remarks>
        /// This method trims whitespace from the fileExtension and removes leading "*." and then ensures the file Extension is:
        ///     Not Null
        ///     The string does not have leading or trailing white space characters.
        ///     Mot empty string
        ///     Does not contain ['\' | '/' | '.' | ':']
        /// </remarks>
        /// <returns>A Result@lt;string&gt; that contains the file extension or an error message on validation failure</returns>
        public static Result<string> ValidateFileExtension(MayBe<string> fileExtension)
        {
            return (fileExtension.HasValue ? Result.Ok<string>(fileExtension.Value) : Result.Fail<string>("Null file extension."))
                .Ensure(fe => fe.Length == fe.Trim().Length, "File extensions has leading or trailing white space characters.")
                .Ensure(fe => !string.IsNullOrWhiteSpace(fe), "File extensions cannot be blank or null")
                .Ensure(fe => !fe.ToCharArray().Contains('\\'), @"File extensions cannot contain '\'")
                .Ensure(fe => !fe.ToCharArray().Contains('/'), @"File extensions cannot contain '/'")
                .Ensure(fe => !fe.ToCharArray().Contains(':'), @"File extensions cannot contain colons.")
                .Ensure((fe) => fe.IndexOf(".", StringComparison.InvariantCulture) == -1, "The fileExtension should not contain full stops.");

        }

        /// <summary>
        /// The main constructor for a FileExtension.  If validateAndThrowOnFailure is true it calls TrimAndValidateFileExtension and throws an InvalidCastException.
        /// </summary>
        /// <param name="fileExtension">The initial file fileExtension primitive</param>
        /// <param name="validateAndThrowOnFailure">Validates the email address and throws InvalidCastException when true. Does nothing when false</param>
        /// <exception cref="InvalidCastException">The message will contain any validation errors</exception>
        private FileExtension(MayBe<string> fileExtension, bool validateAndThrowOnFailure = true) : base(fileExtension)
        {
            if (validateAndThrowOnFailure)
            {
                ValidateFileExtension(fileExtension).OnFailure(error => { throw new InvalidCastException($"Input string is not a valid file extension. Error: {error}"); });
            }
        }

        /// <summary>
        /// Use CreateFileExtension for more concise error handling on input validation where you are suspicious of the file fileExtension, e.g. user input.
        /// </summary>
        /// <param name="fileExtension">A suspect file Extension</param>
        /// <returns>Result&lt;FileExtension&gt;</returns>
        /// <remarks>
        /// This verifies:  
        ///     that the email is not <see langword="null"/>;
        ///     that email is at leaset 3 characters long;
        ///     that the email does not start with an @ sign;
        ///     that the email does not end with an @ sign;
        ///     that the email contains 1 and only 1 @ sign;
        /// </remarks>
        public static Result<FileExtension> CreateFileExtension(MayBe<string> fileExtension)
        {
            return ValidateFileExtension(fileExtension).OnSuccess<string, FileExtension>(v => new FileExtension(v, false));
        }


        /// <summary>
        /// Implicit conversion for simpler readability to MayBe&lt;String&gt;. Use this implicit cast when expecting  a valid FileExtension and throwing an exception is a reasonable thing to do.
        /// </summary>
        /// <param name="possibleFileExtension"></param>
        /// <exception cref="InvalidCastException">This is thrown if the string passed in is not a valid email with the appropriate error.</exception>
        public static implicit operator FileExtension([AllowNull] string possibleFileExtension)
        {
            return new FileExtension(new MayBe<string>(possibleFileExtension));
        }

        /// <summary>
        /// Implicit conversion from May&lt;Be&gt; string to be more explicit about whether the string is null. Use this implicit cast when expecting a valid FileExtension and throwing an exception is a reasonable thing to do.
        /// </summary>
        /// <param name="possibleFileExtension"></param>
        /// <exception cref="InvalidCastException">This is thrown if the string passed in is not a valid email with the appropriate error.</exception>
        public static implicit operator FileExtension(MayBe<string> possibleFileExtension)
        {
            return new FileExtension(possibleFileExtension);
        }


        /// <summary>
        /// The Valid File Extension's value
        /// </summary>
        public string FileExtensionValue
        {
            get
            {
                return Value.Value;
            }
        }

        /// <summary>
        /// Note: by the time this is called all checks for null and 
        /// NoValue have already been called so we just deal with the equality of valid objects.
        /// </summary>
        /// <param name="other"></param>
        /// <returns>whether the two are equal</returns>
        protected override bool EqualsCore(string other)
        {
            return Value.Equals(other);
        }

        /// <summary>
        /// ValueProperty inner GetHash routine
        /// </summary>
        /// <returns>an Integer hash of the  FileExtension class</returns>
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}
