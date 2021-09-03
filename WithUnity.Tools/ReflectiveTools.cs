/* Copyright 2015-2021 Robin Murison

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace WithUnity.Tools
{
    /// <summary>
    /// Refelctive tools for accessing types indirectly
    /// </summary>
    public static partial class ReflectiveTools
    {
        /// <summary>
        /// CurrentMethod returns the name of the method or property calling it.
        /// </summary>
        /// <returns>The name of the calling method or property.</returns>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static string CurrentMethod()
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(1);
            return sf.GetMethod().Name;
        }

        /// <summary>
        /// CallingMethod returns the name of parent method or property that are calling this function.
        /// </summary>
        /// <param name="stackFrames">The number of ancestors to use. Defaults to one.</param>
        /// <returns>The name of the calling method or property.</returns>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static string CallingMethod(int stackFrames = 1)
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(stackFrames);
            string name = sf.GetMethod().Name;
            return name;
        }

        /// <summary>
        /// Gets the column number in the file that contains the code that is executing.
        /// This information is typically extracted from the debugging symbols for the executable.
        /// </summary>
        /// <param name="stackFrames"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static int CallingFileColumnNumber(int stackFrames = 1)
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(stackFrames);
            return sf.GetFileColumnNumber();
        }

        /// <summary>
        /// Gets the line number in the file that contains the code that is executing.
        /// This information is typically extracted from the debugging symbols 
        /// for the executable.  This call may be several calls deep
        /// </summary>
        /// <param name="stackFrames"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static int CallingFileLineNumber(int stackFrames = 1)
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(stackFrames);
            return sf.GetFileLineNumber();
        }

        /// <summary>
        /// Gets the file in which this method is being called from.  This call may be several calls deep
        /// </summary>
        /// <param name="stackFrames">The numbers of levels deep before we report the name.</param>
        /// <returns>The file name</returns>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static string CallingFileName(int stackFrames = 1)
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(stackFrames);
            return sf.GetFileName();
        }

        /// <summary>
        /// While this method has an optional parameter it should never be used
        /// </summary>
        /// <param name="methodName">Must all ways be null.</param>
        /// <returns>The name of the calling method or the input string.</returns>
        /// <remarks>This uses standard tools to achieve the same effect as CurrentMethod
        /// but it can be fooled if someone actually uses the optional parameter.</remarks>
        public static string UnsafeCurrentMethod([CallerMemberName] string methodName = null)
        {
            return methodName;
        }
        ///// <param name="callingObject">The object on which this class was called</param>
        // TODO Tidyup CallingObject correctly and correct Unit Testing
        /// <summary>
        /// Finds a method which has the same number of Generic type parameters
        /// the same return type and the same parameters including whether they 
        /// match the expected generic type
        /// </summary>
        /// <param name="method">The Method this method is checked against</param>
        /// <param name="returnType">The expected return Type.  This may be a generic type</param>
        /// <param name="types">A list of generic types this method should be called with.</param>
        /// <param name="parameters">A list of parameters this shoulf be called with.</param>
        /// <returns>True if we find a match, false otherwise.</returns>
        public static bool TestFoundMethod(/*object callingObject, */MethodInfo method, Type returnType, Type[] types, object[] parameters)
        {
            return FoundMethod(/*callingObject, */method, returnType, types, parameters);
        }

        ///// <param name="callingObject">The object on which this class was called</param>
        // TODO manage this properly with reworked Unit Tests
        /// <summary>
        /// Finds a method which has the same number of Generic type parameters
        /// the same return type and the same parameters including whether they 
        /// match the expected generic type
        /// </summary>
        /// <param name="method">The Method this method is checked against</param>
        /// <param name="returnType">The expected return Type.  This may be a generic type</param>
        /// <param name="types">A list of generic types this method should be called with.</param>
        /// <param name="parameters">A list of parameters this shoulf be called with.</param>
        /// <returns>True if we find a match, false otherwise.</returns>
        private static bool FoundMethod(/*object callingObject, */MethodInfo method, Type returnType, Type[] types, object[] parameters)
        {
            var methodReturnType = method.ReturnType;

            if (methodReturnType.IsGenericParameter)
            {
                if (types[methodReturnType.GenericParameterPosition] != methodReturnType)
                    return false;
            }
            if (returnType != methodReturnType)
            {
                return false;
            }

            var methodParameters = method.GetParameters();
            if (methodParameters.Length != parameters.Length)
            {
                return false;
            }

            for (int i = 0; i < methodParameters.Length; i++)
            {
                var methodParameter = methodParameters[i];
                if (methodParameter.ParameterType.FullName == null)
                {
                    if (types[methodParameter.ParameterType.GenericParameterPosition] != parameters[i].GetType())
                    {
                        return false;
                    }
                }
                else
                {
                    if (methodParameters[i].ParameterType != parameters[i].GetType())
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Call a generic function by passing in a single Type as 
        /// a parameter and the parameters as an array of objects.
        /// </summary>
        /// <param name="callingObject">The object calling this method</param>
        /// <param name="t">The Type to use</param>
        /// <param name="parameters">An array of parameters</param>
        /// <param name="methodName">The name of the method to call. Defaults to the same name as the calling method</param>
        /// <returns>a generic object the return value of the method</returns>
        public static void VoidCall(this object callingObject, Type t, object[] parameters, [CallerMemberName] string methodName = null)
        {
            if (string.IsNullOrWhiteSpace(methodName))
            {
                methodName = CurrentMethod();
            }
            Type[] types = new Type[] { t };
            VoidCall(callingObject, types, parameters, methodName);
        }

        /// <summary>
        /// Call a generic function by passing in the Type as 
        /// a parameter
        /// 
        /// </summary>
        /// <param name="callingObject">The object calling this method</param>
        /// <param name="types">The Types to use</param>
        /// <param name="parameters">An array of parameters</param>
        /// <param name="methodName">The name of the method to call. Defaults to the same name as the calling method</param>
        /// <returns>a generic object the return value of the method</returns>
        private static void VoidCall(this object callingObject, Type[] types, object[] parameters, [CallerMemberName] string methodName = null)
        {
            if (string.IsNullOrWhiteSpace(methodName))
            {
                StackTrace st = new StackTrace();
                StackFrame sf = st.GetFrame(1);
                methodName = sf.GetMethod().Name;
            }
            ((System.Reflection.TypeInfo)(callingObject.GetType())).DeclaredMethods.Where(m => m.Name == methodName) // get a list of all methods with that name
                .Where(m1 => m1.IsGenericMethod == true) // Get only generic methods of that name
                .Where(m1 => m1.GetGenericArguments().Length == types.Length) // Get generic methods with the right number of Type parameters
                .Where(m1 => m1.GetParameters().Length == parameters.Length) // Get generic methods with the right number of Type parameters
                .Single(m1 => FoundMethod(/*callingObject, */ m1, typeof(void), types, parameters)) // get the method with an exact match of parameters.
                .MakeGenericMethod(types) // Set the type to the types passed in.
                .Invoke(callingObject, parameters); // Then call the generic version with Type t.

        }

        /// <summary>
        /// Call a generic function by passing in a single Type as 
        /// a parameter and the parameters as an array of objects.
        /// </summary>
        /// <param name="callingObject">The object calling this method</param>
        /// <param name="returnType">The return type of the method to call</param>
        /// <param name="t">The Type to use</param>
        /// <param name="parameters">An array of parameters</param>
        /// <param name="methodName">The name of the method to call. Defaults to the same name as the calling method</param>
        /// <returns>a generic object the return value of the method</returns>
        public static Result<object> Call(this object callingObject, Type returnType, Type t, object[] parameters, [CallerMemberName] string methodName = null)
        {
            if (string.IsNullOrWhiteSpace(methodName))
            {
                methodName = CurrentMethod();
            }
            Type[] types = new Type[] { t };
            return Call(callingObject, returnType, types, parameters, methodName);
        }

        /// <summary>
        /// Call a generic function by passing in the Type as 
        /// a parameter
        /// 
        /// </summary>
        /// <param name="callingObject">The object calling this method</param>
        /// <param name="returnType">The type returned</param>
        /// <param name="types">The Types to use</param>
        /// <param name="parameters">An array of parameters</param>
        /// <param name="methodName">The name of the method to call. Defaults to the same name as the calling method</param>
        /// <returns>a generic object the return value of the method</returns>
        private static Result<object> Call(this object callingObject, Type returnType, Type[] types, object[] parameters, [CallerMemberName] string methodName = null)
        {
            if (string.IsNullOrWhiteSpace(methodName))
            {
                StackTrace st = new StackTrace();
                StackFrame sf = st.GetFrame(1);
                methodName = sf.GetMethod().Name;
            }
            return Result<object>.Ok(((System.Reflection.TypeInfo)(callingObject.GetType())).DeclaredMethods.Where(m => m.Name == methodName) // get a list of all methods with that name
                .Where(m1 => m1.IsGenericMethod == true) // Get only generic methods of that name
                .Where(m1 => m1.GetGenericArguments().Length == types.Length) // Get generic methods with the right number of Type parameters
                .Where(m1 => m1.GetParameters().Length == parameters.Length) // Get generic methods with the right number of Type parameters
                .Single(m1 => FoundMethod(/* callingObject, */ m1, returnType, types, parameters)) // get the method with an exact match of parameters.
                .MakeGenericMethod(types) // Set the type to the types passed in.
                .Invoke(callingObject, parameters)); // Then call the generic version with Type t.

        }

        // Searching for loaded Assemblies

        /// <summary>
        /// Get the Short name from the long name.
        /// First excludes the full path with versions and keys
        /// Then exludes the assembly informaiton.
        /// </summary>
        /// <param name="fullName"></param>
        /// <returns></returns>
        public static string ShortName(this string fullName)
        {
            string fullClassName = fullName.Split(',')[0];
            string[] classNameParts = fullClassName.Split('.');
            return classNameParts[classNameParts.Length - 1];
        }


        /// <summary>
        ///     Intent: Get assemblies referenced by entry assembly. Not recursive.
        /// </summary>
        public static List<string> GetReferencedAssembliesFlat(this Type type)
        {
            var results = type.Assembly.GetReferencedAssemblies();
            return results.Select(o => o.FullName).OrderBy(o => o).ToList();
        }

        /// <summary>
        ///     Intent: Get assemblies the entry assembly currently needs.
        /// </summary>
        public static Dictionary<string, Assembly> GetLoadedAssemblies(this Assembly assembly)
        {
            return GetLoadedAssemblies(assembly, out _);
        }


        /// <summary>
        ///     Intent: Get assemblies the entry assembly currently needs and any assemblies that have not been found.
        /// </summary>
        public static Dictionary<string, Assembly> GetLoadedAssemblies(this Assembly assembly, out IDictionary<string, string> missingAssemblyList)
        {
            Dictionary<string, Assembly> dependentAssemblyList;

            dependentAssemblyList = new Dictionary<string, Assembly>();
            missingAssemblyList = new Dictionary<string, string>();

            assembly.GetDependentAssemblies(dependentAssemblyList, missingAssemblyList);

            /*
            // Removing check for GlobalAssemblyCache as now obsolete on cross platform scenarios.
            // Only include assemblies that we wrote ourselves (ignore ones from GAC).
            var keysToRemove = dependentAssemblyList.Values.Where(
                o => o.GlobalAssemblyCache).ToList();

            foreach(var k in keysToRemove)
            {
                dependentAssemblyList.Remove(k.FullName.ShortName());
            }
            */

            return dependentAssemblyList;
        }


        /// <summary>
        ///     Intent: Internal recursive class to get all dependent assemblies, and all dependent assemblies of
        ///     dependent assemblies, etc.
        /// </summary>
        private static void GetDependentAssemblies(this Assembly assembly, 
                                                   IDictionary<string, Assembly> dependentAssemblyList,
                                                   IDictionary<string, string> missingAssemblyList)
        {
            // Load assemblies with newest versions first. Omitting the ordering results in false positives on
            // _missingAssemblyList.
        var referencedAssemblies = assembly.GetReferencedAssemblies()
                .OrderByDescending(o => o.Version);

            foreach(var r in referencedAssemblies)
            {
                if(String.IsNullOrEmpty(assembly.FullName))
                {
                    continue;
                }

                if(!dependentAssemblyList.ContainsKey(r.FullName.ShortName()))
                {
                    try
                    {
                        var a = Assembly.ReflectionOnlyLoad(r.FullName);
                        dependentAssemblyList[a.FullName.ShortName()] = a;
                        a.GetDependentAssemblies(dependentAssemblyList,
                                                 missingAssemblyList);
                    }
                    catch
                    {
                        missingAssemblyList.Add(r.FullName.Split(',')[0], assembly.FullName.ShortName());
                    }
                }
            }
        }

        /// <summary>
        ///     Intent: Get missing assemblies.
        /// </summary>
        public static IDictionary<string, string> MyGetMissingAssembliesRecursive(this Assembly assembly)
        {
            IDictionary<string, string> missingAssemblies = new Dictionary<string,string>();
            assembly.GetDependentAssemblies(new Dictionary<string, Assembly>(),
                                            missingAssemblies);
            return missingAssemblies;
        }
    }
}
