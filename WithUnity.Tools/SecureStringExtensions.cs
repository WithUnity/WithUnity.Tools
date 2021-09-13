/* Copyright 2015-2021 Robin Murison

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. */
using NullGuard;
using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;

namespace WithUnity.Tools
{
    using System.Globalization;

    /// <summary>
    /// SecureString extension methods
    /// and related string extension methods
    /// </summary>
    public static class SecureStringExtensions
    {
        /// <summary>
        /// Wipes the memory of an insecure string setting the characters to 0.
        /// Users should set the length to 0 after a call to WipeString
        /// to better hide the previous values length assign a new value to the 
        /// immutable reference and allow GC to dispose the nulled strigns. 
        /// See examples of use below.
        /// </summary>
        /// <param name="insecurePassword">a plain text string that needs to be wiped clean before being released back to general memory</param>
        /// <returns></returns>
        public static unsafe void WipeString(this string insecurePassword)
        {
            fixed (char* cInsecurePassword = insecurePassword)
            {
                for (int i = 0; i < insecurePassword.Length; i++)
                {
                    cInsecurePassword[i] = (char)0;
                }
            }
        }

        /// <summary>
        /// Allows the use of the obfuscated contents in a function that returns a Result with no risk of it being written to a swap file and or being copied about in memory.
        /// The text is filled with NUL characters before releasing it and is by default garbage collected.
        /// </summary>
        /// <param name="securePassword">The hidden data</param>
        /// <param name="parameters">An array of parameters that are used by the function</param>
        /// <param name="func">The function you want to use the hidden data in</param>
        /// <param name="collectGarbage">Do you want to Collect the Garbage or are you more worried about performance</param>
        /// <returns>A Result as to whether the function was successful including the obj paramter after any changes that took place.</returns>
        /// <remarks>The returned result may contain the object</remarks>
        public unsafe static Result UseContents(this SecureString securePassword, object[] parameters, Func<object[], string, Result> func, bool collectGarbage = true)
        {
            string unsecureString;
            Result result = Result.Fail("Function not called.")
                .Ensure(securePassword != null, "Null securePassword");

            // securePassword.ConvertToUnsecureString() only returns null if securePassword is null which is blocked here by NullGuard.
            fixed (char* cp = unsecureString = securePassword.ConvertToUnsecureString().Value)
            /* cp is a pointer amd is intrinsically unsafe and should never be used
             * unless absolutely necessary. However, it is needed for the fixed
             * statement to work. This fixes the managed string in memory.
             * This stops it from ever being written to a swap file.
             * And it minimizes the number of times it appears in memory to one
             * and so the number of times it could appear in a memory dump. 
             * If the program crashed at this point.
             */
            {
                try
                {
                    result = func(parameters, unsecureString);
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                }
                finally
                {
                    unsecureString.WipeString();
                }
            }

            unsecureString = null;
            /* The unsecureString is assigned null to release its previous value and the value null is intentionally never used. */
            if (collectGarbage)
            {
                GC.Collect();
            }
            return result;
        }

        /// <summary>
        /// Allows the use of the obfuscated contents in a function that returns a Result with no risk of it being written to a swap file and or being copied about in memory.
        /// The text is filled with NUL characters before releasing it and is by default garbage collected.
        /// </summary>
        /// <param name="securePassword">The hidden data</param>
        /// <param name="parameter">An array of parameters that are used by the function</param>
        /// <param name="func">The function you want to use the hidden data in</param>
        /// <param name="collectGarbage">Do you want to Collect the Garbage or are you more worried about performance</param>
        /// <returns>A Result as to whether the function was successful including the obj paramter after any changes that took place.</returns>
        /// <remarks>The returned result may contain the object</remarks>
        public unsafe static Result<T> UseContents<T>(this SecureString securePassword, T parameter, Func<T, string, Result<T>> func, bool collectGarbage = true)
        {
            string unsecureString;
            Result<T> result = Result.Fail<T>("Function not called.");
            fixed (char* cp = unsecureString = securePassword.ConvertToUnsecureString().Value)
            /* cp is a pointer amd is intrinsically unsafe and should never be used
             * unless absolutely necessary. However, it is needed for the fixed
             * statement to work. This fixes the managed string in memory.
             * This stops it from ever being written to a swap file.
             * And it minimizes the number of times it appears in memory to one
             * and so the number of times it could appear in a memory dump. 
             * If the program crashed at this point.
             */
            {
                try
                {
                    result = func(parameter, unsecureString);
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                }
                finally
                {
                    unsecureString.WipeString();
                }
            }
#pragma warning disable IDE0059 // Value assigned to symbol is never used
            unsecureString = null;
            /* The unsecureString is assigned null to release its previous value to garbager and the value null is intentionally never used. */
#pragma warning restore IDE0059 // Value assigned to symbol is never used
            if (collectGarbage)
            {
                GC.Collect();
            }
            return result;
        }

