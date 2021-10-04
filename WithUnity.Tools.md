# WithUnity.Tools #

## Type Log

 A class for generic logging 



---
## Type Log.Level

 Enum designating The level of logging requested and written Used Both for reading and writing logs. 



---
#### Field Log.Level.Verbose

 Write logging that is only shown if Verbose logging is turned on for reading. Read: Read all logging regardless of Level written. 



---
#### Field Log.Level.Debug

 Write logging while debugging software. Read: Reads all logging regardless of Level written. 



---
#### Field Log.Level.Information

 Information logging messages 



---
#### Field Log.Level.Warning

 Warning Logging 



---
#### Field Log.Level.Error

 Error Logs 



---
#### Field Log.Level.UnknownState

 This is for pieces of code that should never run. This extreme level of error can only occur because of bugs. e.g. a switch statement that already has a case for all the possible values of the Enum should keep a separate 'default:' case too. to log unknown states and normally should never run. This is to protect code from the situation where someone adds a new value to the enum without knowing about all the switch statements. You cannot hide UnknownState logging. You may prefer to throw an exception but that may increase the problem in production rather than containing the problem. 



>Unkown states are always logged to Visual Studio Debug as well as the Sinks requested and also throw exceptions in Debug mode



---
## Type Log.Sink

 Places you can log your errors to 



---
#### Field Log.Sink.Console

 The command line 



---
#### Field Log.Sink.Debug

 To the Visual Studio Debug output window 



---
#### Field Log.Sink.File

 To a Log file in the %TEMP% directory using the calling assembly's name + ".log" 



---
#### Field Log.LevelLabels

 Used as a Level ToString() to give strings the same length to keep the log alignment the same so that it is easier to read. 



---
#### Property Log.EntryAssemblyName

 The Entry AssemblyName 



---
#### Property Log.LogFileName

 The File name used when logging to a file. 



---
#### Property Log.LockOfLocks

 A lock for creating and individual locks for specific assemblies. 



---
#### Property Log.DefaultOutputs

 Contains the list of sinks to the default methods used to implement them 



---
#### Property Log.Outputs

 Contains a list of sinks to the methods used to implement them 



---
#### Method Log.Initialise(WithUnity.Tools.Log.Level,WithUnity.Tools.Log.Sink[],System.Boolean,System.Boolean,System.String)

 Initializas the log 

|Name | Description |
|-----|------|
|level: |The minimum level of logging to record. (Verbose < Debug < Information < Warning < Error < UnkownState)|
|sinks: |An array of sinks you want to use|
|timeOffset: |Report the time offset in the log.|
|utc: |[[|]] logs things against UTC time [[|]] logs against local system time.|
|logFileName: |Optional file name when logging to a file, the default is %TEMP%\%ApplicationAssemblyName%.log|


---
#### Method Log.SetLoggingLevel(WithUnity.Tools.Log.Level)

 To change the logging level for a certain part of the code 

|Name | Description |
|-----|------|
|newLevel: |The new minimum logging level|


---
#### Method Log.SetLogPrefixText(WithUnity.Tools.Log.Level)

 Set Log Prefix adds The appropriate dateTime or DateTimeOffset followed by the calling Assembly's name and the warning level to the message 

|Name | Description |
|-----|------|
|level: ||
**Returns**: 



---
#### Method Log.WriteToLogs(WithUnity.Tools.Log.Level,System.String)

 Writes of an accepted logging level to each of the sinks in turn. 

|Name | Description |
|-----|------|
|level: |The level of the message|
|text: ||


---
#### Method Log.LevelLog(WithUnity.Tools.Log.Level,System.String)

 The base logging used by all variants Error, Warniung etc. 



---
#### Method Log.LevelLog(WithUnity.Tools.Log.Level,System.Exception)

 The basic Logging for exception Logs the exception and all inner exceptions 



---
#### Method Log.LevelLog(WithUnity.Tools.Log.Level,System.String,System.Object[])

 BaseLogging with a format string and an array of parameters 



---
#### Method Log.Warning(System.String)

 Log a warning 

|Name | Description |
|-----|------|
|text: |The text to log|


---
#### Method Log.Warning(System.Exception)

 Log a warning message about an exception 

|Name | Description |
|-----|------|
|exception: |The exception to log|


---
#### Method Log.Warning(System.String,System.Object[])

 Log a formatted warning message 

|Name | Description |
|-----|------|
|format: |The format string used in string.Format(format, parameters)|
|parameters: |The parameters to pass to string.Format(format, parameters)|


---
#### Method Log.VerboseOk

 Log a verbose OK message 



---
#### Method Log.Verbose(System.String)

 Log a verbose message 

|Name | Description |
|-----|------|
|text: |The text to log|


---
#### Method Log.Verbose(System.Exception)

 Log a verbose message about an exception 

|Name | Description |
|-----|------|
|exception: |The exception to log|


---
#### Method Log.Verbose(System.String,System.Object[])

 Log a formatted verbose message 

|Name | Description |
|-----|------|
|format: |The format string used in string.Format(format, parameters)|
|parameters: |The parameters to pass to string.Format(format, parameters)|


---
#### Method Log.Information(System.String)

 Log an information message 

|Name | Description |
|-----|------|
|text: |The text to log|


---
#### Method Log.Information(System.Exception)

 Log an information message about an exception 

|Name | Description |
|-----|------|
|exception: |The exception to log|


---
#### Method Log.Information(System.String,System.Object[])

 Log a formatted information message 

|Name | Description |
|-----|------|
|format: |The format string used in string.Format(format, parameters)|
|parameters: |The parameters to pass to string.Format(format, parameters)|


---
#### Method Log.Debug(System.String)

 Log a Debug message 

|Name | Description |
|-----|------|
|text: |The text to log|


---
#### Method Log.Debug(System.Exception)

 Log an debug message about an exception 

|Name | Description |
|-----|------|
|exception: |The exception to log|


---
#### Method Log.Debug(System.String,System.Object[])

 Log a formatted Debug message 

|Name | Description |
|-----|------|
|format: |The format string used in string.Format(format, parameters)|
|parameters: |The parameters to pass to string.Format(format, parameters)|


---
#### Method Log.Error(System.String)

 Log an Error message 

|Name | Description |
|-----|------|
|text: |The text to log|


---
#### Method Log.Error(System.Exception)

 Log an Error message about an exception 

|Name | Description |
|-----|------|
|exception: |The exception to log|


---
#### Method Log.Error(System.String,System.Object[])

 Log a formatted Error message 

|Name | Description |
|-----|------|
|format: |The format string used in string.Format(format, parameters)|
|parameters: |The parameters to pass to string.Format(format, parameters)|


---
#### Method Log.UnknownState(System.String)

 Log an UnkownState message 

|Name | Description |
|-----|------|
|text: |The text to log|


---
#### Method Log.UnknownState(System.Exception)

 Log an UnknownState message about an exception 

