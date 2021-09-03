/* Copyright 2015-2020 Robin Murison

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. */
using NullGuard;
using System;

namespace WithUnity.Tools
{
    /// <summary>
    /// A ValueProperty is a ValueObject which has only one
    /// Property.
    /// 
    /// Like ValueObjects ValueProperties must be Immutable.
    /// Hence their name.
    /// 
    /// A ValueProperty contain a single object for example a string
    /// with all the validation of that value
    /// and Equals and GetHashCode methods.
    /// 
    ///The reason a ValueProperty is different from a ValueObject
    ///
    /// Is that it adds implicit assignment between a value and the ValueProperty
    /// and implicit assignment between a MayBe&lt;Value&gt; and the ValueProperty
    /// 
    /// It also allows Comparisons between the held value and the native value or a MayBe&lt;value&gt;
    /// 
    /// The code here give the standard base implementation of
    /// a ValueProperty
    /// 
    /// The various Equals and GetHashCode methods, while 
    /// keeping to the DRY principal as best we can.
    /// </summary>
    /// <typeparam name="T">The Value Property type e.g. public sealed class EmailAddress : ValueProperty&lt;EmailAddress, string&gt;</typeparam>
    /// <typeparam name="HeldType">The Type of the held value; for a EmailAddress that is a string</typeparam>
    public class ValueProperty<T, HeldType>
        where T : ValueProperty<T, HeldType> where HeldType : class
    {
        /// <summary>
        /// The underlying read only data. Maybe&lt;HeldType&gt; may be.
        /// </summary>
        public MayBe<HeldType> Value { get; protected set; }

        /// <summary>
        /// A protected constructor.
        /// It is not intended that this should be used directly
        /// but always as an implicit cast.
        /// </summary>
        /// <param name="value">An instance of the underlying data. 
        /// This is never expected to be null(Protected by NullGuard)</param>
        protected ValueProperty(HeldType value)
        {
            Value = value;
        }

        /// <summary>
        /// A protected constructor.
        /// It is not intended that this should be used directly
        /// but always as an implicit cast.
        /// </summary>
        /// <param name="value">A MayBe&lt;HeldType&gt; structure, which may contain an instance of the underlying data or null.</param>
        protected ValueProperty(MayBe<HeldType> value)
        {
            Value = value;
        }

#pragma warning disable CA1000 // Do not declare static members on generic types
        // FxCop specifically asked me to add this static method
        /// <summary>
        /// Explicit Conversion from HeldType to  ValueProperty&lt;T, HeldType>&gt;
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static ValueProperty<T, HeldType> ToValueProperty(HeldType value)
#pragma warning restore CA1000 // Do not declare static members on generic types
        {
            return new ValueProperty<T, HeldType>(value);
        }

        /// <summary>
        /// Implicit cast from a HeldType instance to ValueProperty&lt;T, HeldType&gt;.
        /// </summary>
        /// <param name="value">The incoming value</param>
        /// <returns>The implicitly cast ValueProperty(Must pass validation or throw an exception)</returns>
        public static implicit operator ValueProperty<T, HeldType>(HeldType value)
        {
            return new ValueProperty<T, HeldType>(value);
        }

#pragma warning disable CA1000 // Do not declare static members on generic types
        // FxCop specifically asked me to add this static method
        /// <summary>
        /// Explicit Conversion from HeldType to  ValueProperty&lt;T, HeldType>&gt;
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static ValueProperty<T, HeldType> ToValueProperty(MayBe<HeldType> value)
#pragma warning restore CA1000 // Do not declare static members on generic types
        {
            return new ValueProperty<T, HeldType>(value.Value);
        }

        /// <summary>
        /// Implicit cast from a MayBe&lt;HeldType&gt; to ValueProperty&lt;T, HeldType&gt;.
        /// </summary>
        /// <param name="value">The incoming value</param>
        /// <returns>The implicitly cast ValueProperty(Must pass validation or throw an exception)</returns>
        public static implicit operator ValueProperty<T, HeldType>(MayBe<HeldType> value)
        {
            return new ValueProperty<T, HeldType>(value);
        }

        /// <summary>
        /// Returns true if equal or false if not equal
        /// </summary>
        /// <param name="obj">The object being compared</param>
        /// <returns>Returns Whether the Value of the ValueProperty is equal to:
        /// 1. if the obj is HeldType the obj;
        /// 2. if the obj is a MayBe&lt;HeldType&gt; the Value of the MayBe&lt;HeldType&gt;;
        /// 3. if the obj is a a ValueProperty&lt;T, HeldType&gt; the Value of the ValueProperty&lt;T, HeldType&gt;
        /// 4. or false.</returns>
        public override bool Equals([AllowNull]object obj)
        {
            if (Value.HasNoValue && obj is null)
                return true;

            HeldType other = null;
            if (obj is HeldType)
            {
                other = obj as HeldType;
            }
            if (obj is MayBe<HeldType>)
            {
                if (((MayBe<HeldType>)obj).HasValue)
                {
                    other = ((MayBe<HeldType>)obj).Value;
                }
            }
            if (obj is ValueProperty<T, HeldType>)
            {
                ValueProperty<T, HeldType> temp = (obj as ValueProperty<T, HeldType>);

                if (temp.Value.HasValue)
                {
                    other = temp.Value.Value;
                }
            }
            if (Value.HasNoValue)
                return other == null;

            return EqualsCore(other);
        }

        /// <summary>
        /// This should be overrriden
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        protected virtual bool EqualsCore(HeldType other)
        {
            // Value is a MayBe<HeldType>
            // Value.Value may be null
            if (Value.HasNoValue)
                return null == other; // Should always false due to NullGuard.

            return Value.Value.Equals(other);
        }

        /// <summary>
        /// GetHashCode calls an overridden method to Get the Hash code based on the HeldType. 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        /// <summary>
        /// == operator for ValueProperty&lt;T, HeldType&gt;
        /// </summary>
        /// <param name="a">a ValueProperty&lt;T, HeldType&gt;</param>
        /// <param name="b">another ValueProperty&lt;T, HeldType&gt;</param>
        /// <returns>true if the two Values are the same</returns>
        public static bool operator ==(ValueProperty<T, HeldType> a, ValueProperty<T, HeldType> b)
        {
            if (a is null && b is null)
                return true;

            if (a is null || b is null)
                return false;

            return a.Equals(b);
        }

        /// <summary>
        /// != operator for ValueProperty&lt;T, HeldType&gt;
        /// </summary>
        /// <param name="a">a ValueProperty&lt;T, HeldType&gt;</param>
        /// <param name="b">another ValueProperty&lt;T, HeldType&gt;</param>
        /// <returns>false if the two Values are the same</returns>
        public static bool operator !=(ValueProperty<T, HeldType> a, ValueProperty<T, HeldType> b)
        {
            return !(a == b);
        }

        /// <summary>
        /// == operator comparing the Value of a ValueProperty&lt;T, HeldType&gt; and the Value of a MayBe&lt;HeldType&gt;
        /// </summary>
        /// <param name="a">a ValueProperty&lt;T, HeldType&gt;</param>
        /// <param name="b">a MayBe&lt;HeldTypey&gt; of the same type</param>
        /// <returns>true if the HeldType Value for both are equal</returns>
        public static bool operator ==(ValueProperty<T, HeldType> a, MayBe<HeldType> b)
        {
            if (a is null)
                return false;

            if (a.Value.HasNoValue && b.HasNoValue)
                return true;

            if (a.Value.HasNoValue || b.HasNoValue)
                return false;

            return a.Value.Equals(b.Value);
        }

        /// <summary>
        /// == operator comparing the Value of a ValueProperty&lt;T, HeldType&gt; and the Value of a MayBe&lt;HeldType&gt;
        /// </summary>
        /// <param name="a">a ValueProperty&lt;T, HeldType&gt;</param>
        /// <param name="b">a MayBe&lt;HeldTypey&gt; of the same type</param>
        /// <returns>false if the HeldType Value for both are equal</returns>
        public static bool operator !=(ValueProperty<T, HeldType> a, MayBe<HeldType> b)
        {
            return !(a == b);
        }

        /// <summary>
        /// == operator comparing a MayBe&lt;HeldType&gt; and a ValueProperty&lt;T, HeldType&gt; 
        /// </summary>
        /// <param name="a">a MayBe&lt;HeldType&gt;</param>
        /// <param name="b">a ValueProperty&lt;T, HeldType&gt;</param>
        /// <returns>true if the HeldType Value for both are equal</returns>
        public static bool operator ==(MayBe<HeldType>  a, ValueProperty<T, HeldType> b)
        {
            return b == a;
        }

        /// <summary>
        /// != operator comparing a MayBe&lt;HeldType&gt; and a ValueProperty&lt;T, HeldType&gt; 
        /// </summary>
        /// <param name="a">a MayBe&lt;HeldType&gt;</param>
        /// <param name="b">a ValueProperty&lt;T, HeldType&gt;</param>
        /// <returns>false if the HeldType Value for both are equal</returns>
        public static bool operator !=(MayBe<HeldType>  a, ValueProperty<T, HeldType> b)
        {
            return !(b == a);
        }

        /// <summary>
        /// == operator comparing a ValueProperty&lt;T, HeldType&gt; and a HeldType
        /// </summary>
        /// <param name="a">a ValueProperty&lt;T, HeldType&gt;</param>
        /// <param name="b">HeldType of the same type as the MayBe&lt;HeldType&gt;.Value</param>
        /// <returns>true if the value of HeldType is e qual or both are null</returns>
        public static bool operator ==([AllowNull] ValueProperty<T, HeldType> a, [AllowNull] HeldType b)
        {
            if (a is null) // the Value Property should never be null
                return false;

            if (a.Value.HasNoValue)
                return b == null;

            return a.Equals(b);
        }

        /// <summary>
        /// != operator comparing a ValueProperty&lt;T, HeldType&gt; and a HeldType
        /// </summary>
        /// <param name="a">a ValueProperty&lt;T, HeldType&gt;</param>
        /// <param name="b">HeldType of the same type</param>
        /// <returns>true if the calue of HeldType is e qual or both are null</returns>
        public static bool operator !=([AllowNull] ValueProperty<T, HeldType> a, [AllowNull] HeldType b)
        {
            return !(a == b);
        }

        /// <summary>
        /// == operator comparing a HeldType and a ValueProperty&lt;T, HeldType&gt;
        /// </summary>
        /// <param name="a">HeldType of the same type</param>
        /// <param name="b">a ValueProperty&lt;T, HeldType&gt;</param>
        /// <returns>true if the calue of HeldType is e qual or both are null</returns>
        public static bool operator ==([AllowNull] HeldType a, [AllowNull] ValueProperty<T, HeldType> b)
        {
            return b == a;
        }

        /// <summary>
        /// != operator comparing a HeldType and a ValueProperty&lt;T, HeldType&gt;
        /// </summary>
        /// <param name="a">HeldType of the same type</param>
        /// <param name="b">a ValueProperty&lt;T, HeldType&gt;</param>
        /// <returns>true if the calue of HeldType is e qual or both are null</returns>
        public static bool operator !=([AllowNull] HeldType a, [AllowNull] ValueProperty<T, HeldType> b)
        {
            return !(b == a);
        }
    }
}