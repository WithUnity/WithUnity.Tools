/* Copyright 2015-2021 Robin Murison

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. */
using System;

namespace WithUnity.Tools.Tests
{
    /// <summary>
    /// Due to string.Copy() being made obsolete we need to reintroduce it for testing
    /// </summary>
    public static class StringExtension
    {
        /// <summary>
        /// Renamed Copy function to provide a string with a different reference
        /// </summary>
        /// <param name="input"></param>
        /// <returns>A string with a different reference</returns>
        /// <remarks>Ensures comparison of strings compares the contents not the reference.</remarks>
        public static String Duplicate(this string input)
        {
            return input != null ? new string(input.ToCharArray()) : null;
        }
    }
}