|Name | Description |
|-----|------|
|exception: |The exception to log|


---
#### Method Log.UnknownState(System.String,System.Object[])

 Log a UnknownState Error message 

|Name | Description |
|-----|------|
|format: |The format string used in string.Format(format, parameters)|
|parameters: |The parameters to pass to string.Format(format, parameters)|


---
## Type MayBe`1

 Here to document the fact that we are expecting this refernce to allow null. Use Fody 

|Name | Description |
|-----|------|
|T: ||


---
#### Property MayBe`1.Value

 The Value contained by the MayBe structure. Throws an exception if their is no Value. 

[[|]]: InvalidOperationException



---
#### Property MayBe`1.HasValue

 Test if there is a Value to protect against accessing the Value if there is none 



---
#### Property MayBe`1.HasNoValue

 Test if there is not a Value: usually for error logging 



---
#### Method MayBe`1.#ctor(`0)

 Internal constructor not for general use. 

|Name | Description |
|-----|------|
|value: ||


---
#### Method MayBe`1.ToMayBe(`0)

 Explicit conversion from a base value of type T and a MayBe<T> 



>This intrinsically a static function. Instantiating another instance just to new another instance would be insane nesting and impossible with a internal constructor.



---
#### Method MayBe`1.op_Implicit(`0)~WithUnity.Tools.MayBe{`0}

 Implicit conversion from a base value of type T and a MayBe<T> 

|Name | Description |
|-----|------|
|value: |The incoming value of type T|


---
#### Method MayBe`1.op_Equality(WithUnity.Tools.MayBe{`0},`0)

 == comparison between MayBe<T> and T 

|Name | Description |
|-----|------|
|maybe: |The MayBe<T> type value|
|value: |The T type value|
**Returns**: true if the Value of maybe == value



---
#### Method MayBe`1.op_Inequality(WithUnity.Tools.MayBe{`0},`0)

 != comparison between MayBe<T> and T 

|Name | Description |
|-----|------|
|maybe: |The MayBe<T> type value|
|value: |The T type value|
**Returns**: false if the Value of maybe == value



---
#### Method MayBe`1.op_Equality(WithUnity.Tools.MayBe{`0},WithUnity.Tools.MayBe{`0})

 == comparison between MayBe<T> and another MayBe<T> 

|Name | Description |
|-----|------|
|first: |A MayBe<T> type value|
|second: |Another MayBe<T> type value|
**Returns**: first.equals(second)



---
#### Method MayBe`1.op_Inequality(WithUnity.Tools.MayBe{`0},WithUnity.Tools.MayBe{`0})

 == comparison between MayBe<T> and another MayBe<T> 

|Name | Description |
|-----|------|
|first: |A MayBe<T> type value|
|second: |Another MayBe<T> type value|
**Returns**: !first.equals(second)



---
#### Method MayBe`1.Equals(System.Object)

 Equals comparison between this and any object 

|Name | Description |
|-----|------|
|obj: |The object to compare against this|
**Returns**: true if the Value of this MayBe == obj



---
#### Method MayBe`1.GetHashCode

 returns the hash for this instance. 

**Returns**: The hash code



---
#### Method MayBe`1.Equals(WithUnity.Tools.MayBe{`0})

 Equals comparison between this and another MayBe<T>. 

|Name | Description |
|-----|------|
|other: |The other MayBe<T> to compare against this|
**Returns**: true if they are equal



---
#### Method MayBe`1.ToString

 The stanard override for ToString() for a MayBe<T>. 

**Returns**: The Value.ToString()

[[T:System.InvalidOperationException|T:System.InvalidOperationException]]: if there is no value



---
#### Method MayBe`1.Unwrap(`0)

 Returns the Value of the MayBe<T> or the default value if it is null 

**Returns**: The Value or default(T) if it has no Value



---
## Type MayBeExtensions

 An Extension method for MayBe structures. 



---
#### Method MayBeExtensions.ToMayBe``1(``0)

 Explicit Conversion to MayBe<T> 

|Name | Description |
|-----|------|
|T: |The underlying type|
|Name | Description |
|-----|------|
|value: ||
**Returns**: 



---
## Type ReflectiveTools

 Refelctive tools for accessing types indirectly 



---
#### Method ReflectiveTools.CurrentMethod

 CurrentMethod returns the name of the method or property calling it. 

**Returns**: The name of the calling method or property.



---
#### Method ReflectiveTools.CallingMethod(System.Int32)

 CallingMethod returns the name of parent method or property that are calling this function. 

|Name | Description |
|-----|------|
|stackFrames: |The number of ancestors to use. Defaults to one.|
**Returns**: The name of the calling method or property.



---
#### Method ReflectiveTools.CallingFileColumnNumber(System.Int32)

 Gets the column number in the file that contains the code that is executing. This information is typically extracted from the debugging symbols for the executable. 

|Name | Description |
|-----|------|
|stackFrames: ||
**Returns**: 



---
#### Method ReflectiveTools.CallingFileLineNumber(System.Int32)

 Gets the line number in the file that contains the code that is executing. This information is typically extracted from the debugging symbols for the executable. This call may be several calls deep 

|Name | Description |
|-----|------|
|stackFrames: ||
**Returns**: 



---
#### Method ReflectiveTools.CallingFileName(System.Int32)

 Gets the file in which this method is being called from. This call may be several calls deep 

|Name | Description |
|-----|------|
|stackFrames: |The numbers of levels deep before we report the name.|
**Returns**: The file name



---
#### Method ReflectiveTools.UnsafeCurrentMethod(System.String)

 While this method has an optional parameter it should never be used 

|Name | Description |
|-----|------|
|methodName: |Must all ways be null.|
**Returns**: The name of the calling method or the input string.



>This uses standard tools to achieve the same effect as CurrentMethod but it can be fooled if someone actually uses the optional parameter.



---
#### Method ReflectiveTools.TestFoundMethod(System.Reflection.MethodInfo,System.Type,System.Type[],System.Object[])

 Finds a method which has the same number of Generic type parameters the same return type and the same parameters including whether they match the expected generic type 

|Name | Description |
|-----|------|
|method: |The Method this method is checked against|
|returnType: |The expected return Type. This may be a generic type|
|types: |A list of generic types this method should be called with.|
|parameters: |A list of parameters this shoulf be called with.|
**Returns**: True if we find a match, false otherwise.



---
#### Method ReflectiveTools.FoundMethod(System.Reflection.MethodInfo,System.Type,System.Type[],System.Object[])

 Finds a method which has the same number of Generic type parameters the same return type and the same parameters including whether they match the expected generic type 

|Name | Description |
|-----|------|
|method: |The Method this method is checked against|
|returnType: |The expected return Type. This may be a generic type|
|types: |A list of generic types this method should be called with.|
|parameters: |A list of parameters this shoulf be called with.|
**Returns**: True if we find a match, false otherwise.



