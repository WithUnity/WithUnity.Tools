/* Copyright 2015-2020 Robin Murison

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. */
using System;
using System.Text;

namespace WithUnity.Tools.ValueProperties
{
    /// <summary>
    /// A string representing one Unicode Character based on 1 or 2 16 bit Unicode code points
    /// </summary>
    /// <remarks>
    ///     This is provided with minimal validataion for a uunicode character. You may want to create your own variant with different validation.
    /// </remarks>
    public sealed class UnicodeCharacter16 : ValueProperty<UnicodeCharacter16, string>
    {

        /// <summary>
        /// Validates a single UTF16 character this may be 1 or 2 16 bit code points
        /// </summary>
        /// <param name="input">The inpout to verify</param>
        /// <remarks>
        /// This verifies that:  
        ///     the email is not <see langword="null"/>;
        ///     email is at leaset 3 characters long;
        ///     the email does not start with an @ sign;
        ///     the email does not end with an @ sign;
        ///     the email contains 1 and only 1 @ sign;
        ///     the email does not need trimming.
        /// </remarks>
        public static Result<string> ValidateUnicodeCharacter16(MayBe<string> input)
        {
            const string invalidSingleHighPoint = "Invalid single high surrogate code point.";
            const string invalidSingleLowPoint = "Invalid single low surrogate code point.";
            const string notASinglePoint = "This is not a single Unicode character.";
            Result<string> result = Result.Initialize(input, "Null string is not Unicode Character")
                .Ensure(value => 1 <= value.Length && value.Length <= 2, $"A Unicode 16 character should contain  1 or 2 code points. This has {input.Value.Length}.")
                .Ensure(value => !Char.IsHighSurrogate(value[0]), invalidSingleHighPoint);
            if (result.IsSuccess)
            {
                if(input.Value.Length == 1)
                {
                    return result.Ensure(value => value[0] != 0xfffe && value[0] != 0xffff, $"Disallowed end code point {(int)input.Value[0]:X}")
                        .Ensure(value => !Char.IsLowSurrogate(value[0]), invalidSingleLowPoint);

                }
                else
                {
                    return result.Ensure(value => !Char.IsLowSurrogate(value[1]), invalidSingleLowPoint);
                }
            }
            return result;
                
        }

        /// <summary>
        /// Constructor for validating the unicode character
        /// </summary>
        /// <param name="input">Maybe a single Unicode Character in a string</param>
        /// <exception cref="InvalidCastException">If the string is not a Unicode character</exception>
        /// <exception cref="ArgumentNullException">If the string is null</exception>
        /// <remarks>
        /// This verifies:
        ///     That the string past in is a single Unicode Character;
        /// Whether high and low surrogate code points are used in the correct order and 
        /// 
        /// </remarks>
        /// <exception cref="InvalidCastException">If the string is not a single Unicode character, Including a null string.</exception>
        public UnicodeCharacter16(MayBe<string> input) : base(input)
        {
            const string invalidSingleHighPoint = "Invalid single high surrogate code point.";
            const string invalidSingleLowPoint = "Invalid single low surrogate code point.";
            const string notASinglePoint = "This is not a single Unicode character.";
            if (input.HasValue)
            {
                string value = input.Value;
                if (value.Length < 1 || 2 < value.Length)
                {
                    throw new InvalidCastException($"A Unicode 16 character should contain  1 or 2 code points. This has {value.Length}.");
                }
                if (value.Length == 1)
                {
                    if (value[0] == 0xfffe || value[0] == 0xffff)
                    {
                        throw new InvalidCastException($"Disallowed end code point {(int)value[0]:X}");
                    }
                    if (Char.IsHighSurrogate(value[0]))
                    {
                        throw new InvalidCastException(invalidSingleHighPoint);
                    }
                    if (Char.IsLowSurrogate(value[0]))
                    {
                        throw new InvalidCastException(invalidSingleLowPoint);
                    }
                    Value = value;
                    Utf32 = value[0];
                    Utf8 = Encoding.UTF8.GetBytes(value);
                    return;
                }
                if (value.Length == 2)
                {
                    if (System.Char.IsHighSurrogate(value[0]))
                    {
                        if (System.Char.IsLowSurrogate(value[1]))
                        {
                            Value = value;
                            Utf32 = Char.ConvertToUtf32(value[0], value[1]);
                            Utf8 = Encoding.UTF8.GetBytes(value);
                            return;
                        }
                        throw new InvalidCastException(invalidSingleLowPoint);
                    }
                    throw new InvalidCastException(invalidSingleHighPoint);
                }
                throw new InvalidCastException(notASinglePoint);
            }
        }

        /// <summary>
        /// Utf32 equivalent
        /// </summary>
        public Int32 Utf32 { get; set; }

        /// <summary>
        /// The UTF 8 equivalent.
        /// </summary>
        public byte[] Utf8 { get; set; }
    }
}
