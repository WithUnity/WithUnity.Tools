/* Copyright 2015-2020 Robin Murison

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. */
using NullGuard;
using System;

namespace WithUnity.Tools.ValueProperties
{
    /// <summary>
    /// A Value Property for validating email addresses.
    /// </summary>
    /// <remarks>
    ///     This is provided with minimal validataion for an EmailAddress. You may want to create your own variant with different validation.
    /// </remarks>
    public sealed class EmailAddress : ValueProperty<EmailAddress, string>
    {
        /// <summary>
        /// Trims any white space and validates an email address
        /// </summary>
        /// <param name="emailAddressValue"></param>
        /// <remarks>
        /// This verifies:  
        ///     that the email is not <see langword="null"/>;
        ///     that email is at leaset 3 characters long;
        ///     that the email does not start with an @ sign;
        ///     that the email does not end with an @ sign;
        ///     that the email contains 1 and only 1 @ sign;
        /// </remarks>
        public static Result<string> ValidateEmailAddress(MayBe<string> emailAddressValue)
        {
            return (emailAddressValue.HasValue ? Result.Ok<string>(emailAddressValue.Value) : Result.Fail<string>("Null email Address"))
                .Ensure(ema => ema.Length == ema.Trim().Length, "Email Address needs trimming")
                .Ensure(ema => 3 <= ema.Length, "The email address is too short")
                .Ensure(ema => 0 < ema.IndexOf("@", StringComparison.InvariantCulture), "No preceding name before @ sign in EmailAddress")
                .Ensure(ema => ema.IndexOf("@", StringComparison.InvariantCulture) != (ema.Length - 1), "No domain name after @ sign in EmailAddress")
                .Ensure(ema => ema.Split('@').Length == 2, @"There are {ema.Split('@').Length - 1} @ signs. Should be 1.");
        }

        /// <summary>
        /// The main constructor for validating email addresses. This is used by implicit casts, when expecting a valid EmailAddress and throwing an exception on failure is a reasonable thing to do.
        /// </summary>
        /// <param name="emailAddressValue"></param>
        /// <param name="validateAndThrowOnFailure">Validates the email address and throws InvalidCastException when true. Does nothing when false</param>
        /// <exception cref="InvalidCastException">This is thrown if the string passed in is not a valid email with the appropriate error.</exception>
        /// <remarks>
        /// This verifies:  
        ///     that the email is not <see langword="null"/>;
        ///     that email is at leaset 3 characters long;
        ///     that the email does not start with an @ sign;
        ///     that the email does not end with an @ sign;
        ///     that the email contains 1 and only 1 @ sign;
        /// </remarks>
        private EmailAddress(MayBe<string> emailAddressValue, bool validateAndThrowOnFailure = true) : base(emailAddressValue)
        {
            if (validateAndThrowOnFailure)
            {
                ValidateEmailAddress(emailAddressValue).OnFailure(error => { throw new InvalidCastException(error); });
            }
        }

        /// <summary>
        /// Use CreateEmailAddress for more concise error handling in cases where you are suspicious of the email source such as it comes from user input.
        /// </summary>
        /// <param name="emailAddressValue"></param>
        /// <returns>Result&lt;EmailAddress&gt;</returns>
        /// <remarks>
        /// This verifies:  
        ///     that the email is not <see langword="null"/>;
        ///     that email is at leaset 3 characters long;
        ///     that the email does not start with an @ sign;
        ///     that the email does not end with an @ sign;
        ///     that the email contains 1 and only 1 @ sign;
        /// </remarks>
        public static Result<EmailAddress> CreateEmailAddress(MayBe<string> emailAddressValue)
        {
            return ValidateEmailAddress(emailAddressValue).OnSuccess<string, EmailAddress>(v => new EmailAddress(v, false));
        }

        /// <summary>
        /// Implicit conversion for simpler readability to MayBe&lt;String&gt;. Only use this implicit cast when expecting a valid EmailAddress and throwing an exception is a reasonable thing to do.
        /// </summary>
        /// <param name="possibleEmail"></param>
        /// <exception cref="InvalidCastException">This is thrown if the string passed in is not a valid email with the appropriate error.</exception>
        /// <remarks>This implicitly casts a sting parameter to a MayBe&lt;string&gt; parameter and then use that implicit operator with the MayBe&lt;string&gt; parameter.
        /// </remarks>
        public static implicit operator EmailAddress([AllowNull]string possibleEmail)
        {
            return new EmailAddress(new MayBe<string>(possibleEmail));
        }

        /// <summary>
        /// Implicit conversion from May&lt;Be&gt; string to be more explicit about whether the string is null. Only use this implicit cast when expecting a valid EmailAddress and throwing an exception is a reasonable thing to do.
        /// </summary>
        /// <param name="possibleEmail"></param>
        /// <exception cref="InvalidCastException">This is thrown if the string passed in is not a valid email with the appropriate error.</exception>
        public static implicit operator EmailAddress(MayBe<string> possibleEmail)
        {
            return new EmailAddress(possibleEmail);
        }

        /// <summary>
        /// The Valid EmailAddress value
        /// </summary>
        public string EmailAddressValue
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
        /// <returns>an Integer hash of the  emailaddress class</returns>
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}