---
#### Method ReflectiveTools.VoidCall(System.Object,System.Type,System.Object[],System.String)

 Call a generic function by passing in a single Type as a parameter and the parameters as an array of objects. 

|Name | Description |
|-----|------|
|callingObject: |The object calling this method|
|t: |The Type to use|
|parameters: |An array of parameters|
|methodName: |The name of the method to call. Defaults to the same name as the calling method|
**Returns**: a generic object the return value of the method



---
#### Method ReflectiveTools.VoidCall(System.Object,System.Type[],System.Object[],System.String)

 Call a generic function by passing in the Type as a parameter 

|Name | Description |
|-----|------|
|callingObject: |The object calling this method|
|types: |The Types to use|
|parameters: |An array of parameters|
|methodName: |The name of the method to call. Defaults to the same name as the calling method|
**Returns**: a generic object the return value of the method



---
#### Method ReflectiveTools.Call(System.Object,System.Type,System.Type,System.Object[],System.String)

 Call a generic function by passing in a single Type as a parameter and the parameters as an array of objects. 

|Name | Description |
|-----|------|
|callingObject: |The object calling this method|
|returnType: |The return type of the method to call|
|t: |The Type to use|
|parameters: |An array of parameters|
|methodName: |The name of the method to call. Defaults to the same name as the calling method|
**Returns**: a generic object the return value of the method



---
#### Method ReflectiveTools.Call(System.Object,System.Type,System.Type[],System.Object[],System.String)

 Call a generic function by passing in the Type as a parameter 

|Name | Description |
|-----|------|
|callingObject: |The object calling this method|
|returnType: |The type returned|
|types: |The Types to use|
|parameters: |An array of parameters|
|methodName: |The name of the method to call. Defaults to the same name as the calling method|
**Returns**: a generic object the return value of the method



---
#### Method ReflectiveTools.ShortName(System.String)

 Get the Short name from the long name. First excludes the full path with versions and keys Then exludes the assembly informaiton. 

|Name | Description |
|-----|------|
|fullName: ||
**Returns**: 



---
#### Method ReflectiveTools.GetReferencedAssembliesFlat(System.Type)

 Intent: Get assemblies referenced by entry assembly. Not recursive. 



---
#### Method ReflectiveTools.GetLoadedAssemblies(System.Reflection.Assembly)

 Intent: Get assemblies the entry assembly currently needs. 



---
#### Method ReflectiveTools.GetLoadedAssemblies(System.Reflection.Assembly,System.Collections.Generic.IDictionary{System.String,System.String}@)

 Intent: Get assemblies the entry assembly currently needs and any assemblies that have not been found. 



---
#### Method ReflectiveTools.GetDependentAssemblies(System.Reflection.Assembly,System.Collections.Generic.IDictionary{System.String,System.Reflection.Assembly},System.Collections.Generic.IDictionary{System.String,System.String})

 Intent: Internal recursive class to get all dependent assemblies, and all dependent assemblies of dependent assemblies, etc. 



---
#### Method ReflectiveTools.MyGetMissingAssembliesRecursive(System.Reflection.Assembly)

 Intent: Get missing assemblies. 



---
## Type Result

 Wraps the results of methods into a standard class. This allows a series of method calls to be chained. This means that you can do error handling in a lot cleaner way. 



---
#### Property Result.IsSuccess

 Used to test for a successful result. 



---
#### Property Result.HasNotes

 Used to test whether the results has any notes. 



---
#### Property Result.Error

 The returned error message. This returns the first error that caused a failure. 



---
#### Property Result.Notes

 A list of Notes. 



---
#### Property Result.IsFailure

 Used to test if the result is a failure. 



---
#### Property Result.Exception

 Used to store an exception, if an exception caused the failure 



---
#### Property Result.CallingMethod

 Used for logging errors so that we can log where the error occured. 



---
#### Method Result.#ctor(WithUnity.Tools.Result)

 Constructs a new copy of a result from the previous version 

|Name | Description |
|-----|------|
|result: ||


---
#### Method Result.#ctor(System.Boolean,System.String,System.String)

 The protected constructor. Used when a logically discovered error or success was found. 

|Name | Description |
|-----|------|
|isSuccess: |Whether the result is a success|
|error: |If the result is a failure this contains the error message|
|callingMethod: |The method that is expecting the result used for logging errors.|
[[T:System.InvalidOperationException|T:System.InvalidOperationException]]: If IsSuccess and there is an error text xor It is not a success and there is not an error text.



---
#### Method Result.#ctor(System.Exception,System.String)

 The protected constructor. Used when an error occurs due to an exception. 

|Name | Description |
|-----|------|
|exception: |If the result is a failure this contains the error message|
|callingMethod: |The method that is expecting the result used for logging errors.|


---
#### Method Result.Fail(System.String)

 Static failure Result constructor method. 

|Name | Description |
|-----|------|
|message: |Some text giving the reason for the failure.|
**Returns**: 



---
#### Method Result.Fail(System.Exception)

 Static failure Result constructor method. 

|Name | Description |
|-----|------|
|exception: |The exception that caused the failure|
**Returns**: 



---
#### Method Result.Fail``1(System.String,System.String)

 Fail<T> returns a failure with the appropriate message. 

|Name | Description |
|-----|------|
|T: ||
|Name | Description |
|-----|------|
|message: |The error message you want to use|
|callingMethod: |Should be left at the default blank string. Present for internal use.|
**Returns**: A typed result with a failure report.



---
#### Method Result.Fail``1(System.Exception,System.String)

 Fail<T> returns a failure with the appropriate exception and message. 

|Name | Description |
|-----|------|
|T: ||
|Name | Description |
|-----|------|
|exception: |The excepion you need to report|
|callingMethod: |Should be left at the default blank string. Present for internal use.|
**Returns**: A typed result with a failure report.



---
#### Method Result.Ok

 The standard OK constructor method 

**Returns**: An OK result



---
#### Method Result.Ok``1(``0,System.String)

 Ok sets up a valid value of the 

|Name | Description |
|-----|------|
|T: ||
|Name | Description |
|-----|------|
|value: ||
|callingMethod: |This should be left at the default value. Only used for internal use.|
**Returns**: 



---
#### Method Result.Initialize``1(``0,System.String,System.String)

 Initialize takes the current value and if it is not Null then it does the same as OK and if it is null then it does the same as Fail 

|Name | Description |
|-----|------|
|value: |The value to set|
|error: |The error message to show if this is null|
|callingMethod: |The method receiving the Result|
**Returns**: 



---
#### Method Result.Initialize``1(WithUnity.Tools.MayBe{``0},System.String,System.String)

 Initialize takes the current value and if it is not Null then it does the same as OK and if it is null then it does the same as Fail 

|Name | Description |
|-----|------|
|value: |The value to set|
|error: |The error message to show if this is null|
|callingMethod: |The method receiving the Result|
**Returns**: The validated result