        /// <summary>
        /// Allows the use of the obfuscated contents in a function that returns a Result with no risk of it being written to a swap file and or being copied about in memory.
        /// The text is filled with NUL characters before releasing it and is by default garbage collected.
        /// </summary>
        /// <param name="securePassword">The hidden data</param>
        /// <param name="func">The function you want to use the hidden data in</param>
        /// <param name="collectGarbage">Do you want to Collect the Garbage or are you more worried about performance</param>
        /// <returns>A Result as to whether the function was successful</returns>
        public unsafe static Result UseContents(this SecureString securePassword, Func<string, Result> func, bool collectGarbage = true)
        {
            string unsecureString;
            Result result = Result.Fail("Function not called.");
#pragma warning disable IDE0059 // Value assigned to symbol is never used
            fixed (char* cp = unsecureString = securePassword.ConvertToUnsecureString().Value)
            /* cp is a pointer amd is intrinsically unsafe and should never be used
             * unless absolutely necessary. However, it is needed for the fixed
             * statement to be used. This fixes the managed string in memory.
             * This stops it from ever being written to a swap file.
             * And it minimizes the number of times it appears in memory
             * and so the number of times it could appear in a memory dump. 
             */
#pragma warning restore IDE0059 // Value assigned to symbol is never used
            {
                try
                {
                    result = func(unsecureString);
                }
                catch(Exception ex)
                {
                    Log.Error(ex);
                }
                finally
                {
                    unsecureString.WipeString();
                }
            }
#pragma warning disable IDE0059 // Value assigned to symbol is never used
            unsecureString = null;
            /* The unsecureString is assigned null to release its previous value to garbage and the value null is intentionally never used. */
#pragma warning restore IDE0059 // Value assigned to symbol is never used
            if (collectGarbage)
            {
                GC.Collect();
            }
            return result;
        }

        /// <summary>
        /// Extracts the string hidden in the secure string.
        /// Please note using this directly it is not advisable.
        /// You need to be very careful of how you handle the revealed contents.
        /// Please use one of the UseContents methods instead.
        /// </summary>
        /// <param name="securePassword"></param>
        /// <returns>A valid MayBe&lt;string&gt;.Value containging of the contents of the SecureString 
        /// or MayBe&lt;string&gt;.HasNoValue if securePassword is null</returns>
        /// <remarks>To reduce visibility of the sensitive data in the SecureString
        /// it is best to access this within fixed memory to ensure the sensitive
        /// data remains in memory and does not get moved around in memory, 
        /// leaving a trail of sensitive data images across memory or even worse gets
        /// written to disk in a swap file or a memory dump. e.g.
        /// 
        /// unsafe void method(SecureString ss)
        /// {
        ///     string unsecureString;
        ///     fixed(char* = unsecureString = ss.ConvertToUnsecureString())
        ///     {
        ///         ... 
        ///         // do what you need to with unsecureString
        ///         ...
        ///         unsecureString.WipeString(); // WipeString clears the fixed memory with NUL.
        ///     }
        ///     // releases the nul filled string, lets garbage collection Collect it.
        ///     unsecureString = null; 
        ///     // Unfortunately the released string will always be the 
        ///     // same length as the sensitive data.
        ///     // You may want to collect the memory to remove even that information
        ///     GC.Collect();
        /// }
        /// 
        /// Unfortunately the only way to fix the memory is by assigning it to a pointer which is
        /// intrinsically unsafe. However we never use the unsafe pointer only the fact that the
        /// string is fixed in memory. This does force you to compile allowing the use of unsafe code.
        /// It is never a good idea to leaving that compiler switch on for any large projects.
        /// This is why UseContents is the prefered way of accessing the contents of a SecureString
        /// It handles the memory management for you.
        /// </remarks>
        /// <exception cref="ArgumentNullException">Could theoretically be thrown, if there is an issue with the code.</exception>
        /// <exception cref="NotSupportedException">May be thrown if run against the wrong version of .NET.</exception>
        /// <exception cref="OutOfMemoryException">If the password is huge: over half the available memory.</exception>
        public static MayBe<string> ConvertToUnsecureString([AllowNull]this SecureString securePassword)
        {
            if (securePassword == null)
                return new MayBe<string>(null);

            IntPtr unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(securePassword);
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }

