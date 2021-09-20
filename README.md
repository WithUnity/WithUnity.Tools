#WithUnity.Tools

## Version History
|Date|Version|Author|Details|
|----|-------|------|-------|
|2021/09/03|1.0.0.0|Robin Murison|First public publication|
|2021/09/16|1.0.0.1|Robin Murison|Corrected ReadMe.md and added more comprehensive Unit Tests for the Log class.|
|2021/09/20|1.0.2.0-alpha|Robin Murison|Preparing for publiucation on NUGET|

##Abstract
WithUnity.Tools Make C# nearer a Functional language by allowing method declaration to be much more honest and adding fluent Validation.

##Abbstract
WithUnity.Tools Make C# nearer a Functional language by allowing method declaration to be much more honest and adding fluent Validation.
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
Please read WithUnity.Tools.MD for detailed type and method information.


## License Agreement
MIT License

Copyright (c) 2015 - 2021 Robin Murison

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

## Unit Tests
All code written for With Unity is unit tested.
These can be found in the WithUnity.Tools.Test project.
At the time of writing this has not been externally auditted and may need improvement.