---
#### Method Result.Initialize(System.Boolean,System.String)

 Result.Initialize uses a boolean predicate to set a Result to Ok if true or fail if false. 

|Name | Description |
|-----|------|
|predicate: |The predicate to create an Ok Result or a failed Result.|
|errorMessage: |The Error message to store in case the predicate is false.|
**Returns**: The next result in the chain



>This is using Result to simplify if tests as opposed to recording a true failure. 



---
#### Method Result.AddNote(System.String)

 AddNote adds a note that can be used to allow the calling method to have more information than a task succeeded or failed. Mainly used for debugging or testing for unusual conditions. 

|Name | Description |
|-----|------|
|newNote: |A comment on an unusual event.|


---
## Type Result`1

 Result<T> is a Result that holds some data on success. 

|Name | Description |
|-----|------|
|T: |The type of data the Result holds|


---
#### Property Result`1.Value

 The Value held of Type T. 



---
#### Method Result`1.#ctor(WithUnity.Tools.Result{`0})

 The Result constructor used internally to pass on a previous Result 

|Name | Description |
|-----|------|
|result: |A previous result|


---
#### Method Result`1.#ctor(`0,System.Boolean,System.String,System.String)

 The Result constructor used internally with an error message 

|Name | Description |
|-----|------|
|value: |The value to be stored|
|isSuccess: |Whether the Result is a success (true) or a failure (false)|
|error: |Text containing an explanation of the error that caused a failure if it failed|
|callingMethod: |The method that called the public constructor for logging purposes.|


---
#### Method Result`1.#ctor(`0,System.Exception,System.String)

 The Result constructor used internally with an exception 

|Name | Description |
|-----|------|
|value: |The value to be stored|
|exception: |The exception that caused the failure|
|callingMethod: |The method that called the public constructor for logging purposes.|


---
#### Method Result`1.ToResult(`0)

 Implicitly casts an object T to the appropriate Result. Returns Fail() if null or OK if a valid value 

|Name | Description |
|-----|------|
|value: |A potentially null pointer|


>TODO: Add unit test



---
#### Method Result`1.op_Implicit(`0)~WithUnity.Tools.Result{`0}

 Implicitly casts an object T to the appropriate Result. Returns Fail() if null or OK if a valid value 

|Name | Description |
|-----|------|
|value: |A potentially null pointer|


>TODO: Add unit test



---
#### Method Result`1.Initialize(`0,System.String)

 Initialize takes the current value and if it is not Null the does the same as OK and if it is null then it does the same as Fail 

|Name | Description |
|-----|------|
|value: |The value to set|
|error: |The error message to show if this is null|
**Returns**: 



---
## Type ResultExtensions

 A series of error validation methods to call methods either OnSuccess OnFailure or OnBoth. OnSuccess usually Chains to the next item in the list unless an error occured in which case it passes on the error. OnFailure does something only if the result has already failed. The Error is passed down the list. All subsequent OnSuccess fail. OnBoth happens in either case. e.g. releasing memory or logging. Ensure queries a predicate method or value. If the predicate fails, it raises an error that is passed on or the previous value is passed on. Add as many variants as you need for the methods you use. 



---
#### Method ResultExtensions.ToResult``1(WithUnity.Tools.MayBe{``0},System.String)

 Usually used in Get data calls. Converts the object to a Result. 

|Name | Description |
|-----|------|
|T: |The type of object got|
|Name | Description |
|-----|------|
|nullable: |The MayBe reference to the object|
|errorMessage: |The Error message we want to show users or log.|
**Returns**: The result to the Get call wrapped up in a Result.



---
#### Method ResultExtensions.OnSuccess(WithUnity.Tools.Result,System.Action)

 If the result is a success then we call the Action otherwise we pass on the failed result. 

|Name | Description |
|-----|------|
|result: |The incoming result|
|action: |The void method to call.|
**Returns**: The next result in the chain.



---
#### Method ResultExtensions.OnSuccess``1(WithUnity.Tools.Result{``0},System.Action)

 OnSuccess calls an action only if previous steps are successful. This is for house keeping actions. That do not affect the Result. 

|Name | Description |
|-----|------|
|T: |The inpout and output type|
|Name | Description |
|-----|------|
|result: |The Incoming and out goiut|
|action: |The Action to run|
**Returns**: 



---
#### Method ResultExtensions.OnSuccess``1(WithUnity.Tools.Result{``0},System.Action{``0})

 OnSuccess calls a function to modify the Value. 

|Name | Description |
|-----|------|
|T: |The input and output type|
|Name | Description |
|-----|------|
|result: |The Incoming and out going|
|action: |An action that uses the value but does not change it.|
**Returns**: 



---
#### Method ResultExtensions.OnSuccess``1(WithUnity.Tools.Result{``0},System.Func{``0,``0})

 OnSuccess calls a function to modify the Value. 

|Name | Description |
|-----|------|
|T: |The input and output type|
|Name | Description |
|-----|------|
|result: |The Incoming result.|
|func: |The Func to run|
**Returns**: A replacement Result with a modifed Value.



>Note this version does not allow for error checking.



---
#### Method ResultExtensions.OnSuccess``1(WithUnity.Tools.Result{``0},System.Func{``0,WithUnity.Tools.Result{``0}})

 if all previous steps are successful, OnSuccess calls a Function on the value and returns a Result<T>. This is for house keeping actions. That do not affect the Result. 

|Name | Description |
|-----|------|
|T: |The input and output type|
|Name | Description |
|-----|------|
|result: |The Incoming and out going|
|func: |The Func to run that does error checking|
**Returns**: A new Result<T> with a modifed Value and possibly an error result.



>Note this version does not allow for error checking.



---
#### Method ResultExtensions.OnSuccess``1(WithUnity.Tools.Result{``0},System.Func{``0,WithUnity.Tools.Result})

 if all previous steps are successful, OnSuccess calls a function and returns. This is for house keeping actions. That do not affect the Result. 

|Name | Description |
|-----|------|
|T: |The input and output Result<T>, unless func fails|
|Name | Description |
|-----|------|
|result: |The Incoming and out going result|
|func: |The Function to run|
**Returns**:  1. If it has previously failed it returns that failure; 2. If the Func was a failure then it returns that failure; 3. If the Func was a success is passes on the Previous the Result with the previous Value.



---
#### Method ResultExtensions.OnSuccess``2(WithUnity.Tools.Result{``0},System.Func{``0,``1})

 if all previous steps are successful, OnSuccess use a function that changes input type Result<TInput> to output type Resukt<TOutput> 

|Name | Description |
|-----|------|
|TInput: |The incoming Result<TInput>|
|Name | Description |
|-----|------|
|TOutput: |The returned Result<TOutput>|
|Name | Description |
|-----|------|
|result: |The incoming result|
|func: |The function used to swap between the two types|


>There is no error handling in the Func and it has to succeed. If you want error handling add a similar static method replacing Func<TInput, TOutput> with Func<TInput, Result<TOutput>>

