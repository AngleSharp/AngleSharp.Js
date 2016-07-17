![logo](https://raw.githubusercontent.com/AngleSharp/AngleSharp.Scripting/master/header.png)

AngleSharp.Scripting
====================

AngleSharp.Scripting extends the core AngleSharp library with some useful scripting capabilities. This repository contains multiple packages. All of them are quickly presented in this file.

Current status of this repository:

[![Build status](https://img.shields.io/appveyor/ci/FlorianRappl/AngleSharp-Scripting.svg?style=flat-square)](https://ci.appveyor.com/project/FlorianRappl/AngleSharp-Scripting)
[![Issues open](https://img.shields.io/github/issues/AngleSharp/AngleSharp.Scripting.svg?style=flat-square)](https://github.com/AngleSharp/AngleSharp.Scripting/issues)

AngleSharp.Scripting.CSharp
---------------------------

The repository contains helpers for dynamic evaluation using the DLR. The dynamic helpers enable JavaScript-like programming, without any requirements on casting. Nevertheless, as in a JavaScript, you'll need to be sure that the instance you are dealing with really exposes the API you are using.

Additionally this library has eventually the goal of providing a very fancy way of integrating Roslyn, scriptcs or any other C# script engine. In HTML files that have been parsed with a configuration that uses `WithCSharp`, scripts can be provided in using the `text/c-sharp` type. Such scripts will then be transformed and evaluated by the provided C# script engine.

The transformation should set the `this` pointer to the `IWindow` instance. Additionally the DLR will be used. Finally, it will be possible to also reference other libraries in the script. The script tag might be used to include external libraries via NuGet.

**Status** Highly experimental and most things are not implemented.

AngleSharp.Scripting.JavaScript
---------------------------

The repository contains DOM bindings for the *Jint* JavaScript engine. *Jint* is fully ECMAScript 5 compatible and provides the basis for evaluating JavaScripts in the context of the AngleSharp DOM representation.

The library comes with a service that exposes `WithJavaScript` to `IConfiguration`. This enables automatic evaluation of `script` elements that have a valid JavaScript type (or without any explicit type, since JavaScript is the default one). The DOM bindings are generated on the fly via reflection. Since *Jint* is interpreting JavaScript, the library can be consumed as a PCL. The downside is that the performance is definitely worse than any compiled JavaScript engine would deliver. For most scripts that should not be a big issue.

**Status** Currently leaving experimental stage and becoming alpha ready.

[![Nuget count](https://img.shields.io/nuget/v/AngleSharp.Scripting.Javascript.svg?style=flat-square)](https://www.nuget.org/packages/AngleSharp.Scripting.Javascript/)
[![Nuget downloads](https://img.shields.io/nuget/dt/AngleSharp.Scripting.Javascript.svg?style=flat-square)](https://www.nuget.org/packages/AngleSharp.Scripting.JavaScript/)

Some legal stuff
----------------

The MIT License (MIT)

Copyright (c) 2015 - 2016 AngleSharp

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
