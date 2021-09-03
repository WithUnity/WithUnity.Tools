/* Copyright 2015-2020 Robin Murison

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. */
using System;
using System.Collections.Generic;

namespace WithUnity.Tools.ValueProperties
{
    /// <summary>
    /// A C# string representing a valid UTF-16 string
    /// </summary>
    /// <remarks>
    ///     This checks that all the code points in the string form valid unicode characters there are all valid Unicode characters.
    /// </remarks>
    public sealed class UnicodeString16 : ValueProperty<UnicodeString16, string>
    {
        private const string invalidSingleHighPoint = "Invalid single high surrogate code point.";
        private const string invalidSingleLowPoint = "Invalid single low surrogate code point.";
        private const string notASinglePoint = "This is not a single Unicode character.";

        /// <summary>
        /// Constructor for validating the unicode string
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="InvalidCastException">If the string is not a Unicode string</exception>
        /// <exception cref="ArgumentNullException">If the string is null</exception>
        /// <remarks>
        /// This verifies:
        ///     That the string past in is a valid  Unicode string;
        /// By checking if  high and low surrogate code points are used in the correct order and the single code points are used as single characters.
        /// </remarks>
        public UnicodeString16(MayBe<string> value) : base(value)
        {
            if (value.HasValue)
            {
                int characterCount = 0;

                int[] utf32 = new int[value.Value.Length + 1];
                foreach (UnicodeCharacter16 uc in GetUnicodeCharacter(value))
                {
                    utf32[characterCount++] = uc.Utf32;
                }
                Utf32String = new int[characterCount];
                Array.Copy(utf32, 0, Utf32String, 0, characterCount);
            }
        }

        /// <summary>
        /// Goes through a string returning valid Unicode characters
        /// </summary>
        /// <param name="input">A MayBe&lt;string&gt; input string.</param>
        /// <returns>The next valid Unicode Character</returns>
        /// <exception cref="InvalidCastException">Is thrown if the string is invalid Unicode</exception>
        public IEnumerable<UnicodeCharacter16> GetUnicodeCharacter(MayBe<string> input)
        {
            int offset = 0;
            if (input.HasValue)
            {
                string value = input.Value;
                do
                {
                    // Throw exeption when starting character with a low surrogate
                    if (System.Char.IsLowSurrogate(value[offset])) throw new InvalidCastException(invalidSingleLowPoint);

                    // The general case where high surrogates are still valid.
                    if (offset < value.Length - 1)
                    {
                        if (System.Char.IsHighSurrogate(value[offset]))
                        {
                            if (System.Char.IsLowSurrogate(value[offset + 1]))
                            {
                                // return the two codepoints
                                offset += 2;
                                yield return new UnicodeCharacter16(value.Substring(offset - 2, 2));
                            }
                            throw new InvalidCastException(invalidSingleHighPoint);
                        }
                        else
                        {
                            // return a single codepoint
                            offset += 1;
                            yield return new UnicodeCharacter16(value.Substring(offset - 1, 1));
                        }
                    }
                    else
                    {
                        // The last code point in the string multi codepoint characters are invalid.
                        if (System.Char.IsHighSurrogate(value[offset]))
                        {
                            throw new InvalidCastException(invalidSingleHighPoint);
                        }
                        if (System.Char.IsLowSurrogate(value[offset]))
                        {
                            throw new InvalidCastException(invalidSingleLowPoint);
                        }
                        offset += 1;
                        yield return new UnicodeCharacter16(value.Substring(offset - 1, 1));
                    }
                } while (offset < value.Length);
            }
        }

        /// <summary>
        /// Provides Utf32 equivalent.
        /// </summary>
        public Int32[] Utf32String { get; set; }

        /// <summary>
        /// Provides Utf32 equivalent.
        /// </summary>
        public byte[] Utf8String { get; set; }
    }
}