**Returns**:  1. If it has previously a failed, it returns that failure. Otherwise it calls the func; 2. If returns a new Result<TOutput> containing the return value of the Func. 



---
#### Method ResultExtensions.OnSuccess``2(WithUnity.Tools.Result{``0},System.Func{``0,WithUnity.Tools.Result{``1}})

 if all previous steps are successful, OnSuccess use a function that changes input type Result<TInput> to output type Resukt<TOutput> 

|Name | Description |
|-----|------|
|TInput: |The incoming Result<TInput>|
|Name | Description |
|-----|------|
|TOutput: |The returned Result<TOutput>|
|Name | Description |
|-----|------|
|result: |The incoming result|
|func: |The function takes incoming TInput and directly returns a Result<<TOutput>> that can be returned directly|


>This version has error handling

**Returns**:  1. If it has previously a failed, it returns that failure. Otherwise it calls the func; 2. If returns a new Result<TOutput> containing the return value of the Func. 



---
#### Method ResultExtensions.OnSuccess(WithUnity.Tools.Result,System.Func{WithUnity.Tools.Result})

 If the previous result has not failed then we call the Function returning a Result otherwise we pass the failed result. down the list. 

|Name | Description |
|-----|------|
|result: |The incoming result|
|func: |The method returning a Result to call.|
**Returns**: The failure result passed down the chain or the return result of the Function.



---
#### Method ResultExtensions.OnFailure(WithUnity.Tools.Result,System.Action)

 If the result has failed then we call the Action otherwise we pass on the failed result. 

|Name | Description |
|-----|------|
|result: |The incoming result|
|action: |The void method to call. Usually error reporting or returning an error.|
**Returns**: Passes the Result down the chain.



---
#### Method ResultExtensions.OnFailureGeneratedDefault``1(WithUnity.Tools.Result{``0},System.Func{WithUnity.Tools.Result{``0}})

 OnFailure it generate a replacement default value. OnSuccess it is left as was. If you need to handle success before generating a default always call OnSuccess first because OnFailureGeneratedDefault will also be a success. Note: the default value will normally be a success. At least I cannot think of a reason why you would generate a bad default. 

|Name | Description |
|-----|------|
|T: |The data wrapped in this Result<T>|
|Name | Description |
|-----|------|
|result: |The incoming result|
|func: |A function to generate a default replacement value.|
**Returns**: On Failure it generates a new valid Result, On Success it Passes the Result down the chain.



---
#### Method ResultExtensions.OnFailure``1(WithUnity.Tools.Result{``0},System.Action)

 If the result has failed then we call the Action otherwise we pass on the failed result. 

|Name | Description |
|-----|------|
|T: |The data wrapped in this Result<T>|
|Name | Description |
|-----|------|
|result: |The incoming result|
|action: |The void method to call. Usually error reporting or returning an error.|
**Returns**: Passes the Result down the chain.



---
#### Method ResultExtensions.OnFailure(WithUnity.Tools.Result,System.Action{System.String})

 If the result has failed then we call the Action otherwise we pass on the failed result. 

|Name | Description |
|-----|------|
|result: |The incoming result|
|action: |A method call that takes the current Error message.|
**Returns**: Passes the Result down the chain.



---
#### Method ResultExtensions.OnFailure``1(WithUnity.Tools.Result{``0},System.Action{System.String})

 If the result has failed then we Report the Error value otherwise we pass on the result. 

|Name | Description |
|-----|------|
|T: |The type of the contained data.|
|Name | Description |
|-----|------|
|result: |The incoming result|
|action: |The void method to call. That OnFailure reports the error or throws an exception.|
**Returns**: Passes the result down the chain.



---
#### Method ResultExtensions.OnBoth(WithUnity.Tools.Result,System.Action{WithUnity.Tools.Result})

 We call the Action regardless of the Result and pass the Result down the Chain. 

|Name | Description |
|-----|------|
|result: |The incoming result|
|action: |The void method to call. that uses or affects the result. Usually closing files or handles...|
**Returns**: Passes the Result down the chain.



---
#### Method ResultExtensions.OnBoth(WithUnity.Tools.Result,System.Action)

 We call the Action regardless of the Result and pass the Result down the Chain. 

|Name | Description |
|-----|------|
|result: |The incoming result|
|action: |The void method to call. That has no use for the result. Usually closing files or handles...|
**Returns**: Passes the Result down the chain.



>TODO Unit tests



---
#### Method ResultExtensions.OnBoth``1(WithUnity.Tools.Result,System.Func{WithUnity.Tools.Result,``0})

 We call the Function regardless of the Result and pass in the Result. It then returns a value of Type T. 

|Name | Description |
|-----|------|
|T: |The type the func returns|
|Name | Description |
|-----|------|
|result: |The incoming result|
|func: |The Func to call on that Result that will return a value of type T|
**Returns**: Data of an unspecified Type



---
#### Method ResultExtensions.Ensure``1(WithUnity.Tools.Result{``0},System.Func{``0,System.Boolean},System.String)

 Ensures that a predicate is true if not it raises an Error 

|Name | Description |
|-----|------|
|T: |Incoming wrapped type|
|Name | Description |
|-----|------|
|result: |Incoming Resukt object|
|predicate: |The loical predicate method to perform|
|errorMessage: |Error message|
**Returns**: The next result in the chain



---
#### Method ResultExtensions.Ensure``1(WithUnity.Tools.Result{``0},System.Func{System.Boolean},System.String)

 Ensures that a predicate is true if not it raises an Error, using a lamda function. 

|Name | Description |
|-----|------|
|T: |Type of wrapped data|
|Name | Description |
|-----|------|
|result: |Incoming Result object|
|predicate: |The logical predicate method to perform|
|errorMessage: |Error message|
**Returns**: The next result in the chain



---
#### Method ResultExtensions.Ensure``1(WithUnity.Tools.Result{``0},System.Boolean,System.String)

 Ensures that a predicate is true if not it raises an Error 

|Name | Description |
|-----|------|
|T: |Type of wrapped data|
|Name | Description |
|-----|------|
|result: |Incoming Resukt object|
|predicate: |The logical predicate method to perform|
|errorMessage: |Error message|
**Returns**: The next result in the chain



---
#### Method ResultExtensions.Ensure(WithUnity.Tools.Result,System.Func{System.Boolean},System.String)

 Ensures that a predicate is true if not it raises an Error. Using a function or Lamda. 

|Name | Description |
|-----|------|
|result: |Incoming Result object|
|predicate: |The logical predicate method to perform|
|errorMessage: |Error message|
**Returns**: The next result in the chain



---
#### Method ResultExtensions.Ensure(WithUnity.Tools.Result,System.Boolean,System.String)

 Ensures that a predicate is true if not it raises an Error 