        /// <summary>
        /// Creates a SHA512 hash for the text
        /// </summary>
        /// <param name="text">The text to hash</param>
        /// <returns>a 64 character hash hex encoded hash</returns>
        public static string GetStringSecurityHash(this string text)
        {
            byte[] output;
            using (SHA512 hasher = SHA512.Create())
            {
                output = hasher.ComputeHash(Encoding.UTF8.GetBytes(text));
            }
            StringBuilder sBuilder = new();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < output.Length; i++)
            {
                sBuilder = sBuilder.Append(value: output[i].ToString(format: "x2", CultureInfo.InvariantCulture));
                output[i] = 0;
            }
            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        /// <summary>
        /// SecureString.GetHashCode() returns the hash of the SecureString not the string contained in it.
        /// So two SecureStrings with the same string held inside have different hascodes.
        /// To get the same hashout for the same string in you need to extract the string and take the hash of that.
        /// </summary>
        /// <param name="securePassword">The SecureString to call  this on.</param>
        /// <returns>The strings SHA512 hash</returns>
        public /*internal*/ static string GetSecureStringSecurityHash(this SecureString securePassword)
        {
            return (securePassword.UseContents((contents) =>
            {
                return Result.Ok<string>(contents.GetStringSecurityHash());
            }) as Result<string>).Value;
        }

        /// <summary>
        /// Moves a string to a SecureString. It clears the memory of the string and leaves it blank.
        /// </summary>
        /// <param name="password"></param>
        /// <returns>A SecureString containing the string</returns>
        /// <exception cref="ArgumentNullException">Could theoretically be thrown.</exception>  
        /// <exception cref="ArgumentOutOfRangeException">Could theoretically be thrown.</exception>  
        /// <exception cref="CryptographicException">Could theoretically be thrown.</exception>  
        /// <exception cref="NotSupportedException">Could theoretically be thrown.</exception>  

        /// <remarks>Side effect the incoming string is nulled.
        /// If you want to minimise the risk of the old nulled string been spotted giving 
        /// the length of the secret in a swap 
        /// file or in a memory dump, assign a different value to the string.
        /// and GC.Collect() to free up the string.
        /// </remarks>
        public static SecureString MoveToSecureString([AllowNull]this string password)
        {
            if (password == null)
                return null;
            unsafe
            {
                fixed (char* passwordChars = password)
                {
                    var securePassword = new SecureString(passwordChars, password.Length);
                    securePassword.MakeReadOnly();
                    password.WipeString();
                    return securePassword;
                }
            }
        }

        /// <summary>
        /// Verifies if two SecureStrings Contents are equal.
        /// </summary>
        /// <param name="ss1">The first Secure String</param>
        /// <param name="ss2">The second Secure String to compare it against.</param>
        /// <param name="collectMemory">For security reasons it is best to collect memory
        /// to reduce the chance of leaving any password length information lying about in memory.
        /// Default Value is true.</param>
        /// <returns>true if the contents of the two SecureStrings are equal</returns>
        public /*internal*/ static bool EqualContents([AllowNull]this SecureString ss1, [AllowNull]SecureString ss2, bool collectMemory=true)
        {
            if (object.ReferenceEquals(ss1, ss2))
                return true;

            // Should never happen on extension method.
            if(ss1 == null)
            {
                return ss2 == null;
            }

            if(ss2 == null)
            {
                return false;
            }

            if(ss1.Length != ss2.Length)
            {
                return false;
            }

            if(ss1.GetSecureStringSecurityHash() != ss2.GetSecureStringSecurityHash())
            {
                return false;
            }

            unsafe
            {
                bool answer = true;
                // By here we lnow they both have values
                fixed (char* passwordChars1 = ss1.ConvertToUnsecureString().Value,
                             passwordChars2 = ss2.ConvertToUnsecureString().Value)
                {
                    for(int i = 0; i < ss1.Length; i++)
                    {
                        if(*(passwordChars1 + i) != *(passwordChars2 + i))
                        {
                            answer = false;
                        }
                        // We need to wipe the memory before leaving fixed memory.
                        *(passwordChars1 + i) = (char)0;
                        *(passwordChars2 + i) = (char)0;
                    }
                }
                if (collectMemory)
                {
                    GC.Collect();
                }
                return answer;
            }
        }
    }
}