# WithUnity.Tools
WithUnity.Tools Provides:
1. The Result class to allow fluent Validation.
2. The MayBe struct is used in conjunction with Fody.NullGuard to make optional object parameters more evident and to ensure ArgumentNullException is thrown if the MayBe struct is not used.
3. Logging of the validation results if desired using the Log class.
4. The ValueProperty class to create readonly properties. Adding validation to individual properties and throw InvalidCastException if the data is not valid. The ValueProperty class can be made transparent to the user as implicit casts to and from the ValueProperty can be provided.
5. A few common uses of ValueProperty classes FileExtension EmailAddress, Unicode16 string. Using these ValueProperty classes is transparent to the user as implicit casts to and from the ValueProperty and the HeldType have been provided. 
6. Result uses the ReflectiveTools class to determine where errors occur in the code. The Log class and the ReflectiveTools class can be used independently.
---
## Example usage of Result, Maybe and a ValueProperty implementation.
```
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
        /// The Main constructor for an email address
        /// </summary>
        /// <param name="emailAddressValue"></param>
        /// <exception cref="InvalidCastException">This is thrown if the string passed in is not a valid email with the appropriate error.</exception>
        /// <remarks>
        /// This verifies:  
        ///     that the email is not <see langword="null"/>;
        ///     that email is at leaset 3 characters long;
        ///     that the email does not start with an @ sign;
        ///     that the email does not end with an @ sign;
        ///     that the email contains 1 and only 1 @ sign;
        /// </remarks>
        public EmailAddress(MayBe<string> emailAddressValue) : base(emailAddressValue)
        {
            Result<string> result = (emailAddressValue.HasValue ? Result.Ok<string>(emailAddressValue.Value) : Result.Fail<string>("Null email Address"))
                .OnSuccess<string>(ema => ema.Trim())
                .Ensure(ema => 3 <= ema.Length, "The email address is too short")
                .Ensure(ema => 0 < ema.IndexOf("@", StringComparison.InvariantCulture), "No preceding name before @ sign in EmailAddress")
                .Ensure(ema => ema.IndexOf("@", StringComparison.InvariantCulture) != (ema.Length - 1), "No domain name after @ sign in EmailAddress")
                .Ensure(ema => ema.Split('@').Length == 2, @"There are {ema.Split('@').Length - 1} @ signs. Should be 1.")
                .OnFailure(error => { throw new InvalidCastException(error); });
        }

        /// <summary>
        /// Implicit conversion for simpler readability to MayBe&lt;String&gt;
        /// </summary>
        /// <param name="possibleEmail"></param>
        public static implicit operator EmailAddress([AllowNull]string possibleEmail)
        {
            return new EmailAddress(new MayBe<string>(possibleEmail));
        }

        /// <summary>
        /// Implicit conversion from May&lt;Be&gt; string to be more explicit about whether the string is null.
        /// </summary>
        /// <param name="possibleEmail"></param>
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

```
---