|Name | Description |
|-----|------|
|result: |Incoming Result object|
|predicate: |The logical predicate as a boolean|
|errorMessage: |Error message|
**Returns**: The next result in the chain



---
#### Method ResultExtensions.Ensure``1(WithUnity.Tools.Result{``0},System.Boolean,System.String,System.Boolean@,System.Collections.Generic.ICollection{System.String})

 Ensures that a predicate is true if not it raises an Error. It also returns the result of the Predicate in an out parameter and adds any errors to a collection of error strings. 

|Name | Description |
|-----|------|
|T: |Type of wrapped data|
|Name | Description |
|-----|------|
|result: |Incoming Result object|
|predicate: |The local predicate method to perform|
|errorMessage: |Error message|
|storedResult: |Allows the result to be stored for separate processing|
|errors: |A collection of error messages to store all errors.|
**Returns**: The next result in the chain



---
#### Method ResultExtensions.Ensure(WithUnity.Tools.Result,WithUnity.Tools.MayBe{System.Object[]},WithUnity.Tools.MayBe{System.Object[]},System.Func{System.Object,System.Object,System.Boolean},System.Func{System.Int32,System.String},System.Func{System.Int32,System.Int32,System.String},System.Func{System.Int32,System.Object,System.Object,System.String})

 Ensures that a predicate is true if not it raises an Error over the whole of an array. The input arrays may be null and so we test for that too and return true if they are both null or wrongLengthErrorMessage if only one of them is null. 

|Name | Description |
|-----|------|
|result: |Incoming Resukt object|
|array1: |The first array to be compared (Maybe null)|
|array2: |The second array to be compared against (Maybe null).|
|predicate: |The logical predicate method to perform. Note null arrays are considered equal.|
|NullErrorMessage: |A Function to return which parameter array [1 or 2] is null assuming only one of them is null.|
|wrongLengthErrorMessage: |A wrong length method. The length of the two arrays will be passed in.|
|predicateErrorMessage: |A method to return the predicate Error Message. This allows the use of parameters without forcing you to use them. The parameters passed in are the offset and the two objects that differ at that offset.|
**Returns**: The next result in the chain



---
#### Method ResultExtensions.Initialize``1(WithUnity.Tools.MayBe{``0},System.String)

 Initialize ensures that the data is a valid object and returns a Result.Fail<T> if it is not or returns the A Result.Ok<T> if the constructor is OK. 

|Name | Description |
|-----|------|
|T: |Type of wrapped data|
|Name | Description |
|-----|------|
|data: |data is a structure MayBe<T*gt; that can contain null or a value|
|errorMessage: |Error message|
**Returns**: The next result in the chain



>TODO: Add unit tests



---
#### Method ResultExtensions.StripData``1(WithUnity.Tools.Result{``0})

 Converts a Result<<T>> to a Result, because. Some results need to hide the data. 

|Name | Description |
|-----|------|
|T: |The type of the underlying data|
|Name | Description |
|-----|------|
|result: |The incoming result.|
**Returns**: A simple Result with out the data.



---
## Type ValueProperties.EmailAddress

 A Value Property for validating email addresses. 



> This is provided with minimal validataion for an EmailAddress. You may want to create your own variant with different validation. 



---
#### Method ValueProperties.EmailAddress.ValidateEmailAddress(WithUnity.Tools.MayBe{System.String})

 Trims any white space and validates an email address 

|Name | Description |
|-----|------|
|emailAddressValue: ||


> This verifies: that the email is not [[|]]; that email is at leaset 3 characters long; that the email does not start with an @ sign; that the email does not end with an @ sign; that the email contains 1 and only 1 @ sign; 



---
#### Method ValueProperties.EmailAddress.#ctor(WithUnity.Tools.MayBe{System.String},System.Boolean)

 The main constructor for validating email addresses. This is used by implicit casts, when expecting a valid EmailAddress and throwing an exception on failure is a reasonable thing to do. 

|Name | Description |
|-----|------|
|emailAddressValue: ||
|validateAndThrowOnFailure: |Validates the email address and throws InvalidCastException when true. Does nothing when false|
[[T:System.InvalidCastException|T:System.InvalidCastException]]: This is thrown if the string passed in is not a valid email with the appropriate error.



> This verifies: that the email is not [[|]]; that email is at leaset 3 characters long; that the email does not start with an @ sign; that the email does not end with an @ sign; that the email contains 1 and only 1 @ sign; 



---
#### Method ValueProperties.EmailAddress.CreateEmailAddress(WithUnity.Tools.MayBe{System.String})

 Use CreateEmailAddress for more concise error handling in cases where you are suspicious of the email source such as it comes from user input. 

|Name | Description |
|-----|------|
|emailAddressValue: ||
**Returns**: Result<EmailAddress>



> This verifies: that the email is not [[|]]; that email is at leaset 3 characters long; that the email does not start with an @ sign; that the email does not end with an @ sign; that the email contains 1 and only 1 @ sign; 



---
#### Method ValueProperties.EmailAddress.op_Implicit(System.String)~WithUnity.Tools.ValueProperties.EmailAddress

 Implicit conversion for simpler readability to MayBe<String>. Only use this implicit cast when expecting a valid EmailAddress and throwing an exception is a reasonable thing to do. 

|Name | Description |
|-----|------|
|possibleEmail: ||
[[T:System.InvalidCastException|T:System.InvalidCastException]]: This is thrown if the string passed in is not a valid email with the appropriate error.



>This implicitly casts a sting parameter to a MayBe<string> parameter and then use that implicit operator with the MayBe<string> parameter. 



---
#### Method ValueProperties.EmailAddress.op_Implicit(WithUnity.Tools.MayBe{System.String})~WithUnity.Tools.ValueProperties.EmailAddress

 Implicit conversion from May<Be> string to be more explicit about whether the string is null. Only use this implicit cast when expecting a valid EmailAddress and throwing an exception is a reasonable thing to do. 

|Name | Description |
|-----|------|
|possibleEmail: ||
[[T:System.InvalidCastException|T:System.InvalidCastException]]: This is thrown if the string passed in is not a valid email with the appropriate error.



---
#### Property ValueProperties.EmailAddress.EmailAddressValue

 The Valid EmailAddress value 



---
#### Method ValueProperties.EmailAddress.EqualsCore(System.String)

 Note: by the time this is called all checks for null and NoValue have already been called so we just deal with the equality of valid objects. 

|Name | Description |
|-----|------|
|other: ||
**Returns**: whether the two are equal



---
#### Method ValueProperties.EmailAddress.GetHashCode

 ValueProperty inner GetHash routine 

**Returns**: an Integer hash of the emailaddress class



---
## Type ValueProperties.FileExtension

 FileExtension ValueProperty 



> This is provided with minimal validataion for a file fileExtension. You may want to create your own variant with different validation. 



---
#### Method ValueProperties.FileExtension.ValidateFileExtension(WithUnity.Tools.MayBe{System.String})

 Validates a file fileExtension 

