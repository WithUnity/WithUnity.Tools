#WithUnity.Tools

## Version History
|Date|Version|Author|Details|
|----|-------|------|-------|
|2021/09/03|1.0.0.0|Robin Murison|First public publication|

##Abbstract
Blesec.Tools Make C# nearer a Functional language by allowing method declaration to be much more honest and adding fluent Validation.
We intend to add Task templates to provide appropriate parallel processing. Although C# Tasks are fine for the job Using of the templates would allow cross cutting logging and other concerns.

The two main features to make method declaration more honest. Use the MayBe structure when there is an expectation that null maybe a valid reference as a parameter.
WHen there is a possibility that a method may fail, use the Result<T> classe to return either the valid object or the relevant error message. 
The backlog has a mission to add the option to use error codes to allow localized of error messages, where these messages need to be publically displayed.

There is also a Log class that can be initialized to provide automatic logging from the Result class. With Verbose successes and Error level failures failures and informational notes.

There is also the ValueProperty to create readonly properties. Used to add validation to individual properties and to make using the ValueProperty class transparent to the user.

There are also a few implementation of a ValueProperty showing how to validation should be implemeted in the constructor and should throw InvalidCastException when invalid data is passed in.

ValueProperty can be implicitly used as the underlying type, this is to make them just as usable as the underlying data type.

Result uses some Reflexive tools to log from where errors occur in the Logging. 
The Logging and the Reflexive tools are independent of using the Result validation mechanism.

## Detailed documentation 
The exact details can be found in the [.\WithUnity.Tools\WithUnity.Tools.xml"](XML documentation).

## Unit Tests
All code written for With Unity is unit tested.
These can be found in the WithUnity.Tools.Test project.
At the time of writing this has not been externally auditted and may need improvement.