/* Copyright 2015-2021 Robin Murison

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. */
using NullGuard;
using System;
// Needs Unit Test Review
namespace WithUnity.Tools
{
    /// <summary>
    /// Here to document the fact that we are expecting
    /// this refernce to allow null.
    /// 
    /// Use Fody
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct MayBe<T> : IEquatable<MayBe<T>>
        where T : class
    {
        private readonly T _Value;

        /// <summary>
        /// The Value contained by the MayBe structure.
        /// Throws an exception if their is no Value.
        /// </summary>
        /// <exception>InvalidOperationException</exception>
        public T Value
        {
            get
            {
                if (HasNoValue)
                    throw new InvalidOperationException();

                return _Value;
            }
        }

        /// <summary>
        /// Test if there is a Value to protect against accessing the Value if there is none
        /// </summary>
        public bool HasValue => _Value != null;

        /// <summary>
        /// Test if there is not a Value: usually for error logging
        /// </summary>
        public bool HasNoValue => !HasValue;

        /// <summary>
        /// Internal constructor not for general use.
        /// </summary>
        /// <param name="value"></param>
        internal MayBe([AllowNull]T value)
        {
            _Value = value;
        }

#pragma warning disable CA1000 // Do not declare static members on generic types
        /// <summary>
        /// Explicit conversion from a base value of type T and a MayBe&lt;T&gt;
        /// </summary>
        /// <remarks>This intrinsically a static function. 
        /// Instantiating another instance just to new another instance would be insane nesting 
        /// and impossible with a internal constructor.</remarks>
        // TODO: add Unit Test!
        public static MayBe<T> ToMayBe([AllowNull] T value)
#pragma warning restore CA1000 // Do not declare static members on generic types
        {
            return new MayBe<T>(value);
        }

        /// <summary>
        /// Implicit conversion from a base value of type T and a MayBe&lt;T&gt;
        /// </summary>
        /// <param name="value">The incoming value of type T</param>
        public static implicit operator MayBe<T>([AllowNull]T value)
        {
            return new MayBe<T>(value);
        }

        /// <summary>
        /// == comparison between MayBe&lt;T&gt; and T
        /// </summary>
        /// <param name="maybe">The MayBe&lt;T&gt; type value</param>
        /// <param name="value">The T type value</param>
        /// <returns>true if the Value of maybe == value</returns>
        public static bool operator ==(MayBe<T> maybe, [AllowNull]T value)
        {
            if (maybe.HasNoValue)
                return value == null;

            return maybe.Value.Equals(value);
        }

        /// <summary>
        /// != comparison between MayBe&lt;T&gt; and T
        /// </summary>
        /// <param name="maybe">The MayBe&lt;T&gt; type value</param>
        /// <param name="value">The T type value</param>
        /// <returns>false if the Value of maybe == value</returns>
        public static bool operator !=(MayBe<T> maybe, [AllowNull]T value)
        {
            return !(maybe == value);
        }

        /// <summary>
        /// == comparison between MayBe&lt;T&gt; and another MayBe&lt;T&gt;
        /// </summary>
        /// <param name="first">A MayBe&lt;T&gt; type value</param>
        /// <param name="second">Another MayBe&lt;T&gt; type value</param>
        /// <returns>first.equals(second)</returns>
        public static bool operator ==(MayBe<T> first, MayBe<T> second)
        {
            return first.Equals(second);
        }

        /// <summary>
        /// == comparison between MayBe&lt;T&gt; and another MayBe&lt;T&gt;
        /// </summary>
        /// <param name="first">A MayBe&lt;T&gt; type value</param>
        /// <param name="second">Another MayBe&lt;T&gt; type value</param>
        /// <returns>!first.equals(second)</returns>
        public static bool operator !=(MayBe<T> first, MayBe<T> second)
        {
            return !(first == second);
        }

        /// <summary>
        /// Equals comparison between this and any object
        /// </summary>
        /// <param name="obj">The object to compare against this</param>
        /// <returns>true if the Value of this MayBe == obj</returns>
        public override bool Equals([AllowNull]object obj)
        {
            if (obj == null)
            {
                return false;
            }

            T other = null;
            if (obj is T)
            {
                other = obj as T;
            }
            else
            {
                if ((obj is MayBe<T>))
                {
                    MayBe<T> maybeOther = (MayBe<T>)obj;
                    if (maybeOther.HasNoValue)
                        return _Value == null;
                        
                    other = maybeOther._Value;
                }
            }
            if(other == null)
            {
                return false;
            }
            return Equals(other);
        }

        /// <summary>
        /// returns the hash for this instance.
        /// </summary>
        /// <returns>The hash code</returns>
        public override int GetHashCode()
        {
            int hash = typeof(T).GetHashCode();
            if (HasValue)
                return hash ^= Value.GetHashCode();
            return hash;
        }

        /// <summary>
        /// Equals comparison between this and another MayBe&lt;T&gt;.
        /// </summary>
        /// <param name="other">The other MayBe&lt;T&gt; to compare against this</param>
        /// <returns>true if they are equal</returns>
        public bool Equals(MayBe<T> other)
        {
            if (HasNoValue && other.HasNoValue)
                return true;

            if (HasNoValue || other.HasNoValue)
                return false;

            return _Value.Equals(other._Value);
        }

        /// <summary>
        /// The stanard override for ToString() for a MayBe&lt;T&gt;. 
        /// </summary>
        /// <returns>The Value.ToString()</returns>
        /// <exception cref="InvalidOperationException">if there is no value</exception>
        public override string ToString()
        {
            return Value.ToString();
        }

        /// <summary>
        /// Returns the Value of the MayBe&lt;T&gt; or the default value if it is null 
        /// </summary>
        /// <returns>The Value or default(T) if it has no Value</returns>
        public T Unwrap(T defaultValue = default)
        {
            if (HasValue)
                return Value;

            return defaultValue;
        }
    }
}