|Name | Description |
|-----|------|
|fileExtension: ||


> This method trims whitespace from the fileExtension and removes leading "*." and then ensures the file Extension is: Not Null The string does not have leading or trailing white space characters. Mot empty string Does not contain ['\' | '/' | '.' | ':'] 

**Returns**: A Result@lt;string> that contains the file extension or an error message on validation failure



---
#### Method ValueProperties.FileExtension.#ctor(WithUnity.Tools.MayBe{System.String},System.Boolean)

 The main constructor for a FileExtension. If validateAndThrowOnFailure is true it calls TrimAndValidateFileExtension and throws an InvalidCastException. 

|Name | Description |
|-----|------|
|fileExtension: |The initial file fileExtension primitive|
|validateAndThrowOnFailure: |Validates the email address and throws InvalidCastException when true. Does nothing when false|
[[T:System.InvalidCastException|T:System.InvalidCastException]]: The message will contain any validation errors



---
#### Method ValueProperties.FileExtension.CreateFileExtension(WithUnity.Tools.MayBe{System.String})

 Use CreateFileExtension for more concise error handling on input validation where you are suspicious of the file fileExtension, e.g. user input. 

|Name | Description |
|-----|------|
|fileExtension: |A suspect file Extension|
**Returns**: Result<FileExtension>



> This verifies: that the email is not [[|]]; that email is at leaset 3 characters long; that the email does not start with an @ sign; that the email does not end with an @ sign; that the email contains 1 and only 1 @ sign; 



---
#### Method ValueProperties.FileExtension.op_Implicit(System.String)~WithUnity.Tools.ValueProperties.FileExtension

 Implicit conversion for simpler readability to MayBe<String>. Use this implicit cast when expecting a valid FileExtension and throwing an exception is a reasonable thing to do. 

|Name | Description |
|-----|------|
|possibleFileExtension: ||
[[T:System.InvalidCastException|T:System.InvalidCastException]]: This is thrown if the string passed in is not a valid email with the appropriate error.



---
#### Method ValueProperties.FileExtension.op_Implicit(WithUnity.Tools.MayBe{System.String})~WithUnity.Tools.ValueProperties.FileExtension

 Implicit conversion from May<Be> string to be more explicit about whether the string is null. Use this implicit cast when expecting a valid FileExtension and throwing an exception is a reasonable thing to do. 

|Name | Description |
|-----|------|
|possibleFileExtension: ||
[[T:System.InvalidCastException|T:System.InvalidCastException]]: This is thrown if the string passed in is not a valid email with the appropriate error.



---
#### Property ValueProperties.FileExtension.FileExtensionValue

 The Valid File Extension's value 



---
#### Method ValueProperties.FileExtension.EqualsCore(System.String)

 Note: by the time this is called all checks for null and NoValue have already been called so we just deal with the equality of valid objects. 

|Name | Description |
|-----|------|
|other: ||
**Returns**: whether the two are equal



---
#### Method ValueProperties.FileExtension.GetHashCode

 ValueProperty inner GetHash routine 

**Returns**: an Integer hash of the FileExtension class



---
## Type ValueProperties.UnicodeCharacter16

 A string representing one Unicode Character based on 1 or 2 16 bit Unicode code points 



> This is provided with minimal validataion for a uunicode character. You may want to create your own variant with different validation. 



---
#### Method ValueProperties.UnicodeCharacter16.ValidateUnicodeCharacter16(WithUnity.Tools.MayBe{System.String})

 Validates a single UTF16 character this may be 1 or 2 16 bit code points 

|Name | Description |
|-----|------|
|input: |The inpout to verify|


> This verifies that: the email is not [[|]]; email is at leaset 3 characters long; the email does not start with an @ sign; the email does not end with an @ sign; the email contains 1 and only 1 @ sign; the email does not need trimming. 



---
#### Method ValueProperties.UnicodeCharacter16.#ctor(WithUnity.Tools.MayBe{System.String})

 Constructor for validating the unicode character 

|Name | Description |
|-----|------|
|input: |Maybe a single Unicode Character in a string|
[[T:System.InvalidCastException|T:System.InvalidCastException]]: If the string is not a Unicode character

[[T:System.ArgumentNullException|T:System.ArgumentNullException]]: If the string is null



> This verifies: That the string past in is a single Unicode Character; Whether high and low surrogate code points are used in the correct order and 

[[T:System.InvalidCastException|T:System.InvalidCastException]]: If the string is not a single Unicode character, Including a null string.



---
#### Property ValueProperties.UnicodeCharacter16.Utf32

 Utf32 equivalent 



---
#### Property ValueProperties.UnicodeCharacter16.Utf8

 The UTF 8 equivalent. 



---
## Type ValueProperties.UnicodeString16

 A C# string representing a valid UTF-16 string 



> This checks that all the code points in the string form valid unicode characters there are all valid Unicode characters. 



---
#### Method ValueProperties.UnicodeString16.#ctor(WithUnity.Tools.MayBe{System.String})

 Constructor for validating the unicode string 

|Name | Description |
|-----|------|
|value: ||
[[T:System.InvalidCastException|T:System.InvalidCastException]]: If the string is not a Unicode string

[[T:System.ArgumentNullException|T:System.ArgumentNullException]]: If the string is null



> This verifies: That the string past in is a valid Unicode string; By checking if high and low surrogate code points are used in the correct order and the single code points are used as single characters. 



---
#### Method ValueProperties.UnicodeString16.GetUnicodeCharacter(WithUnity.Tools.MayBe{System.String})

 Goes through a string returning valid Unicode characters 

|Name | Description |
|-----|------|
|input: |A MayBe<string> input string.|
**Returns**: The next valid Unicode Character

[[T:System.InvalidCastException|T:System.InvalidCastException]]: Is thrown if the string is invalid Unicode



---
#### Property ValueProperties.UnicodeString16.Utf32String

 Provides Utf32 equivalent. 



---
#### Property ValueProperties.UnicodeString16.Utf8String

 Provides Utf32 equivalent. 



---
## Type ValueProperty`2

 A ValueProperty is a ValueObject which has only one Property. Like ValueObjects ValueProperties must be Immutable. Hence their name. A ValueProperty contain a single object for example a string with all the validation of that value and Equals and GetHashCode methods. The reason a ValueProperty is different from a ValueObject Is that it adds implicit assignment between a value and the ValueProperty and implicit assignment between a MayBe<Value> and the ValueProperty It also allows Comparisons between the held value and the native value or a MayBe<value> The code here give the standard base implementation of a ValueProperty The various Equals and GetHashCode methods, while keeping to the DRY principal as best we can. 

|Name | Description |
|-----|------|
|T: |The Value Property type e.g. public sealed class EmailAddress : ValueProperty<EmailAddress, string>|
|Name | Description |
|-----|------|
|HeldType: |The Type of the held value; for a EmailAddress that is a string|


---
#### Property ValueProperty`2.Value

 The underlying read only data. Maybe<HeldType> may be. 



---
#### Method ValueProperty`2.#ctor(`1)

 A protected constructor. It is not intended that this should be used directly but always as an implicit cast. 

|Name | Description |
|-----|------|
|value: |An instance of the underlying data. This is never expected to be null(Protected by NullGuard)|


---
#### Method ValueProperty`2.#ctor(WithUnity.Tools.MayBe{`1})

 A protected constructor. It is not intended that this should be used directly but always as an implicit cast. 

|Name | Description |
|-----|------|
|value: |A MayBe<HeldType> structure, which may contain an instance of the underlying data or null.|


---
#### Method ValueProperty`2.ToValueProperty(`1)

 Explicit Conversion from HeldType to ValueProperty<T, HeldType>> 

|Name | Description |
|-----|------|
|value: ||
**Returns**: 



---
#### Method ValueProperty`2.op_Implicit(`1)~WithUnity.Tools.ValueProperty{`0,`1}

 Implicit cast from a HeldType instance to ValueProperty<T, HeldType>. 

|Name | Description |
|-----|------|
|value: |The incoming value|
**Returns**: The implicitly cast ValueProperty(Must pass validation or throw an exception)



---
#### Method ValueProperty`2.ToValueProperty(WithUnity.Tools.MayBe{`1})

 Explicit Conversion from HeldType to ValueProperty<T, HeldType>> 

|Name | Description |
|-----|------|
|value: ||
**Returns**: 



---
#### Method ValueProperty`2.op_Implicit(WithUnity.Tools.MayBe{`1})~WithUnity.Tools.ValueProperty{`0,`1}

 Implicit cast from a MayBe<HeldType> to ValueProperty<T, HeldType>. 

|Name | Description |
|-----|------|
|value: |The incoming value|
**Returns**: The implicitly cast ValueProperty(Must pass validation or throw an exception)



---
#### Method ValueProperty`2.Equals(System.Object)

 Returns true if equal or false if not equal 

|Name | Description |
|-----|------|
|obj: |The object being compared|
**Returns**: Returns Whether the Value of the ValueProperty is equal to: 1. if the obj is HeldType the obj; 2. if the obj is a MayBe<HeldType> the Value of the MayBe<HeldType>; 3. if the obj is a a ValueProperty<T, HeldType> the Value of the ValueProperty<T, HeldType> 4. or false.



---
#### Method ValueProperty`2.EqualsCore(`1)

 This should be overrriden 

|Name | Description |
|-----|------|
|other: ||
**Returns**: 



---
#### Method ValueProperty`2.GetHashCode

 GetHashCode calls an overridden method to Get the Hash code based on the HeldType. 

**Returns**: 



---
#### Method ValueProperty`2.op_Equality(WithUnity.Tools.ValueProperty{`0,`1},WithUnity.Tools.ValueProperty{`0,`1})

 == operator for ValueProperty<T, HeldType> 

|Name | Description |
|-----|------|
|a: |a ValueProperty<T, HeldType>|
|b: |another ValueProperty<T, HeldType>|
**Returns**: true if the two Values are the same



---
#### Method ValueProperty`2.op_Inequality(WithUnity.Tools.ValueProperty{`0,`1},WithUnity.Tools.ValueProperty{`0,`1})

 != operator for ValueProperty<T, HeldType> 

|Name | Description |
|-----|------|
|a: |a ValueProperty<T, HeldType>|
|b: |another ValueProperty<T, HeldType>|
**Returns**: false if the two Values are the same



---
#### Method ValueProperty`2.op_Equality(WithUnity.Tools.ValueProperty{`0,`1},WithUnity.Tools.MayBe{`1})

 == operator comparing the Value of a ValueProperty<T, HeldType> and the Value of a MayBe<HeldType> 

|Name | Description |
|-----|------|
|a: |a ValueProperty<T, HeldType>|
|b: |a MayBe<HeldTypey> of the same type|
**Returns**: true if the HeldType Value for both are equal



---
#### Method ValueProperty`2.op_Inequality(WithUnity.Tools.ValueProperty{`0,`1},WithUnity.Tools.MayBe{`1})

 == operator comparing the Value of a ValueProperty<T, HeldType> and the Value of a MayBe<HeldType> 

|Name | Description |
|-----|------|
|a: |a ValueProperty<T, HeldType>|
|b: |a MayBe<HeldTypey> of the same type|
**Returns**: false if the HeldType Value for both are equal



---
#### Method ValueProperty`2.op_Equality(WithUnity.Tools.MayBe{`1},WithUnity.Tools.ValueProperty{`0,`1})

 == operator comparing a MayBe<HeldType> and a ValueProperty<T, HeldType> 

|Name | Description |
|-----|------|
|a: |a MayBe<HeldType>|
|b: |a ValueProperty<T, HeldType>|
**Returns**: true if the HeldType Value for both are equal



---
#### Method ValueProperty`2.op_Inequality(WithUnity.Tools.MayBe{`1},WithUnity.Tools.ValueProperty{`0,`1})

 != operator comparing a MayBe<HeldType> and a ValueProperty<T, HeldType> 

|Name | Description |
|-----|------|
|a: |a MayBe<HeldType>|
|b: |a ValueProperty<T, HeldType>|
**Returns**: false if the HeldType Value for both are equal



---
#### Method ValueProperty`2.op_Equality(WithUnity.Tools.ValueProperty{`0,`1},`1)

 == operator comparing a ValueProperty<T, HeldType> and a HeldType 

|Name | Description |
|-----|------|
|a: |a ValueProperty<T, HeldType>|
|b: |HeldType of the same type as the MayBe<HeldType>.Value|
**Returns**: true if the value of HeldType is e qual or both are null



---
#### Method ValueProperty`2.op_Inequality(WithUnity.Tools.ValueProperty{`0,`1},`1)

 != operator comparing a ValueProperty<T, HeldType> and a HeldType 

|Name | Description |
|-----|------|
|a: |a ValueProperty<T, HeldType>|
|b: |HeldType of the same type|
**Returns**: true if the calue of HeldType is e qual or both are null



---
#### Method ValueProperty`2.op_Equality(`1,WithUnity.Tools.ValueProperty{`0,`1})

 == operator comparing a HeldType and a ValueProperty<T, HeldType> 

|Name | Description |
|-----|------|
|a: |HeldType of the same type|
|b: |a ValueProperty<T, HeldType>|
**Returns**: true if the calue of HeldType is e qual or both are null



---
#### Method ValueProperty`2.op_Inequality(`1,WithUnity.Tools.ValueProperty{`0,`1})

 != operator comparing a HeldType and a ValueProperty<T, HeldType> 

|Name | Description |
|-----|------|
|a: |HeldType of the same type|
|b: |a ValueProperty<T, HeldType>|
**Returns**: true if the calue of HeldType is e qual or both are null



---


